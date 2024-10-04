import {
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { Product } from './product';
import { ProductService } from '../../services/products.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-all-products',
  templateUrl: './all-products.component.html',
  styleUrl: './all-products.component.scss',
})
export class AllProductsComponent implements OnInit, OnDestroy {
  products: Product[] = [];
  loading = true;

  subscription: Subscription = new Subscription;

  constructor(private _productService: ProductService) {}

  ngOnInit() {
    this.loading = true;
    this.getProducts();
  }

  getProducts() {
    this.subscription = this._productService
      .getProducts()
      .subscribe((result) => {
        if (result != undefined) {
          this.handleProducts(result as Product[]);
        } 
        else 
        {
          this.loading = false;
        }
      });
  }

  handleProducts(data: Product[]) {
    let productsArray: Product[] = [];

    if (data.length > 0) {
      data.forEach((element) => {
        let p: Product = {
          name: element.name,
          productId: element.productId,
          unitSize: element.unitSize,
          hazardous: element.hazardous,
        };
        productsArray.push(p);
      });
      this.products = productsArray;
      this.loading = false;
    } 
    else {
      this.loading = false;
    }
  }

  onUpdate() {
    this.getProducts();
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
