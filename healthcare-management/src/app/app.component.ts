import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, NavigationEnd, Router, RouterOutlet} from '@angular/router';
import {HeaderComponent} from './components/header/header.component';
import {filter} from 'rxjs';
import {AuthenticationService} from './services/authentication/authentication.service';
import {NgIf} from '@angular/common';

@Component({
    selector: 'app-root',
    imports: [
      RouterOutlet,
      HeaderComponent,
      NgIf,
    ],
    templateUrl: './app.component.html',
    styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit{
  title = 'healthcare-management';

  protected isNavbarHidden : boolean = false;

  constructor(private router: Router, private activatedRoute: ActivatedRoute,
              private authenticationService: AuthenticationService) {
  }

  ngOnInit(): void {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      const childRoute = this.activatedRoute.firstChild;
      this.isNavbarHidden = childRoute?.snapshot.data['isNavbarHidden'];
    });
  }
}
