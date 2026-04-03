import { effect, signal, WritableSignal } from "@angular/core";
import { Observable } from "rxjs";
import { UpdateResult } from "./contract-models";

type ModelLoadReason = 'initial' | 'change' | 'refresh' | 'submit';

export type ModelStateMode = {
    state: 'empty';
} | {
    state: 'loading';
    reason: ModelLoadReason;
} | {
    state: 'error';
    reason: 'network' | 'permission' | 'unknown' | 'validation';
} | {
    state: 'loaded';
    reason: ModelLoadReason;
}

export type SaveResult<TFetch> = {
    result: 'success';
    data?: TFetch;
} | {
    result: 'failure';
    reason: 'logical' | 'permission' | 'transient' | 'unknown';
    error?: string;
}

export type ModelState<TFetch, TSave> = {
    fetched: WritableSignal<TFetch | null>;
    edit: WritableSignal<TSave | null>;
    state: WritableSignal<ModelStateMode>;
    error: WritableSignal<string>;
    lastUpdated: WritableSignal<Date | null>;
    submitted: WritableSignal<boolean>;
    editAction: WritableSignal<EditAction | null>;
}

export type EditAction = 'new' | 'edit';

export type PrepareForEditParams<TFetch, TSave> = {
    action: 'new';
    createEmptyModel: () => TSave;
} | {
    action: 'edit';
    mapFetchModelToEdit: (fetch: TFetch) => TSave;
}

export type ModelManager<TFetch, TSave> = ModelState<TFetch, TSave> & {
    load: (reason: ModelLoadReason) => void;
    prepareForEdit: (params: PrepareForEditParams<TFetch, TSave>) => void;
    save: (saveParams: SaveParams<TFetch, TSave>) => void;
}

export const modelState = <TFetch, TSave>(): ModelState<TFetch, TSave> => ({
    fetched: signal<TFetch | null>(null),
    edit: signal<TSave | null>(null),
    state: signal<ModelStateMode>({ state: 'empty' }),
    error: signal<string>(''),
    lastUpdated: signal<Date | null>(null),
    submitted: signal<boolean>(false),
    editAction: signal<EditAction | null>(null),
});

export type Valid<TUpdate> = {
    valid: true;
    model: TUpdate;
}
export type Invalid = {
    valid: false;
    error: string;
}
export const valid = <TUpdate>(model: TUpdate): Valid<TUpdate> => ({ valid: true, model });
export const invalid = (error: string): Invalid => ({
    valid: false,
    error
});

export type ValidationResult<TUpdate> = Valid<TUpdate> | Invalid;

export type ValidationParams<TFetch, TSave> = {
    action: 'new';
    edit: TSave;
    state: ModelState<TFetch, TSave>;
} | {
    action: 'edit';
    fetched: TFetch;
    edit: TSave;
    state: ModelState<TFetch, TSave>;
};

export type ValidateModelForSave<TFetch, TSave> = (validationParams: ValidationParams<TFetch, TSave>) => ValidationResult<TSave>;

export type SaveParams<TFetch, TSave> = {
    validate: ValidateModelForSave<TFetch, TSave>;
    successCallback: (model?: TFetch | TSave) => void;
    saveFunction: (model: TSave) => Observable<UpdateResult<TFetch>>;
}

export const modelManager = <TFetch, TSave>(loadFn: () => Observable<TFetch>): ModelManager<TFetch, TSave> => {
    const state = modelState<TFetch, TSave>();

    return {
        ...state,
        load: async (reason: ModelLoadReason) => {
            state.state.set({ state: 'loading', reason: reason });
            state.submitted.set(false);
            loadFn().subscribe({
                next: (data) => {
                    state.fetched.set(data);
                    state.state.set({ state: 'loaded', reason: reason });
                    state.error.set('');
                    state.lastUpdated.set(new Date());
                },
                error: (err) => {
                    state.state.set({ state: 'error', reason: 'network' });
                    state.error.set('Failed to load data');
                }
            });
        },
        prepareForEdit: (params: PrepareForEditParams<TFetch, TSave>) => {
            state.editAction.set(params.action);
            if (params.action === 'new') {
                state.edit.set(params.createEmptyModel());
            }
            else {
                effect(() => {
                    var fetched = state.fetched();
                    if (!fetched) {
                        state.edit.set(null);
                        return;
                    }
                    state.edit.set(params.mapFetchModelToEdit(fetched));
                });
            }
        },
        save: ({ validate, successCallback, saveFunction }: SaveParams<TFetch, TSave>) => {
            state.submitted.set(true);
            state.state.set({ state: 'loading', reason: 'submit' });

            const action = state.editAction();
            if (!action) {
                state.error.set('No edit action specified');
                state.state.set({ state: 'error', reason: 'validation' });
                return;
            }

            const fetched = state.fetched();
            if (action == 'edit' && !fetched) {
                const error = `The Model Object is ${fetched}. This is a programming error and should not happen.`;
                state.error.set(error);
                state.state.set({
                    state: "error",
                    reason: "validation"
                });
                return;
            }

            const edit = state.edit();
            if (!edit) {
                state.error.set(`The Model Object is ${edit}. This is a programming error and should not happen.`);
                state.state.set({ state: 'error', reason: 'validation' });
                return;
            }

            const validationParams: ValidationParams<TFetch, TSave> = action === 'new' ? {
                action: 'new',
                edit: edit as TSave,
                state
            } : {
                action: 'edit',
                fetched: fetched as TFetch,
                edit: edit as TSave,
                state
            };

            const validationResult = validate(validationParams);

            if (!validationResult.valid) {
                state.error.set(validationResult.error);
                state.state.set({
                    state: "error",
                    reason: "validation"
                });
                return;
            }

            saveFunction(edit).subscribe({
                next: (result) => {
                    if (result.result === 'success') {
                        if (result.data) {
                            state.fetched.set(result.data);
                        }
                        state.state.set({ state: 'loaded', reason: 'submit' });
                        state.error.set('');
                        state.lastUpdated.set(new Date());
                        successCallback(result.data || fetched || edit);
                    } else {
                        state.state.set({ state: 'error', reason: result.reason === 'logical' ? 'permission' : 'unknown' });
                        state.error.set(result.reason || 'Failed to save data');
                    }
                },
                error: (err) => {
                    state.state.set({ state: 'error', reason: 'network' });
                    state.error.set('Failed to save data');
                }
            });
        },
    };
};

export abstract class ModelPresenterComponent<TFetch, TSave = TFetch> {
    manager?: ModelManager<TFetch, TSave>;
}
