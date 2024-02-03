using System.ComponentModel.DataAnnotations;

namespace GroupProjectDepositCatalogWebApp.Models
{
    public class SearchFormEntity
    {
        public DepositType TypeOfDeposit { get; set; }
        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "This field's value must be greater than 0.")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public float InvestmentSum { get; set; }
        public CurrencyType? CurrencyType { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "This field's value must be greater than 0 and less then 100")]
        public int DurrationPlanned { get; set; }
        public InterestPaymentType? InterestPaymentType { get; set; }
        public OwnershipType? OwnershipType { get; set; }
        public InterestEnumType? TypeOfInterest { get; set; }
        
        public TimeDepositOption TimeDeposit { get; set; }
        
        public OverdraftPossabilityOption OverdraftPossability { get; set; }
        
        public CreditPossabilityOption CreditPossability { get; set; }

        public IEnumerable<SearchResultEntity>? Results { get; set; }
    }
}
public enum TimeDepositOption
{
    All = 1,
    True = 2,
    False = 3
}
public enum OverdraftPossabilityOption
{
    All = 1,
    True = 2,
    False = 3
}
public enum CreditPossabilityOption
{
    All = 1,
    True = 2,
    False = 3
}