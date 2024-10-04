import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
  HttpParams,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map } from 'rxjs/operators';
import { NewTransaction } from '../components/transaction-history/transaction';

@Injectable()
export class TransactionService {
  //TODO: export baseURL to a config file
  private baseURL = 'http://localhost:5205/api/transactions/';

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

  getTransactionHistory(currentWarehouseId: string) {
    let currentAPIMethod = 'getTransactionHistory';
    let url = this.baseURL + currentAPIMethod;

    const optionsWithParam = this.requestOptions;

    let queryParams = new HttpParams();
    queryParams = queryParams.append('currentWHId', currentWarehouseId);
    optionsWithParam.params = queryParams;

    return this.http.get(url, { params: queryParams }).pipe(
      map((data) => data),
      catchError(this.handleError)
    );
  }

  createTransaction(transaction: NewTransaction) {
    let currentAPIMethod = 'createTransaction';
    let url = this.baseURL + currentAPIMethod;

    const data = JSON.stringify(transaction);

    return this.http.post<any>(url, data, this.requestOptions);
  }

  private handleError(error: HttpErrorResponse | any) {
    console.error(error.error || error.body.error);
    return error.error || 'Server error occured';
  }
}
