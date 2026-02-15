import { ProfileResponse } from '../../../models/contract-models';
import { modelManager, ModelPresenterComponent } from '../../../models/model-state';
import { ProfileService } from '../../../services/profile.service';
import { Component, forwardRef, model, OnInit } from '@angular/core';

@Component({
  selector: 'ember-show-profile',
  imports: [],
  templateUrl: './show-profile.html',
  styleUrl: './show-profile.scss',
  providers: [{ provide: ModelPresenterComponent<any>, useExisting: forwardRef(() => ShowProfile) }]
})
export class ShowProfile extends ModelPresenterComponent<ProfileResponse> implements OnInit {

  profileId = model<string | null>(null);
  override manager = modelManager<ProfileResponse>(() =>
    this.profileService.getProfile({
      profileId: this.profileId() ?? undefined
    })
  );

  get profile() {
    return this.manager.state.data;
  }

  constructor(
    private readonly profileService: ProfileService
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
}
