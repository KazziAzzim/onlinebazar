import { Component, inject } from '@angular/core';
import { AsyncPipe, CurrencyPipe, NgFor } from '@angular/common';
import { ListingsApiService } from '../../core/listings-api.service';

@Component({
  standalone: true,
  imports: [NgFor, AsyncPipe, CurrencyPipe],
  template: `
  <h2>Latest Listings</h2>
  <article *ngFor="let item of (vm$ | async)?.items">
    <h3>{{item.title}}</h3>
    <p>{{item.description}}</p>
    <strong>{{item.price | currency:'USD'}}</strong>
  </article>
  `
})
export class ListingsComponent {
  private api = inject(ListingsApiService);
  vm$ = this.api.getListings();
}
