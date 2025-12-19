namespace Domain.Interfaces
{
    public interface IRelevanceService
    {
        double CalculateRelevance(int stars, int forks, int watchers);
    }
}
