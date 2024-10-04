import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
  HttpParams,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map } from 'rxjs/operators';

@Injectable()
export class WarehouseService {
  //TODO: export baseURL to a config file
  private baseURL = 'http://localhost:5205/api/warehouses/';

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

  getWarehouses() {
    let currentAPIMethod = 'getAllWarehouses';
    let url = this.baseURL + currentAPIMethod;
    return this.http.get(url).pipe(
      map((data) => data),
      catchError(this.handleError)
    );
  }

  updateCapacity(warehouseId: string, newCapacity: number) {
    let currentAPIMethod = 'updateWarehouseCapacity';
    let url = this.baseURL + currentAPIMethod;

    const optionsWithParam = this.requestOptions;

    let queryParams = new HttpParams();
    queryParams = queryParams.append('warehouseId', warehouseId);
    queryParams = queryParams.append('newCapacity', newCapacity);

    optionsWithParam.params = queryParams;

    return this.http.put(url, {}, { params: queryParams }).pipe(
      map((data) => data),
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse | any) {
    console.error(error.error || error.body.error);
    return error.error || 'Server error occured';
  }
}
