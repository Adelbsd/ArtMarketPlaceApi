import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})

export class RoleGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}
canActivate(route: ActivatedRouteSnapshot): boolean {
  const expectedRole = route.data['expectedRole'];  
  const userRole = this.authService.getUserRole();  

  console.log('RoleGuard - expectedRole:', expectedRole);
  console.log('RoleGuard - userRole:', userRole);

  if (userRole !== expectedRole) {
    this.router.navigate(['/login']);
    return false;
  }

  return true;
}




}
