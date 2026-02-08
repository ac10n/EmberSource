import { Routes } from '@angular/router';
import { HomeComponent } from './home/home';
import { CreateAccountComponent } from './create-account/create-account';
import { LoginComponent } from './components/auth/login/login';
import { DashboardComponent } from './dashboard/dashboard';

export const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'home', component: HomeComponent },
    { path: 'create-account', component: CreateAccountComponent },
    { path: 'login', component: LoginComponent },
    { path: 'dashboard', component: DashboardComponent },    
];
