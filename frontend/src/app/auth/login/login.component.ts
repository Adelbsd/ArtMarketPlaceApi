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

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit() {
    this.authService.login(this.loginData).subscribe({
      next: (res) => {
        console.log('Connexion réussie', res);
        localStorage.setItem('token', res.token);

        const mappedRole = this.authService.getUserRole(); 
        console.log('Redirection en cours vers le rôle :', mappedRole);
        this.redirectToDashboard(mappedRole);
      },
      error: (err) => {
        console.error('Erreur de connexion', err);
        alert('Email ou mot de passe incorrect');
      }
    });
  }

  redirectToDashboard(role: string | null) {
    switch (role) {
      case 'Admin':
        this.router.navigate(['/admin-dashboard']);
        break;
      case 'Artisan':
        this.router.navigate(['/artisan-dashboard']);
        break;
      case 'Customer':
        this.router.navigate(['/customer-dashboard']);
        break;
      case 'DeliveryPartner':
        this.router.navigate(['/delivery-dashboard']);
        break;
      default:
        console.warn('Rôle inconnu, redirection vers /login');
        this.router.navigate(['/login']);
    }
  }
}
