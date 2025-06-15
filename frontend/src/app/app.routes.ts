// src/app/app.routes.ts
import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { AdminComponent } from './dashboards/admin/admin.component';
import { ArtisanComponent } from './dashboards/artisan/artisan.component';
import { CustomerComponent } from './dashboards/customer/customer.component';
import { DeliveryComponent } from './dashboards/delivery/delivery.component';
import { RoleGuard } from './guards/role.guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },

  {
    path: 'admin-dashboard',
    component: AdminComponent,
    canActivate: [RoleGuard],
    data: { expectedRole: 'Admin' }
  },
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
  },

  { path: '', redirectTo: 'login', pathMatch: 'full' }
];


