import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RepositorioService } from '../../../services/repositorio.service';

@Component({
  selector: 'app-favoritos',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './favoritos.component.html',
  styleUrl: './favoritos.component.css'
})
export class FavoritosComponent implements OnInit {
  protected repoService = inject(RepositorioService);
  isCollapsed = signal(false);

  ngOnInit() {
    this.repoService.carregarFavoritos();
  }

  toggleSidebar() {
    this.isCollapsed.update(v => !v);
  }

  remover(id: string) {
    this.repoService.removerFavorito(id);
  }
}