import { Component } from '@angular/core';

@Component({
  standalone: true,
  template: `
    <h2>Admin Login</h2>
    <form>
      <input type="email" placeholder="Email" />
      <input type="password" placeholder="Password" />
      <button type="submit">Sign in</button>
    </form>
  `
})
export class LoginComponent {}
