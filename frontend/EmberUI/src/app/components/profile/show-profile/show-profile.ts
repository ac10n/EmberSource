import { ProfileResponse, UpdateProfileRequest } from '../../../models/contract-models';
import { modelManager, ModelPresenterComponent, SaveResult } from '../../../models/model-state';
import { ProfileService } from '../../../services/profile.service';
import { Component, forwardRef, model, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatDialog } from '@angular/material/dialog';
import { EditMyProfileComponent } from '../edit-my-profile-component/edit-my-profile-component';

@Component({
  selector: 'ember-show-profile',
  imports: [MatCardModule],
  templateUrl: './show-profile.html',
  styleUrl: './show-profile.scss',
  providers: [{ provide: ModelPresenterComponent<any>, useExisting: forwardRef(() => ShowProfile) }]
})
export class ShowProfile extends ModelPresenterComponent<ProfileResponse, UpdateProfileRequest> implements OnInit {

  profileId = model<string | null>(null);
  override manager = modelManager<ProfileResponse, UpdateProfileRequest>(() =>
    this.profileService.getProfile({
      profileId: this.profileId() ?? undefined
    })
  );

  get profile() {
    return this.manager.fetched;
  }

  constructor(
    private readonly profileService: ProfileService,
    private dialog: MatDialog
  ) {
    super();
    this.profileId.subscribe((id) => {
      this.profile.set(null);
      this.manager.load('change');
    });
  }

  ngOnInit(): void {
    this.manager.load('initial');
  }

  edit() {
    const dialogRef = this.dialog.open(EditMyProfileComponent, {
      width: '400px',
      data: { manager: this.manager }
    });

    dialogRef.afterClosed().subscribe((result?: SaveResult<ProfileResponse>) => {
      if (result?.result === 'success') {
        this.manager.load('refresh');
      }
    });
  }
}
