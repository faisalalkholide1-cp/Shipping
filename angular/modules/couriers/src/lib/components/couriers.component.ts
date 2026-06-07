import { Component, inject, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal, NgbModalRef, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmationService } from '@abp/ng.theme.shared';
import { LocalizationPipe } from '@abp/ng.core';
import { CourierListFilterDto, CourierProfileDto, CourierService, CreateCourierDto, UpdateCourierDto } from 'src/app/proxy/couriers/application';
import { CourierStatus, courierStatusOptions } from 'src/app/proxy/couriers/domain';

@Component({
  selector: 'app-couriers',
  standalone: true,
  templateUrl: './couriers.component.html',
  styleUrls: ['./couriers.component.scss'],
  imports: [CommonModule, ReactiveFormsModule, FormsModule, NgbModule],
})
export class CouriersComponent implements OnInit {
  private courierService = inject(CourierService);
  private fb             = inject(FormBuilder);
  private modalService   = inject(NgbModal);
  private confirmation   = inject(ConfirmationService);

  @ViewChild('courierModal') courierModal!: TemplateRef<any>;

  // ── Data ─────────────────────────────────────────────────────
  couriers: CourierProfileDto[]   = [];
  totalCount                       = 0;
  isLoading                        = false;
  selectedCourier: CourierProfileDto | null = null;

  // ── Pagination ────────────────────────────────────────────────
  currentPage  = 1;
  pageSize     = 10;

  // ── Filters ───────────────────────────────────────────────────
  filterText      = '';
  filterZone      = '';
  filterStatus: CourierStatus | null = null;
  filterAvailable: boolean | null    = null;

  // ── Modal ─────────────────────────────────────────────────────
  modalRef?:  NgbModalRef;
  isEditMode  = false;
  editingId?: string;
  form!:      FormGroup;
  isSaving    = false;

  // ── Enums ─────────────────────────────────────────────────────
  CourierStatus   = CourierStatus;
  statusOptions   = courierStatusOptions;

  // ─────────────────────────────────────────────────────────────

  ngOnInit(): void {
    this.buildForm();
    this.loadCouriers();
  }

  buildForm(): void {
    this.form = this.fb.group({
      fullName: ['', Validators.required],
      phone:    ['', Validators.required],
      email:    ['', [Validators.required, Validators.email]],
      zone:     ['', Validators.required],
      password: [''],
    });
  }

  // ── Load ──────────────────────────────────────────────────────
  loadCouriers(): void {
    this.isLoading = true;
    const filter: CourierListFilterDto = {
      filter:         this.filterText    || undefined,
      zone:           this.filterZone    || undefined,
      status:         this.filterStatus  ?? undefined,
      isAvailable:    this.filterAvailable ?? undefined,
      skipCount:      (this.currentPage - 1) * this.pageSize,
      maxResultCount: this.pageSize,
      sorting:        'fullName',
    };

    this.courierService.getList(filter).subscribe({
      next: res => {
        this.couriers   = res.items ?? [];
        this.totalCount = res.totalCount ?? 0;
        this.isLoading  = false;

        // حدّث التفاصيل الجانبية إذا كان هناك مندوب محدد
        if (this.selectedCourier) {
          this.selectedCourier =
            this.couriers.find(c => c.id === this.selectedCourier?.id) ?? null;
        }
      },
      error: () => (this.isLoading = false),
    });
  }

  // ── Select (Detail Panel) ─────────────────────────────────────
  selectCourier(courier: CourierProfileDto): void {
    this.selectedCourier = this.selectedCourier?.id === courier.id ? null : courier;
  }

  closeDetail(): void {
    this.selectedCourier = null;
  }

  // ── Create Modal ──────────────────────────────────────────────
  openCreateModal(): void {
    this.isEditMode = false;
    this.editingId  = undefined;
    this.form.reset();
    this.form.get('password')?.setValidators([Validators.required, Validators.minLength(6)]);
    this.form.get('password')?.updateValueAndValidity();
    this.modalRef = this.modalService.open(this.courierModal, { size: 'md', centered: true });
  }

  // ── Edit Modal ────────────────────────────────────────────────
  openEditModal(courier: CourierProfileDto): void {
    this.isEditMode = true;
    this.editingId  = courier.id;
    this.form.get('password')?.clearValidators();
    this.form.get('password')?.updateValueAndValidity();
    this.form.patchValue({
      fullName: courier.fullName,
      phone:    courier.phone,
      email:    courier.email,
      zone:     courier.zone,
      password: '',
    });
    this.modalRef = this.modalService.open(this.courierModal, { size: 'md', centered: true });
  }

  // ── Save ──────────────────────────────────────────────────────
  save(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.isSaving = true;

    const val = this.form.value;
    const obs = this.isEditMode
      ? this.courierService.update(this.editingId!, val as UpdateCourierDto)
      : this.courierService.create(val as CreateCourierDto);

    obs.subscribe({
      next:  () => { this.isSaving = false; this.modalRef?.close(); this.loadCouriers(); },
      error: () => (this.isSaving = false),
    });
  }

  // ── Delete ────────────────────────────────────────────────────
  delete(courier: CourierProfileDto): void {
    this.confirmation.warn('ShippingMvp::AreYouSureToDelete', 'ShippingMvp::AreYouSure').subscribe(status => {
      if (status === 'confirm') {
        this.courierService.delete(courier.id!).subscribe(() => {
          if (this.selectedCourier?.id === courier.id) this.selectedCourier = null;
          this.loadCouriers();
        });
      }
    });
  }

  // ── Availability Toggle ───────────────────────────────────────
  toggleAvailability(courier: CourierProfileDto): void {
    this.courierService
      .setAvailability(courier.id!, !courier.isAvailable)
      .subscribe(() => this.loadCouriers());
  }

  // ── Filters ───────────────────────────────────────────────────
  onFilterChange(): void { this.currentPage = 1; this.loadCouriers(); }
  onPageChange(page: number): void { this.currentPage = page; this.loadCouriers(); }
  get totalPages(): number { return Math.ceil(this.totalCount / this.pageSize); }

  // ── Helpers ───────────────────────────────────────────────────
  getStatusLabel(status?: CourierStatus): string {
    return CourierStatus[status ?? 0];
  }

  getStatusClass(status?: CourierStatus): string {
    const map: Record<number, string> = {
      [CourierStatus.Active]:   'badge-active',
      [CourierStatus.Inactive]: 'badge-inactive',
      [CourierStatus.Busy]:     'badge-busy',
    };
    return map[status ?? 0] ?? '';
  }

  isInvalid(field: string): boolean {
    const ctrl = this.form.get(field);
    return !!(ctrl?.invalid && ctrl?.touched);
  }
}