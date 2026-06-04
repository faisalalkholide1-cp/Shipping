import { RouterOutletComponent } from '@abp/ng.core';
import { Routes } from '@angular/router';

export const PARCELS_ROUTES: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: RouterOutletComponent,
    children: [
      // {
      //   path: '',
      //   loadComponent: () =>
      //     import('./components/parcels.component').then(c => c.ParcelsComponent),
      // },
    ],
  },
];

export const TRACK_ROUTES: Routes = [
  // {
  //   path: '',
  //   loadComponent: () => import('./components/track.component').then(c => c.TrackComponent),
  // },
];
