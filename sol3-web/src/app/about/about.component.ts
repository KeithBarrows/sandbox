import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.scss']
})
export class AboutComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  isMenuHidden:boolean = true;

  public toggleMenu(){
    this.isMenuHidden = !this.isMenuHidden;
  }
}
