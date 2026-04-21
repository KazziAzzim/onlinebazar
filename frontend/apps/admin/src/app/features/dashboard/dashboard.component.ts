import { Component } from '@angular/core';

@Component({
  standalone: true,
  template: `
  <h2>Dashboard</h2>
  <div class="cards">
    <section>Total Listings: 1280</section>
    <section>Pending Approval: 42</section>
    <section>New Enquiries: 76</section>
  </div>`
})
export class DashboardComponent {}
