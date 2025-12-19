export interface GitHubRepository {
  id: string;
  name: string;
  fullName: string;
  description: string;
  url: string;
  stars: number;
  forks: number;
  watchers: number;
  relevanceScore: number;
  isFavorite: boolean;
}