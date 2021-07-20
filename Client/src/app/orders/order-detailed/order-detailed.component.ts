import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OrdersService } from '../orders.service';
import { BreadcrumbService } from 'xng-breadcrumb';
import { IOrder } from 'src/app/shared/models/order';

@Component({
  selector: 'app-order-detailed',
  templateUrl: './order-detailed.component.html',
  styleUrls: ['./order-detailed.component.scss']
})
export class OrderDetailedComponent implements OnInit {
  order: IOrder;

  constructor(private orderService: OrdersService , 
    private activateRoute: ActivatedRoute, 
    private bcService: BreadcrumbService) {
      this.bcService.set('@orderDetailed', '');
     }

  ngOnInit(): void {
    this.getDetailedOrderedItem(+this.activateRoute.snapshot.paramMap.get('id'));
  }

  getDetailedOrderedItem(id:number){
    this.orderService.getOrderDetailed(id).subscribe((order: IOrder)=>{
      this.order=order;
      this.bcService.set('@orderDetailed', `Order# ${order.id} - ${order.status}`);
    }, err=>{
      console.log(err);
    })
  }

}
