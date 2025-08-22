import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  styleUrl: './login.component.scss',
  templateUrl: './login.component.html',
})
export class LoginComponent {
  isRegisterMode = false;   

  loginData = {
    email: '',
    mdp: ''
  };

  registerData = {
    nomComplet: '',
    email: '',
    mdp: '',
    role: 1
  };

  roles = [
    { value: 0, label: 'Artisan' },
    { value: 1, label: 'Client' },
    { value: 2, label: 'Livreur' }
  ];

  constructor(private authService: AuthService, private router: Router) {}

  toggleMode() {
    this.isRegisterMode = !this.isRegisterMode;
  }

  onSubmit() {
    this.authService.login(this.loginData).subscribe({
      next: (res) => {
        this.authService.saveToken(res.token);
        const mappedRole = this.authService.getUserRole();
        this.redirectToDashboard(mappedRole);
      },
      error: (err) => {
        alert('Email ou mot de passe incorrect');
      }
    });
  }

  onRegister() {
    this.authService.register(this.registerData).subscribe({
      next: () => {
        alert('Compte créé avec succès');
        //this.isRegisterMode = false;
      },
      error: (err) => {
        alert('Erreur lors de l’inscription');
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
      case 'Client':
        this.router.navigate(['/customer-dashboard']);
        break;
      case 'Livreur':
        this.router.navigate(['/delivery-dashboard']);
        break;
      default:
        this.router.navigate(['/login']);
    }
  }
}
