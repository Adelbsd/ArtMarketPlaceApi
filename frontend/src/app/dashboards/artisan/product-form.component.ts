import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './product-form.component.html'
})
export class ProductFormComponent {
  productForm: FormGroup;
  message = '';

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private authService: AuthService
  ) {
    this.productForm = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      price: ['', Validators.required],
      imageUrl: ['', Validators.required],
      category: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.productForm.invalid) return;

    const artisanId = this.authService.getUserId();
    if (!artisanId) {
      this.message = 'Impossible de récupérer votre ID utilisateur';
      return;
    }

    this.http.post(`http://localhost:5009/api/artisans/${artisanId}/products`, this.productForm.value)
      .subscribe({
        next: () => {
          this.message = 'Produit créé avec succès !';
          this.productForm.reset();
        },
        error: (err) => {
          console.error('Erreur lors de la création du produit', err);
          this.message = 'Erreur lors de la création du produit';
        }
      });
  }
}
