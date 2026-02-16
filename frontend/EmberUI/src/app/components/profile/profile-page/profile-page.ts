import { Component, signal } from '@angular/core';
import { EditMyProfileComponent } from "../edit-my-profile-component/edit-my-profile-component";
import { ProfileService } from '../../../services/profile.service';
import { ProfileResponse } from '../../../models/contract-models';
import { ModelPresenter } from "../../common/model-presenter/model-presenter";
import { ShowProfile } from '../show-profile/show-profile';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'ember-profile-page',
  imports: [EditMyProfileComponent, ModelPresenter, ShowProfile],
  templateUrl: './profile-page.html',
  styleUrl: './profile-page.scss',
})
export class ProfileComponent {

  profile = signal<ProfileResponse | null>(null);
  profileLoading = signal<boolean>(true);
  profileError = signal<string>('');

  constructor(
    private readonly profileService: ProfileService,
    private readonly authService: AuthService,
    private readonly router: Router,
  ) {
  }

  logout() {
    this.authService.logout().subscribe({
      next: () => {
        this.router.navigate(['/']);
      },
      error: (err) => {
        console.error('Logout failed', err);
      }
    });
  }
}
