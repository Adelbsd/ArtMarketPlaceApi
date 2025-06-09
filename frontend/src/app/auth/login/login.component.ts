import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

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

  onSubmit() {
    console.log('Login submitted', this.loginData);
    
  }
}
