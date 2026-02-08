import { Injectable } from '@angular/core';
import {
  HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError, BehaviorSubject, filter, switchMap, take, catchError } from 'rxjs';
import { AuthService } from './auth.service';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';

@Injectable()
export class RefreshInterceptor implements HttpInterceptor {
  private refreshing = false;
  private refreshedToken$ = new BehaviorSubject<string | null>(null);

  constructor(private auth: AuthService, private router: Router) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (!req.url.startsWith(environment.apiBaseUrl)) {
      return next.handle(req);
    }

    return next.handle(req).pipe(
      catchError(err => {
        if (!(err instanceof HttpErrorResponse)) return throwError(() => err);

        // Don’t try to refresh on login/refresh endpoints themselves
        const isAuthCall =
          req.url.includes('/auth/login') || req.url.includes('/auth/refresh');

        if (err.status !== 401 || isAuthCall) {
          return throwError(() => err);
        }

        // If already refreshing, wait until it finishes then retry
        if (this.refreshing) {
          return this.refreshedToken$.pipe(
            filter(t => t != null),
            take(1),
            switchMap(() => next.handle(this.addAuthHeader(req)))
          );
        }

        this.refreshing = true;
        this.refreshedToken$.next(null);

        return this.auth.refresh().pipe(
          switchMap(res => {
            this.refreshing = false;
            this.refreshedToken$.next(res.accessToken);
            return next.handle(this.addAuthHeader(req));
          }),
          catchError(refreshErr => {
            this.refreshing = false;
            this.auth.clearTokens();
            this.router.navigate(['/login']);
            return throwError(() => refreshErr);
          })
        );
      })
    );
  }

  private addAuthHeader(req: HttpRequest<any>) {
    const token = this.auth.getAccessToken();
    if (!token) return req;
    return req.clone({ setHeaders: { Authorization: `Bearer ${token}` }, withCredentials: true });
  }
}
