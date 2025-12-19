import { ApplicationConfig, provideZonelessChangeDetection } from '@angular/core';
import { provideHttpClient } from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZonelessChangeDetection(), // Já configurado como Zoneless
    provideHttpClient(), // Adicione esta linha para habilitar o HttpClient
    provideRouter(routes)
  ]
};