import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ListingsApiService {
  private http = inject(HttpClient);
  private base = '/api/listings';

  getListings() {
    return this.http.get<{items: Array<{title:string;description:string;price:number}>}>(this.base);
  }
}
