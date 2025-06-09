import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common'; 

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,CommonModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'art-marketplace-frontend';

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
