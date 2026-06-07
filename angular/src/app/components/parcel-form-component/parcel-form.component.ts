import { Component, Input, Output, EventEmitter, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CreateParcelDto, UpdateParcelDto, ParcelDto } from '../../proxy/shipping-management/parcels/dtos';

@Component({
  selector: 'app-parcel-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './parcel-form.component.html',
  styleUrls: ['./parcel-form.component.scss'],
})
export class ParcelFormComponent implements OnInit, OnChanges {
  @Input() mode: 'create' | 'edit' = 'create';
  @Input() parcel?: ParcelDto | null;
  @Output() save = new EventEmitter<CreateParcelDto | UpdateParcelDto>();
  @Output() cancel = new EventEmitter<void>();

  form!: FormGroup;

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.buildForm();
    if (this.mode === 'edit' && this.parcel) this.patchForm();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.parcel && !changes.parcel.firstChange) {
      this.patchForm();
    }
    if (changes.mode && this.form) {
      // if switched to create, reset form
      if (this.mode === 'create') this.form.reset();
    }
  }

  buildForm(): void {
    this.form = this.fb.group({
      senderName: ['' , Validators.required],
      senderPhone: ['' , Validators.required],
      receiverName: ['' , Validators.required],
      receiverPhone: ['' , Validators.required],
      pickupAddress: ['' , Validators.required],
      deliveryAddress: ['' , Validators.required],
      weight: [null],
      price: [null],
    });
  }

  patchForm(): void {
    if (!this.parcel) return;
    this.form.patchValue({
      senderName: this.parcel.senderName,
      senderPhone: this.parcel.senderPhone,
      receiverName: this.parcel.receiverName,
      receiverPhone: this.parcel.receiverPhone,
      pickupAddress: this.parcel.pickupAddress,
      deliveryAddress: this.parcel.deliveryAddress,
      weight: this.parcel.weight,
      price: this.parcel.price,
    });
  }

  submit(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.save.emit(this.form.value as CreateParcelDto | UpdateParcelDto);
  }

  onCancel(): void {
    this.cancel.emit();
  }

  isInvalid(field: string): boolean {
    const ctrl = this.form.get(field);
    return !!(ctrl?.invalid && ctrl?.touched);
  }
}
