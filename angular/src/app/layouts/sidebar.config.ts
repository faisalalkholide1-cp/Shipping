import { provideAppInitializer, inject } from '@angular/core';
import { ReplaceableComponentsService } from '@abp/ng.core';
import { eThemeLeptonXComponents } from '@abp/ng.theme.lepton-x';
import { CustomSidebarComponent } from './my-custom-sidebar.component';

function initCustomNavItems() {
  const replaceableComponents = inject(ReplaceableComponentsService);
  replaceableComponents.add({
    key: eThemeLeptonXComponents.Sidebar,
    component: CustomSidebarComponent,
  });

  try {
    // Diagnostic: mark body and log to console to confirm registration at runtime
    if (typeof document !== 'undefined') {
      document.body.classList.add('custom-sidebar-registered');
      // eslint-disable-next-line no-console
      console.log('[SIDEBAR_PROVIDER] CustomSidebarComponent registered');
    }
  } catch (e) {
    // noop
  }
}

export const SIDEBAR_PROVIDER = [
  provideAppInitializer(() => {
    initCustomNavItems();
  }),
];