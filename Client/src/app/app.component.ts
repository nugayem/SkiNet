import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AccountService } from './account/account.service';
import { BasketService } from './basket/basket.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit { 

  constructor(private basketService: BasketService, private accountService: AccountService ) {
  }
  ngOnInit(): void {
    this.loadBasket();   
    this.loadCurrentUser();
  }
  loadCurrentUser(){
    const token= localStorage.getItem('token');
    if(token){
      this.accountService.loadCurrentUser(token).subscribe(()=>{
        console.log('loaded User')
      }, error=>{
        console.log(error);
      })
    }
  }
  loadBasket(){
    const basketId= localStorage.getItem('basket_id');
    console.log(" Basket ID is "+ basketId);
    //localStorage.removeItem('basket_id');

      //    this.basketService.deleteLocalBasket(basketId);
    if(basketId){
      this.basketService.getBasket(basketId)
          .subscribe(()=>{
            console.log("Initialize Basket"+ basketId);
          }, error=>{
            console.log(error);
          });
    }
  }
}
