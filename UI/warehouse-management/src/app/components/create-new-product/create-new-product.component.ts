import { Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { Product, NewProduct } from '../all-products/product';
import { ProductService } from '../../services/products.service';
import { ClrForm } from '@clr/angular';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-create-new-product',
  templateUrl: './create-new-product.component.html',
  styleUrl: './create-new-product.component.scss',
})
export class CreateNewProductComponent {
  product!: Product;
  opened = false;
  productForm: any;

  subscription: Subscription = new Subscription();

  @Output() updated: EventEmitter<any> = new EventEmitter<any>();

  // TODO: Add custom validation on the unitSize to allow only positive integers
  newProductForm = new FormGroup({
    nameInput: new FormControl('', [
      Validators.required,
      Validators.minLength(3),
    ]),
    unitSizeInput: new FormControl('', Validators.required),
    hazardOptions: new FormControl('', Validators.required),
  });

  @ViewChild(ClrForm, { static: true }) clrForm: ClrForm | undefined;

  constructor(private _productService: ProductService) {}

  submit(): void {
    if (this.newProductForm.invalid) {
      this.clrForm?.markAsTouched();
    } else {
      // No errors so let's create a product
      this.createProduct();
    }
  }

  createProduct(): void {
    // fix unitSize
    let unitSize = 1;
    if (this.newProductForm.controls['unitSizeInput'].value) {
      unitSize = parseFloat(
        this.newProductForm.controls['unitSizeInput'].value
      );
    }

    let hazardousFlag =
      this.newProductForm.controls['hazardOptions'].value === 'true' ? 1 : 0;

    let formProduct: NewProduct = {
      // nameInput is required so no way it's empty
      name: this.newProductForm.controls['nameInput'].value || '',
      unitSize: unitSize,
      hazardous: hazardousFlag as any,
    };

    this.subscription = this._productService
      .createProduct(formProduct)
      .subscribe((data) => {
        this.updateAllProducts(data?.id);
        this.cancel();
      });
  }

  updateAllProducts(id: string): void {
    this.updated.emit({ id: id });
  }

  openModal(): void {
    this.opened = true;
  }

  cancel(): void {
    this.opened = false;
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
