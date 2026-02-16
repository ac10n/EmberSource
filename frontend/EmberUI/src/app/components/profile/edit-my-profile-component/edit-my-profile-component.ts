import { Component, inject, Inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ProfileService } from '../../../services/profile.service';
import { ProfileResponse, UpdateProfileRequest } from '../../../models/contract-models';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { invalid, ModelManager, ModelPresenterComponent, SaveResult, valid, ValidateModelForSave } from '../../../models/model-state';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'ember-edit-my-profile-component',
  templateUrl: './edit-my-profile-component.html',
  styleUrls: ['../../forms.scss', './edit-my-profile-component.scss'],
  imports: [FormsModule]
})
export class EditMyProfileComponent implements OnInit {

  private snackBar = inject(MatSnackBar);
  manager?: ModelManager<ProfileResponse, UpdateProfileRequest>;

  get profile() {
    return this.manager?.edit;
  }

  get state() {
    return this.manager?.state;
  }

  constructor(
    private readonly profileService: ProfileService,
    private readonly authService: AuthService,
    private readonly router: Router,
    private dialogRef: MatDialogRef<EditMyProfileComponent, SaveResult<ProfileResponse | UpdateProfileRequest>>,
    @Inject(MAT_DIALOG_DATA) public data: ModelPresenterComponent<ProfileResponse, UpdateProfileRequest>,
  ) {
    this.manager = data?.manager;
    if (!this.manager) {
      this.snackBar.open('Message archived');
      this.dialogRef.close();
    }
    this.manager?.prepareForEdit({
      action: 'edit',
      mapFetchModelToEdit(fetch) {
        return {
          fullName: fetch.fullName,
          birthYear: fetch.birthYear,
          jurisdiction: fetch.jurisdiction
        };
      },
    });
  }

  ngOnInit(): void {
    this.manager?.load('initial');
  }

  validate: ValidateModelForSave<ProfileResponse, UpdateProfileRequest> = ({ edit }) => {
    if (!edit.fullName || !edit.birthYear || !edit.jurisdiction) {
      return invalid('Please fill in all required fields.');
    }
    return valid(edit);
  }

  onSubmit(formValid: boolean | null) {
    this.manager?.save({
      validate: this.validate,
      successCallback: (model) => {
        this.dialogRef.close({ result: 'success', data: model });
      },
      saveFunction: this.profileService.updateProfile,
    });
  }
}
