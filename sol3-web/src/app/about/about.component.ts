import { Sol3MaterialModule } from './../material-model';
import { Component, OnInit } from '@angular/core';
import * as aboutData from './about.json';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.scss']
})
export class AboutComponent implements OnInit {

  posts: any = (aboutData as any).default;

  constructor() { }

  ngOnInit(): void {
  }

  panelOpenState = false;
  isMenuHidden:boolean = true;

  public toggleMenu(){
    this.isMenuHidden = !this.isMenuHidden;
  }
}
