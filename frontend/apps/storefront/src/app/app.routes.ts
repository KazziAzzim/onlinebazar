import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', loadComponent: () => import('./features/home/home.component').then(m => m.HomeComponent), title: 'Home | OnlineBazar' },
  { path: 'listings', loadComponent: () => import('./features/listings/listings.component').then(m => m.ListingsComponent), title: 'Listings | OnlineBazar' },
  { path: 'about', loadComponent: () => import('./features/home/about.component').then(m => m.AboutComponent), title: 'About | OnlineBazar' },
  { path: '**', redirectTo: '' }
];
