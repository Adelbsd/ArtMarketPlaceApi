import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-navbar',
   standalone: true,
  templateUrl: './navbar.component.html',
   imports: [CommonModule, RouterModule],
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  userPageLink = '';
  userPageLabel = '';

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit() {
  this.authService.role$.subscribe(role => {
    console.log(" Rôle reçu dans Navbar:", role);
    if (!role) {
      this.userPageLink = '';
      this.userPageLabel = '';
      return;
    }

    switch (role) {
      case 'Admin':
        this.userPageLink = '/admin-dashboard';
        this.userPageLabel = 'Tableau Admin';
        break;
      case 'Artisan':
        this.userPageLink = '/artisan-dashboard';
        this.userPageLabel = 'Tableau Artisan';
        break;
      case 'Client':
        this.userPageLink = '/customer-dashboard';
        this.userPageLabel = 'Tableau Client';
        break;
      case 'Livreur':
        this.userPageLink = '/delivery-dashboard';
        this.userPageLabel = 'Tableau Livreur';
        break;
      default:
        this.userPageLink = '';
        this.userPageLabel = '';
    }
  });
}


  isLoggedIn(): boolean {
    return this.authService.isLoggedIn(); 
  }
  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
