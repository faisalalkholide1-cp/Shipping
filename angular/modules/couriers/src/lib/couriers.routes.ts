import { RouterOutletComponent } from '@abp/ng.core';
import { Routes } from '@angular/router';

export const COURIERS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./components/couriers.component').then(c => c.CouriersComponent),
  },
];