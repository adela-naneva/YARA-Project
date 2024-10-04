import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map } from 'rxjs/operators';
import { NewProduct } from '../components/all-products/product';

@Injectable()
export class ProductService {
  //TODO: export baseURL to a config file
  private baseURL = 'http://localhost:5205/api/products/';

  constructor(private http: HttpClient) {}

  getProducts() {
    let currentAPIMethod = 'getAllProducts';
    let url = this.baseURL + currentAPIMethod;
    return this.http.get(url).pipe(
      map((data) => data),
      catchError(this.handleError)
    );
  }

  createProduct(product: NewProduct) {
    let currentAPIMethod = 'createProduct';
    let url = this.baseURL + currentAPIMethod;

    const headerDict = {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
    }
    
    const requestOptions = {                                                                                                                                                                                 
      headers: new HttpHeaders(headerDict), 
    };

    const data = JSON.stringify(product);
    return this.http.post<any>(url, data, requestOptions);
  }

  private handleError(error: HttpErrorResponse | any) {
    console.error(error.error || error.body.error);
    return error.error || 'Server error occured';
  }
}
