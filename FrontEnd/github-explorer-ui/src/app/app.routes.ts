import { Routes } from '@angular/router';
import { ReposComponent } from './pages/repositorios/repos/repos.component';

export const routes: Routes = [
  { 
    path: '', 
    component: ReposComponent 
  },
  { 
    path: '**', 
    redirectTo: '' 
  }
];