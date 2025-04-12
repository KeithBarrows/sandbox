import { Component, OnInit } from '@angular/core';
import * as aboutData from './about.json';
import { CommonModule } from '@angular/common';
import { MatExpansionModule } from '@angular/material/expansion';
import { MenuComponent } from '../menu/menu.component';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.scss'],
  imports: [CommonModule, MatExpansionModule, MenuComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
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
