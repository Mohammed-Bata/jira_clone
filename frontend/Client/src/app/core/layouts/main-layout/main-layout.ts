import { Component, OnInit } from '@angular/core';
import { Navbar } from '../../../shared/components/navbar/navbar';
import { Sidebar } from '../../../shared/components/sidebar/sidebar';
import { RouterOutlet } from '@angular/router';
import { AuthService } from '../../services/authservice';


@Component({
  selector: 'app-main-layout',
  imports: [Navbar,Sidebar,RouterOutlet],
  templateUrl: './main-layout.html',
  styleUrl: './main-layout.scss',
})
export class MainLayout implements OnInit {

  Authready = false;

  constructor(private authservice:AuthService){}
  
    ngOnInit(){
      this.authservice.refresh().subscribe({
        next:() => {
          console.log("refresh page");
          this.Authready = true;
        },
        error:() => console.log("not logged in")
      })
    }
}
