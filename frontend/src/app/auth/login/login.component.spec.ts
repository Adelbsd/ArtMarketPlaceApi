import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
})
export class LoginComponent {
  loginData = {
    email: '',
    mdp: ''
  };

  errorMessage = '';

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit() {
  this.authService.login(this.loginData).subscribe({
    next: (res) => {
      console.log('Connexion réussie');
      localStorage.setItem('token', res.token);
      localStorage.setItem('role', res.role);
      this.redirectToDashboard(res.role);
    },
    error: (err) => {
      this.errorMessage = 'Email ou mot de passe incorrect';
      console.error('Erreur de connexion', err);
    }
  });
}


  
  redirectToDashboard(role: number) {
  switch (role) {
    case 0: 
      this.router.navigate(['/admin-dashboard']);
      break;
    case 1: 
      this.router.navigate(['/artisan-dashboard']);
      break;
    case 2: 
      this.router.navigate(['/customer-dashboard']);
      break;
    case 3: 
      this.router.navigate(['/delivery-dashboard']);
      break;
    default:
      this.router.navigate(['/login']);
  }
}

  }

