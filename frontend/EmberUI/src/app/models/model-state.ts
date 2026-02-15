import { signal, WritableSignal } from "@angular/core";
import { Observable } from "rxjs";

type ModelLoadReason = 'initial' | 'change' | 'refresh' | 'submit';

export type ModelStateMode = {
    state: 'empty';
} | {
    state: 'loading';
    reason: ModelLoadReason;
} | {
    state: 'error';
    reason: 'network' | 'permission' | 'unknown';
} | {
    state: 'loaded';
    reason: ModelLoadReason;
}

export type UpdateResult<TModel> = {
    result: 'success';
    data?: TModel;
} | {
    result: 'failure';
    reason: 'logical' | 'permission' | 'transient' | 'unknown';
    error?: string;
}

export type ModelState<TModel> = {
    data: WritableSignal<TModel | null>;
    state: WritableSignal<ModelStateMode>;
    error: WritableSignal<string>;
    lastUpdated: WritableSignal<Date | null>;
}

export type ModelManager<TModel> = {
    state: ModelState<TModel>;
    load: (reason: ModelLoadReason) => void;
    save?: (model: TModel, callback: () => void) => void;
}

export const modelState = <TModel>(): ModelState<TModel> => ({
    data: signal<TModel | null>(null),
    state: signal<ModelStateMode>({ state: 'empty' }),
    error: signal<string>(''),
    lastUpdated: signal<Date | null>(null),
});

export const modelManager = <TModel>(loadFn: () => Observable<TModel>, saveFn?: (model: TModel) => Observable<UpdateResult<TModel>>): ModelManager<TModel> => {
    const state = modelState<TModel>();

    return {
        state: state,
        load: async (reason: ModelLoadReason) => {
            state.state.set({ state: 'loading', reason: reason });
            loadFn().subscribe({
                next: (data) => {
                    state.data.set(data);
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
        save: (model: TModel, callback: () => void) => {
            if (!saveFn) {
                throw new Error('Save function not provided');
            }
            state.state.set({ state: 'loading', reason: 'submit' });
            saveFn(model).subscribe({
                next: (result) => {
                    if (result.result === 'success') {
                        if (result.data) {
                            state.data.set(result.data);
                        }
                        state.state.set({ state: 'loaded', reason: 'submit' });
                        state.error.set('');
                        state.lastUpdated.set(new Date());
                        callback();
                    } else {
                        state.state.set({ state: 'error', reason: result.reason === 'logical' ? 'permission' : 'unknown' });
                        state.error.set(result.error || 'Failed to save data');
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

export abstract class ModelPresenterComponent<TModel> {
    manager?: ModelManager<TModel>;
}
