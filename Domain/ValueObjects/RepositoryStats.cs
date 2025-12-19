namespace Domain.ValueObjects
{
    public class RepositoryStats : ValueObject
    {
        public int Stars { get; }
        public int Forks { get; }
        public int Watchers { get; }

        public RepositoryStats(int stars, int forks, int watchers)
        {
            // Regra de negócio: estatísticas não podem ser negativas
            Stars = stars < 0 ? 0 : stars;
            Forks = forks < 0 ? 0 : forks;
            Watchers = watchers < 0 ? 0 : watchers;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Stars;
            yield return Forks;
            yield return Watchers;
        }
    }
}
