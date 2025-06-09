import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-product-form',
  templateUrl: './product-form.component.html',
  
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule], 
})
export class ProductFormComponent {
  productForm: FormGroup;
  message: string = '';

  constructor(private fb: FormBuilder, private http: HttpClient) {
    this.productForm = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      price: [0, [Validators.required, Validators.min(1)]],
      imageUrl: [''],
      category: ['']
    });
  }

  onSubmit() {
    if (this.productForm.invalid) return;

    const product = this.productForm.value;

    this.http.post('/api/products', product).subscribe({
      next: () => {
        this.message = 'Produit créé avec succès !';
        this.productForm.reset();
      },
      error: (err) => {
        this.message = 'Erreur lors de la création du produit.';
        console.error(err);
      }
    });
  }
}
