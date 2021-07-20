import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { OrderDetailedComponent } from './order-detailed/order-detailed.component';
import { OrdersComponent } from './orders.component';

const routes: Routes = [ 
  {path:'', component:OrdersComponent},
  {path:':id', component:OrderDetailedComponent, data:{breadcrumb: {alias:'OrdersDetailed'}}}
];


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ]
})
export class OrdersRoutingModule { }
