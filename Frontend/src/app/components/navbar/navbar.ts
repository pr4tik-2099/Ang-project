import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { Router,RouterLink } from "@angular/router";
import {Auth} from '../../service/auth';
import { MatMenuItem, MatMenuModule } from '@angular/material/menu';
import { NgIf } from "../../../../node_modules/@angular/common/types/_common_module-chunk";
import { CommonModule } from  '@angular/common';

@Component({
  selector: 'app-navbar',
  imports: [MatToolbarModule, MatMenuItem, MatMenuModule, MatButtonModule, MatIconModule, RouterLink, MatMenuModule,CommonModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar {
  authService = inject(Auth);
  router =inject(Router);
menu: any;

  isLoggedIn(){
    return this.authService.isLoggedIn();
  }
}
