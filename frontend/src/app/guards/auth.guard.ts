import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(): boolean | UrlTree {
    if (!this.authService.isLoggedIn()) {
      return this.router.parseUrl('/login');
    }

    const role = this.authService.getUserRole();

    switch (role) {
      case 'Admin':
        return this.router.parseUrl('/admin-dashboard');
      case 'Artisan':
        return this.router.parseUrl('/artisan-dashboard');
      case 'Customer':
        return this.router.parseUrl('/customer-dashboard');
      case 'DeliveryPartner':
        return this.router.parseUrl('/delivery-dashboard');
      default:
        return this.router.parseUrl('/login');
    }
  }
}
