import { Component, OnInit } from '@angular/core';
import { MenuComponent } from '../menu/menu.component';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.scss'],
  imports: [MenuComponent, CommonModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class ContactComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  isMenuHidden:boolean = true;

  public toggleMenu(){
    this.isMenuHidden = !this.isMenuHidden;
  }
}
