import { Component, OnInit, OnDestroy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { ConfigStateService, LocalizationPipe,CoreModule, SessionStateService } from '@abp/ng.core';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-custom-sidebar',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive, LocalizationPipe,CoreModule],
  templateUrl: './my-custom-sidebar.component.html',
  styleUrls: ['./custom-sidebar.component.scss']
})
export class CustomSidebarComponent implements OnInit, OnDestroy {

  navRailCollapsed = false;
  parcelsNavOpen   = false;
  fleetNavOpen     = false;
  settingsNavOpen  = false;

  isRtl = true; // افتراضي عربي

  private destroy$ = new Subject<void>();
private sessionState = inject(SessionStateService);
private configState = inject(ConfigStateService);
  // constructor(private configState: ConfigStateService) {}

  ngOnInit(): void {
    // مراقبة لغة النظام وتحديد الاتجاه ديناميكياً
    this.sessionState.getLanguage$().subscribe(() => {
      const currentCulture = this.sessionState.getLanguage();
      // إذا كانت الثقافة تبدأ بـ 'ar' (مثل ar, ar-YE, ar-SA) يتم تفعيل الـ RTL تلقائياً
      this.isRtl = currentCulture ? currentCulture.startsWith('ar') : true;
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  toggleNavRail() {
    this.navRailCollapsed = !this.navRailCollapsed;
  }

  toggleParcelsNav(event: Event) {
    event.preventDefault();
    this.parcelsNavOpen = !this.parcelsNavOpen;
  }

  toggleFleetNav(event: Event) {
    event.preventDefault();
    this.fleetNavOpen = !this.fleetNavOpen;
  }

  toggleSettingsNav(event: Event) {
    event.preventDefault();
    this.settingsNavOpen = !this.settingsNavOpen;
  }

  // toggleNavRail()              { this.navRailCollapsed = !this.navRailCollapsed; }
  // toggleParcelsNav(e: Event)   { e.preventDefault(); this.parcelsNavOpen  = !this.parcelsNavOpen;  }
  // toggleFleetNav(e: Event)     { e.preventDefault(); this.fleetNavOpen    = !this.fleetNavOpen;    }
  // toggleSettingsNav(e: Event)  { e.preventDefault(); this.settingsNavOpen = !this.settingsNavOpen; }
}