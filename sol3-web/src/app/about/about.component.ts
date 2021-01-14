import { Sol3MaterialModule } from './../material-model';
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

  panelOpenState = false;
  isMenuHidden:boolean = true;

  public toggleMenu(){
    this.isMenuHidden = !this.isMenuHidden;
  }
}
