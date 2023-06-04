namespace GroupProjectDepositCatalogWebApp.Models
{
    public class SearchResultEntity
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string? DepositName { get; set; }
        public CurrencyType TypeOfCurrency { get; set; }
        public float EffectiveAnnualInterestRate { get; set; }
        public float MinSum { get; set; }
        public InterestEnumType TypeOfInterest { get; set; }

    }
}
