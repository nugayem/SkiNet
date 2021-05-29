import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  slides = [
    {image: 'assets/images/hero1.jpg', text: 'First'},
    {image: 'assets/images/hero2.jpg', text: 'Second'},
    {image: 'assets/images/hero3.jpg', text: 'Third'}
 ];
 noWrapSlides = false;
 showIndicator = true;
  constructor() { }

  ngOnInit(): void {
  }

}
