import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
  HttpParams,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map } from 'rxjs/operators';
import { InventoryRecord } from '../components/warehouses/inventory';

@Injectable()
export class InventoryService {
  //TODO: export baseURL to a config file
  private baseURL = 'http://localhost:5205/api/inventory/';

  constructor(private http: HttpClient) {}

  headerDict = {
    'Content-Type': 'application/json',
    Accept: 'application/json',
  };

  params = new HttpParams();

  requestOptions = {
    headers: new HttpHeaders(this.headerDict),
    params: this.params,
  };

  getWarehouseInventory(currentWarehouseId: string) {
    let currentAPIMethod = 'getInventory';
    let url = this.baseURL + currentAPIMethod;

    const optionsWithParam = this.requestOptions;

    let queryParams = new HttpParams();
    queryParams = queryParams.append('warehouseId', currentWarehouseId);
    optionsWithParam.params = queryParams;

    return this.http
      .get(url, { params: queryParams })
      .pipe(map((data) => data));
  }

  getWarehousesWithProduct(
    currentProductId: string,
    currentWarehouseId: string
  ) {
    let currentAPIMethod = 'getWarehousesWithProduct';
    let url = this.baseURL + currentAPIMethod;

    const optionsWithParam = this.requestOptions;

    let queryParams = new HttpParams();
    queryParams = queryParams.append('productId', currentProductId);
    queryParams = queryParams.append('warehouseId', currentWarehouseId);
    optionsWithParam.params = queryParams;

    return this.http.get(url, { params: queryParams }).pipe(
      map((data) => data),
      catchError(this.handleError)
    );
  }

  createInventoryRecord(record: InventoryRecord) {
    let currentAPIMethod = 'addProductToInventory';
    let url = this.baseURL + currentAPIMethod;

    const optionsWithParam = this.requestOptions;

    let queryParams = new HttpParams();
    queryParams = queryParams.append('warehouseId', record.warehouseId);
    queryParams = queryParams.append('productId', record.productId);
    queryParams = queryParams.append('amount', record.amount);
    optionsWithParam.params = queryParams;

    return this.http.post(url, {}, { params: queryParams }).pipe(
      map((data) => data),
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse | any) {
    console.error(error.error || error.body.error);
    return error || 'Server error occured';
  }
}
