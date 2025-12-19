import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { FavoritosComponent } from './pages/repositorios/favoritos/favoritos.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, FavoritosComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App { }