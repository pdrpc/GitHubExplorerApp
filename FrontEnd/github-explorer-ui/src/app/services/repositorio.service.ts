import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GitHubRepository } from '../models/github-repository.model';

@Injectable({
  providedIn: 'root'
})
export class RepositorioService {
  private http = inject(HttpClient);
  private readonly apiUrl = 'https://localhost:7206/api/GitHub'; 

  // O Signal que mantém a lista de favoritos sincronizada no App inteiro
  public favoritos = signal<GitHubRepository[]>([]);


  buscar(query: string): Observable<GitHubRepository[]> {
    return this.http.get<GitHubRepository[]>(`${this.apiUrl}/repos?query=${query}`);
  }

  // Carrega os favoritos do servidor e atualiza o Signal
  carregarFavoritos(): void {
    this.http.get<GitHubRepository[]>(`${this.apiUrl}/favoritos`).subscribe({
      next: (dados) => this.favoritos.set(dados),
      error: (err) => console.error('Erro ao carregar favoritos', err)
    });
  }

  adicionarFavorito(repo: GitHubRepository): void {
    this.http.post<void>(`${this.apiUrl}/favoritos`, repo).subscribe({
      next: () => this.carregarFavoritos(), // Recarrega para garantir sincronia com o DB
      error: (err) => console.error('Erro ao adicionar', err)
    });
  }

  removerFavorito(id: string): void {
    this.http.delete<void>(`${this.apiUrl}/favoritos/${id}`).subscribe({
      next: () => this.carregarFavoritos(), // Recarrega e o Signal avisa os componentes
      error: (err) => console.error('Erro ao remover', err)
    });
  }
}