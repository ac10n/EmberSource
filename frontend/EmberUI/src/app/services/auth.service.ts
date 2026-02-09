import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';

export interface LoginRequest {
  userName: string;
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

  constructor(
    private readonly http: HttpClient
  ) {
  }

  private storage: Storage = localStorage;

  private setStorage(rememberMe: boolean) {
    this.storage = rememberMe ? localStorage : sessionStorage;
  }

  login(req: LoginRequest): Observable<TokenResponse> {
    this.setStorage(req.rememberMe);

    return this.http.post<TokenResponse>(`${environment.apiBaseUrl}/v01/auth/login`, req, {
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
    return this.http.post<void>(`${environment.apiBaseUrl}/v01/auth/logout`, {}, { withCredentials: true })
      .pipe(tap(() => this.clearTokens()));
  }

  getAccessToken(): string | null {
    return sessionStorage.getItem(this.accessTokenKey) ?? localStorage.getItem(this.accessTokenKey);
  }

  private setAccessToken(token: string) {
    this.storage.setItem(this.accessTokenKey, token);
    this.accessToken$.next(token);
  }

  private getRefreshToken(): string | null {
    return this.storage.getItem(this.refreshTokenKey);
  }

  private setRefreshToken(token: string) {
    this.storage.setItem(this.refreshTokenKey, token);
  }

  clearTokens() {
    this.storage.removeItem(this.accessTokenKey);
    this.storage.removeItem(this.refreshTokenKey);
    this.accessToken$.next(null);
  }

  accessTokenChanges() {
    return this.accessToken$.asObservable();
  }
}