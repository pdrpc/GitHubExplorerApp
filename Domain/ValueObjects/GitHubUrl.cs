namespace Domain.ValueObjects
{
    public class GitHubUrl : ValueObject
    {
        public string Value { get; }

        public GitHubUrl(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !value.Contains("github.com"))
                throw new ArgumentException("URL do GitHub inválida.");

            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
