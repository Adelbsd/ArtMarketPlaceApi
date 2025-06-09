import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ProductFormComponent } from './product-form.component';
import { ProductListComponent } from './product-list.component';
@Component({
  selector: 'app-artisan',
  standalone: true,
  imports: [CommonModule, ProductFormComponent, ProductListComponent],
  templateUrl: './artisan.component.html',
  styleUrls: ['./artisan.component.scss']
})
export class ArtisanComponent {}