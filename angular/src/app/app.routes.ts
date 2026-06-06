import { authGuard, permissionGuard } from '@abp/ng.core';
import { Routes } from '@angular/router';
import { COURIERS_ROUTES } from 'modules/couriers/src/public-api';

export const APP_ROUTES: Routes = [
  // ── Home ──────────────────────────────────────────────────────
  {
    path: '',
    pathMatch: 'full',
    loadComponent: () => import('./home/home.component').then(c => c.HomeComponent),
  },

  // ── Parcels ───────────────────────────────────────────────────
  {
    path: 'parcels',
    loadComponent: () =>
      import('./components/parcels.component1').then(c => c.ParcelsComponent),
    canActivate: [authGuard, permissionGuard],
    data: { requiredPolicy: 'ShippingManagement.Parcels' },
  },
  {
    path: 'parcels/create',
    loadComponent: () =>
      import('./components/parcel-create.component').then(c => c.ParcelCreateComponent),
    canActivate: [authGuard, permissionGuard],
    data: { requiredPolicy: 'ShippingManagement.Parcels.Create' },
  },

  // ── Tracking ──────────────────────────────────────────────────
  {
    path: 'tracking',
    loadComponent: () =>
      import('./components/track.component1').then(c => c.TrackComponent),
  },

  // ── Couriers ──────────────────────────────────────────────────
  {
    path: 'couriers',
    children: COURIERS_ROUTES,
    // canActivate: [authGuard, permissionGuard],
    data: { requiredPolicy: 'Couriers.Couriers' },
  },

  // ── ABP Modules ───────────────────────────────────────────────
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
];