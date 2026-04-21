import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: 'login', loadComponent: () => import('./features/dashboard/login.component').then(m => m.LoginComponent) },
  { path: '', loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent) },
  { path: 'listings', loadComponent: () => import('./features/listings/admin-listings.component').then(m => m.AdminListingsComponent) },
  { path: 'content', loadComponent: () => import('./features/content/content-pages.component').then(m => m.ContentPagesComponent) }
];
