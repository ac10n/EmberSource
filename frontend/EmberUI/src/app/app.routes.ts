import { Routes } from '@angular/router';
import { HomeComponent } from './home/home';
import { CreateAccountComponent } from './create-account/create-account';
import { LoginComponent } from './components/auth/login/login';
import { DashboardComponent } from './dashboard/dashboard';
import { EditMyProfileComponent } from './components/profile/edit-my-profile-component/edit-my-profile-component';
import { authGuard } from './services/auth.guard';
import { guestGuard } from './services/guest.guard';

export const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'home', component: HomeComponent },
    { path: 'create-account', component: CreateAccountComponent },
    { path: 'login', component: LoginComponent, canActivate: [guestGuard] },
    { path: 'dashboard', component: DashboardComponent },
    { path: 'edit-profile', component: EditMyProfileComponent, canActivate: [authGuard] },
];
