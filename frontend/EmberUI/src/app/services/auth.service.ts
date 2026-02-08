import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';

export interface LoginRequest {
  email: string;
  password: string;
  rememberMe: boolean;
}

export interface TokenResponse {
  accessToken: string;
  refreshToken?: string; // if your backend returns it in body; omit if using HttpOnly cookie
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private accessTokenKey = 'access_token';
  private refreshTokenKey = 'refresh_token';

  private accessToken$ = new BehaviorSubject<string | null>(this.getAccessToken());

  constructor(private http: HttpClient) {}

  login(req: LoginRequest): Observable<TokenResponse> {
    return this.http.post<TokenResponse>(`${environment.apiBaseUrl}/auth/login`, req, {
      // if refresh token is HttpOnly cookie, you likely need this:
      withCredentials: true,
    }).pipe(
      tap(res => {
        this.setAccessToken(res.accessToken);

        // If refresh token comes in JSON body (less ideal):
        if (res.refreshToken) this.setRefreshToken(res.refreshToken);
      })
    );
  }

  refresh(): Observable<TokenResponse> {
    // If refresh token is in HttpOnly cookie, no body needed; just withCredentials
    // If refresh token is stored client-side, send it in body.
    const refreshToken = this.getRefreshToken();

    return this.http.post<TokenResponse>(`${environment.apiBaseUrl}/auth/refresh`,
      refreshToken ? { refreshToken } : {},
      { withCredentials: true }
    ).pipe(
      tap(res => {
        this.setAccessToken(res.accessToken);
        if (res.refreshToken) this.setRefreshToken(res.refreshToken);
      })
    );
  }

  logout(): Observable<void> {
    // optionally call backend to invalidate refresh token
    return this.http.post<void>(`${environment.apiBaseUrl}/auth/logout`, {}, { withCredentials: true })
      .pipe(tap(() => this.clearTokens()));
  }

  // --- token storage ---
  getAccessToken(): string | null {
    return localStorage.getItem(this.accessTokenKey);
  }

  private setAccessToken(token: string) {
    localStorage.setItem(this.accessTokenKey, token);
    this.accessToken$.next(token);
  }

  private getRefreshToken(): string | null {
    return localStorage.getItem(this.refreshTokenKey);
  }

  private setRefreshToken(token: string) {
    localStorage.setItem(this.refreshTokenKey, token);
  }

  clearTokens() {
    localStorage.removeItem(this.accessTokenKey);
    localStorage.removeItem(this.refreshTokenKey);
    this.accessToken$.next(null);
  }

  accessTokenChanges() {
    return this.accessToken$.asObservable();
  }
}