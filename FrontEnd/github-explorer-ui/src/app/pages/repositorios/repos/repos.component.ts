import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RepositorioService } from '../../../services/repositorio.service';
import { GitHubRepository } from '../../../models/github-repository.model';

@Component({
  selector: 'app-repos',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './repos.component.html',
  styleUrl: './repos.component.css'
})
export class ReposComponent {
  protected repoService = inject(RepositorioService);

  repositorios = signal<GitHubRepository[]>([]);
  termoBusca = signal('');
  carregando = signal(false);

  pesquisar() {
    const query = this.termoBusca().trim();
    if (!query) return;

    this.carregando.set(true);
    this.repoService.buscar(query).subscribe({
      next: (res) => {
        this.repositorios.set(res);
        this.carregando.set(false);
      },
      error: () => this.carregando.set(false)
    });
  }
  
  ehFavorito(id: string): boolean {
    return this.repoService.favoritos().some(f => f.id === id);
  }

  toggleFavorito(repo: GitHubRepository) {
    if (this.ehFavorito(repo.id)) {
      this.repoService.removerFavorito(repo.id);
    } else {
      this.repoService.adicionarFavorito(repo);
    }
  }
}