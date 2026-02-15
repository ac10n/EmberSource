import { Component, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ProfileService } from '../../../services/profile.service';
import { UpdateProfileRequest } from '../../../models/contract-models';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'ember-edit-my-profile-component',
  templateUrl: './edit-my-profile-component.html',
  styleUrls: ['../../forms.scss', './edit-my-profile-component.scss'],
  imports: [FormsModule]
})
export class EditMyProfileComponent implements OnInit {
  fullName = signal<string | null>(null);
  birthYear = signal<number | null>(null);
  jurisdiction = signal<string | null>(null);

  oldPassword = signal<string | null>(null);
  newPassword = signal<string | null>(null);
  confirmPassword = signal<string | null>(null);

  loading = signal<boolean>(false);
  submitted = signal<boolean>(false);
  errorText = signal<string>('');

  constructor(
    private readonly profileService: ProfileService,
    private readonly authService: AuthService,
    private readonly router: Router,
  ) {
  }

  ngOnInit(): void {
    this.loading.set(true);
    this.profileService.getProfile()
      .subscribe({
        next: (profile) => {
          this.fullName.set(profile.fullName);
          this.birthYear.set(profile.birthYear);
          this.jurisdiction.set(profile.jurisdiction);
          this.loading.set(false);
        },
        error: (err: unknown) => {
          this.loading.set(false);
          this.errorText.set(
            (err as any)?.error?.message ?? 'Failed to load profile. Please try again.');
        },
      });
  }

  onSubmit(formValid: boolean | null) {
    this.submitted.set(true);
    this.errorText.set('');
    if (!formValid) return;

    this.loading.set(true);

    const fullName = this.fullName();
    const birthYear = this.birthYear();
    const jurisdiction = this.jurisdiction();

    if (!fullName || !birthYear || !jurisdiction) {
      this.loading.set(false);
      this.errorText.set('Please fill in all required fields.');
      return;
    }

    const updateData: UpdateProfileRequest = {
      fullName: fullName,
      birthYear: birthYear,
      jurisdiction: jurisdiction
    };

    var oldPassword = this.oldPassword();
    const newPassword = this.newPassword();
    const confirmPassword = this.confirmPassword();

    if (newPassword) {
      if (newPassword !== confirmPassword) {
        this.loading.set(false);
        this.errorText.set('New Password and Confirm Password do not match.');
        return;
      }

      if (!oldPassword) {
        this.loading.set(false);
        this.errorText.set('Please enter your current password to set a new password.');
        return;
      }


      updateData.newPassword = newPassword;
      updateData.oldPassword = oldPassword;
    }

    this.profileService.updateProfile(updateData)
      .subscribe({
        next: () => {
          this.loading.set(false);
          // Handle successful profile update, e.g., show a success message
        },
        error: (err: unknown) => {
          this.loading.set(false);
          this.errorText.set(
            (err as any)?.error?.message ?? 'Profile update failed. Please try again.');
        },
      });
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
