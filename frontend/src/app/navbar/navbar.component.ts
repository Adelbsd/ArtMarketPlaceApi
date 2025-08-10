import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent {
  userPageLink = '/profile';
  userPageLabel = 'Mon profil';

  constructor(private router: Router) {}

  isLoggedIn(): boolean {
    return typeof window !== 'undefined' && !!localStorage.getItem('token');
  }

  logout() {
    if (typeof window !== 'undefined') {
      localStorage.removeItem('token');
      this.router.navigate(['/login']);
    }
  }
}
