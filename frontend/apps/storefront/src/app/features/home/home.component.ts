import { Component } from '@angular/core';

@Component({
  standalone: true,
  selector: 'app-home',
  template: `
    <section class="hero">
      <h1>Buy Smart. Sell Fast.</h1>
      <p>A fresh marketplace experience with verified listings and modern discovery.</p>
      <a routerLink="/listings">Browse Listings</a>
    </section>
  `,
  styles: [`.hero{padding:4rem 1rem;max-width:920px;margin:auto}`]
})
export class HomeComponent {}
