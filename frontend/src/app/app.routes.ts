import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { AdminComponent } from './dashboards/admin/admin.component';
import { ArtisanComponent } from './dashboards/artisan/artisan.component';
import { CustomerComponent } from './dashboards/customer/customer.component';
import { DeliveryComponent } from './dashboards/delivery/delivery.component';
import { RoleGuard } from './guards/role.guard';
import { UserProductsComponent } from './admin/user-products/user-products.component';
import { ProductListComponent } from './dashboards/artisan/product-list.component';
import { AuthGuard } from './guards/auth.guard';
import { PublicProductsComponent } from './public/public-products/public-products.component';
import { HomeComponent } from './home/home.component';

export const routes: Routes = [
  { path: 'home', component: HomeComponent },
 
  { path: 'login', component: LoginComponent },

 
  { path: 'produits', component: PublicProductsComponent, canActivate: [AuthGuard] },
  
  
  { path: '', redirectTo: '/home', pathMatch: 'full' },

  {
    path: 'admin-dashboard',
    component: AdminComponent,
    canActivate: [RoleGuard],
    data: { expectedRole: 'Admin' }
  },
  { path: 'admin/user-products/:id', component: UserProductsComponent },

  {
    path: 'artisan-dashboard',
    component: ArtisanComponent,
    canActivate: [RoleGuard],
    data: { expectedRole: 'Artisan' }
  },
  {
    path: 'customer-dashboard',
    component: CustomerComponent,
    canActivate: [RoleGuard],
    data: { expectedRole: 'Customer' }
  },
  {
    path: 'delivery-dashboard',
    component: DeliveryComponent,
    canActivate: [RoleGuard],
    data: { expectedRole: 'DeliveryPartner' }
  }
];
