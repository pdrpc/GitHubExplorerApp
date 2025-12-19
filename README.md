# 🚀 GitHub Explorer - Full Stack Challenge

![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Angular](https://img.shields.io/badge/Angular-DD0031?style=for-the-badge&logo=angular&logoColor=white)
![TypeScript](https://img.shields.io/badge/TypeScript-007ACC?style=for-the-badge&logo=typescript&logoColor=white)

> [!IMPORTANT]
> Este projeto foi desenvolvido como um desafio técnico avançado, priorizando **Clean Architecture** no ecossistema .NET e as funcionalidades de **vanguarda do Angular 19**, como o modo **Zoneless** e **Signals**.

---

## 🏗️ Arquitetura e Decisões Técnicas

A solução foi concebida para ser altamente escalável e performática, separando claramente as responsabilidades entre cliente e servidor.

### 🛡️ BackEnd (.NET 9)
Utiliza os princípios da **Clean Architecture** para desacoplar a lógica de negócio das infraestruturas externas (GitHub API).

* **Algoritmo de Relevância**: Implementado na camada de domínio para garantir que os resultados mais importantes apareçam primeiro. O cálculo segue a fórmula:
  $$Score = (Stars \times 1.0) + (Forks \times 2.0) + (Watchers \times 0.5)$$
* **Camadas**: Core, Application, Infrastructure e Presentation.
* **Segurança**: Configuração de políticas de **CORS** para comunicação segura com o FrontEnd.

### ⚡ FrontEnd (Angular 19)
Explora o que há de mais moderno no framework para entregar uma experiência de usuário (UX) fluida.

* **Zoneless Application**: A detecção de mudanças não depende mais do `zone.js`, reduzindo o overhead do framework e diminuindo o tamanho final do bundle.
* **Estado com Signals**: Gerenciamento de estado reativo através de **Shared Signals** no `RepositorioService`, permitindo sincronização em tempo real entre o grid de pesquisa e a sidebar de favoritos.
* **Interface**: Desenvolvida com **CSS Grid** e **Flexbox**, apresentando um tema *Dark Mode* inspirado na identidade visual do GitHub.

---

## 📂 Estrutura do Projeto

Para facilitar a navegação do avaliador, o repositório segue a estrutura monorepo abaixo:

```bash
GitHubExplorerApp/
├── BackEnd/                         # Solução .NET Core
│   ├── GitHubExplorerApp.Core/      # Domínio e Interfaces
│   ├── GitHubExplorerApp.Infra/     # Integração com GitHub API
│   └── GitHubExplorerApp.Web/       # Endpoints REST
└── FrontEnd/                        # Projeto Angular
    └── github-explorer-ui/
        ├── src/app/pages/           # Componentes Repos e Favoritos
        ├── src/app/services/        # Injeção de dependência e Signals
        └── src/app/models/          # Interfaces TypeScript
```

## 🔧 Configuração e Instalação

### 1. Requisitos
* .NET SDK 9.0+
* Node.js 20+
* Angular CLI 19+

### 2. Rodando o BackEnd
1. Acesse a pasta: ``` cd BackEnd/GitHubExplorerApp.PresentationApi ```
2. Execute: ``` dotnet run ```
3. A API estará disponível em: ```https://localhost:7206```

### 3. Rodando o FrontEnd
1. Acesse a pasta: ```cd FrontEnd/github-explorer-ui```
2. Instale as dependências: ```npm install```
3. Inicie o servidor: ```ng serve```
4. Acesse no navegador: ```http://localhost:4200```

---

## ✅ Funcionalidades
* [x] Busca de repositórios via GitHub API.
* [x] Ranqueamento por relevância (Score customizado).
* [x] Sidebar vertical colapsável para gestão de favoritos.
* [x] Sincronização em tempo real entre componentes (Signals).
* [x] Interface moderna com feedback visual de carregamento.
