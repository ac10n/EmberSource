import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { finalize } from 'rxjs/operators';
import { AuthService } from '../../../services/auth.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.html',
  styleUrl: './login.scss',
  imports: [FormsModule],
})
export class LoginComponent {
  username = '';
  password = '';
  rememberMe = true;

  loading = false;
  submitted = false;
  errorText = '';

  constructor(
    private auth: AuthService,
    private router: Router
  ) {
  }

  onSubmit(formValid: boolean | null) {
    this.submitted = true;
    this.errorText = '';

    if (!formValid) return;

    this.loading = true;

    this.auth
      .login({
        email: this.username,
        password: this.password,
        rememberMe: this.rememberMe,
      })
      .pipe(finalize(() => (this.loading = false)))
      .subscribe({
        next: () => this.router.navigateByUrl('/'),
        error: (err) => {
          this.errorText =
            err?.error?.message ?? 'Login failed. Please check your credentials.';
        },
      });
  }
}
