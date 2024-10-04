import {
  Component,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { Subscription } from 'rxjs';
import { Transaction } from './transaction';
import { TransactionService } from '../../services/transactions.service';

@Component({
  selector: 'app-transaction-history',
  templateUrl: './transaction-history.component.html',
  styleUrl: './transaction-history.component.scss',
})
export class TransactionHistoryComponent
  implements OnInit, OnChanges, OnDestroy
{
  transactions: Transaction[] = [];
  loading = true;

  subscription: Subscription = new Subscription();

  @Input() currentWarehouseId: string = '';

  constructor(private _transactionService: TransactionService) {}

  ngOnInit() {
    this.loading = true;
    this.getTransactions();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (
      changes['currentWarehouseId'] &&
      !changes['currentWarehouseId'].isFirstChange() &&
      changes['currentWarehouseId'].previousValue !=
        changes['currentWarehouseId'].currentValue
    ) {
      this.getTransactions();
    }
  }

  getTransactions() {
    if (this.currentWarehouseId && this.currentWarehouseId !== '') {
      this.subscription = this._transactionService
        .getTransactionHistory(this.currentWarehouseId)
        .subscribe({
          next: (result) => this.handleTransactions(result as Transaction[]),
          error: () => {
            this.loading = false;
            this.transactions = [];
          },
        });
    }
  }

  handleTransactions(data: Transaction[]) {
    let trArray: Transaction[] = [];

    if (data.length > 0) {
      data.forEach((element) => {
        let t: Transaction = {
          transactionId: element.transactionId,
          type: this.defineTransactionType(
            this.currentWarehouseId,
            element.sourceWarehouseId,
            element.targetWarehouseId
          ),
          creationDate: element.creationDate,
          productId: element.productId,
          productName: element.productName,
          amount: element.amount,
          sourceWarehouseId: element.sourceWarehouseId,
          sourceWarehouseName: element.sourceWarehouseName,
          targetWarehouseId: element.targetWarehouseId,
          targetWarehouseName: element.targetWarehouseName,
        };
        trArray.push(t);
      });
      this.transactions = trArray;
      this.loading = false;
    } else {
      this.loading = false;
    }
  }

  defineTransactionType(
    currentWH: string,
    sourceId?: string,
    targetId?: string
  ): 'Import' | 'Export' {
    if (!targetId) {
      //no destination so puff, not existing anymore, export
      return 'Export';
    } else if (!sourceId) {
      // if sourceId is empty it is initial import
      return 'Import';
    } else if (sourceId == currentWH && targetId != currentWH) {
      // sourceId not empty and equals current WH id so it's an export
      return 'Export';
    } else {
      return 'Import';
    }
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
