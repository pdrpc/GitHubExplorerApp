using Domain.Interfaces;

namespace Domain.Services
{
    public class RelevanceService : IRelevanceService
    {
        public double CalculateRelevance(int stars, int forks, int watchers)
        {
            /* Lógica de Relevância: 
           - Estrelas (stargazers_count): Peso 3. É o principal indicador de qualidade e popularidade
           - Forks (forks_count): Peso 2. Indica que a comunidade está utilizando o código para novos projetos
           - Watchers (watchers_count): Peso 1. Indica interesse contínuo, mas é um engajamento menos ativo
            */
            return (stars * 3) + (forks * 2) + watchers;
        }
    }
}
