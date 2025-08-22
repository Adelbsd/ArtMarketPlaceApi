import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-product-form',
  templateUrl: './product-form.component.html',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule], 
})
export class ProductFormComponent {
  productForm: FormGroup;
  message: string = '';

  constructor(
    private fb: FormBuilder, 
    private http: HttpClient,
    private authService: AuthService
  ) {
    this.productForm = this.fb.group({
      titre: ['', Validators.required],         
      description: ['', Validators.required],
      prix: [0, [Validators.required, Validators.min(1)]], 
      imageUrl: [''],
      categorie: [''],
      stock: [1, [Validators.required, Validators.min(1)]]  
    });
  }

  onSubmit() {
    if (this.productForm.invalid) return;

    const artisanId = this.authService.getUserId(); 
    if (!artisanId) {
    this.message = '❌ Impossible de créer le produit : artisan non identifié.';
    return;
  }
    const product = this.productForm.value; 

    this.http.post(`http://localhost:5009/api/artisans/${artisanId}/produits`, product).subscribe({
      next: () => {
        this.message = '✅ Produit créé avec succès !';
        this.productForm.reset();
      },
      error: (err) => {
        this.message = '❌ Erreur lors de la création du produit.';
        console.error('Erreur API:', err);
      }
    });
  }
}
