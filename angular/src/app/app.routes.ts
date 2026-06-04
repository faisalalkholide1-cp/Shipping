import { authGuard, permissionGuard } from '@abp/ng.core';
import { Routes } from '@angular/router';

export const APP_ROUTES: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadComponent: () => import('./home/home.component').then(c => c.HomeComponent),
  },
  {
    path: 'account',
    loadChildren: () => import('@abp/ng.account').then(c => c.createRoutes()),
  },
  {
    path: 'identity',
    loadChildren: () => import('@abp/ng.identity').then(c => c.createRoutes()),
  },
  {
    path: 'tenant-management',
    loadChildren: () => import('@abp/ng.tenant-management').then(c => c.createRoutes()),
  },
  {
    path: 'setting-management',
    loadChildren: () => import('@abp/ng.setting-management').then(c => c.createRoutes()),
  },
  {
    path: 'books',
    loadComponent: () => import('./book/book.component').then(c => c.BookComponent),
    canActivate: [authGuard, permissionGuard],
  },
  {
    path: 'parcels',
    loadComponent: () => import('./components/parcels.component1').then(c => c.ParcelsComponent),
    canActivate: [authGuard, permissionGuard],
  },
  {
    path: 'parcelsCreate',
    loadComponent: () => import('./components/parcel-create.component').then(c => c.ParcelCreateComponent),
    canActivate: [authGuard, permissionGuard],
  },
  {
    path: 'tracking',
    loadComponent: () => import('./components/track.component1').then(c => c.TrackComponent),
  },
  // {
  //   path: 'parcels',
  //   loadChildren: () => import('@modules/parcels').then(m => m.PARCELS_ROUTES),
  //   canActivate: [authGuard],
  // },
  // {
  //   path: 'track',
  //   loadChildren: () => import('@modules/parcels').then(m => m.TRACK_ROUTES),
  // },
];
