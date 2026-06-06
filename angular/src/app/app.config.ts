// import { provideCouriers } from '@couriers/config';
// import {  } from '@couriers/config';
// import {  } from '@couriers/config';
import { eThemeSharedComponents, provideAbpCore, ReplaceableComponentsService, withOptions } from '@abp/ng.core';
import { provideAbpOAuth } from '@abp/ng.oauth';
import { provideSettingManagementConfig } from '@abp/ng.setting-management/config';
import { provideFeatureManagementConfig } from '@abp/ng.feature-management';
import { provideAbpThemeShared,} from '@abp/ng.theme.shared';
import { provideIdentityConfig } from '@abp/ng.identity/config';
import { provideAccountConfig } from '@abp/ng.account/config';
import { provideTenantManagementConfig } from '@abp/ng.tenant-management/config';
import { registerLocaleForEsBuild } from '@abp/ng.core/locale';
import { provideThemeLeptonX } from '@abp/ng.theme.lepton-x';
import { provideSideMenuLayout } from '@abp/ng.theme.lepton-x/layouts';
import { provideLogo, withEnvironmentOptions } from "@abp/ng.theme.shared";
import { APP_INITIALIZER, ApplicationConfig } from '@angular/core';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideRouter } from '@angular/router';
import { environment } from '../environments/environment';
import { APP_ROUTES } from './app.routes';
import { APP_ROUTE_PROVIDER } from './route.provider';
import { FOOTER_PROVIDER } from './footer/footer.config';
import { provideParcels } from '@modules/parcels/config';
import { eThemeLeptonXComponents } from '@abp/ng.theme.lepton-x'; 
import { CustomSidebarComponent } from './layouts/my-custom-sidebar.component';
import { provideCouriers } from 'modules/couriers/config/src/providers';

export function initializeApp(replaceableComponents: ReplaceableComponentsService) {
  return () => {
    replaceableComponents.add({
      component: CustomSidebarComponent,
      key: eThemeLeptonXComponents.Sidebar, 
    });

    // 2. إضافة استبدال الـ Navbar (لأن LeptonX في وضعية الـ Side Menu يعتبرها أحياناً Navbar)
    replaceableComponents.add({
      component: CustomSidebarComponent,
      key: eThemeLeptonXComponents.Navbar, 
    });
  };
}

export const appConfig: ApplicationConfig = {
  providers: [
    {
    provide: APP_INITIALIZER,
    // provideCouriers(),
    useFactory: initializeApp,
    deps: [ReplaceableComponentsService],
    multi: true,
  },
provideParcels(),
  provideCouriers(),
    provideRouter(APP_ROUTES),
    APP_ROUTE_PROVIDER,
    FOOTER_PROVIDER,
    provideAnimations(),
    provideAbpCore(
      withOptions({
        environment,
        registerLocaleFn: registerLocaleForEsBuild(),
      }),
    ),
    provideAbpOAuth(),
    provideIdentityConfig(),
    provideSettingManagementConfig(),
    provideFeatureManagementConfig(),
    provideThemeLeptonX(),
    provideSideMenuLayout(),
    provideLogo(withEnvironmentOptions(environment)),
    provideAccountConfig(),
    provideTenantManagementConfig(),
    provideAbpThemeShared(),
    provideParcels(),
  ]
};
