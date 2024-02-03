using System.ComponentModel.DataAnnotations;

namespace GroupProjectDepositCatalogWebApp.Models
{
    public class DepositModel
    {
        public int Id { get; set; }
        [Required]        
        public string DepositName { get; set; } = null!;

        public bool TimeDeposit { get; set; }
        public bool OverdraftPossability { get; set; }
        public bool CreditPossability { get; set; }
        public bool MonthlyCompounding { get; set; }
        public bool TerminalCapitalization { get; set; }
        public bool ValidForClientsOnly { get; set; }
        public DepositType TypeOfDeposit { get; set; }
        public InterestEnumType TypeOfInterest { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public InterestPaymentType InterestPaymentType { get; set; }
        public OwnershipType OwnershipType { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:0.00}%")]
        public float EffectiveAnnualInterestRate { get; set; }

        public ICollection<ShiftingInterestRateDataModel>? ShiftingInteresData { get; set; }

        [Display(Name = "URL")]
        [DataType(DataType.Url)]
        [DisplayFormat(DataFormatString = "<a href='{0}'>{0}</a>", HtmlEncode = false)]
        public string? WebLinkToOffer { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 6, ErrorMessage = "The length of MyString must be between 6 and 200 characters.")]
        public string? DescriptionOfNegotiatedInterestRate { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public float MinSum { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 6, ErrorMessage = "The length of MyString must be between 6 and 200 characters.")]
        public string? MinSumDescription { get; set; }
        public int MinDuration { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public float MaxSum { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 6, ErrorMessage = "The length of MyString must be between 6 and 200 characters.")]
        public string? MaxSumDescription { get; set; }
        public int MaxDuration { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 6, ErrorMessage = "The length of MyString must be between 6 and 200 characters.")]
        public string? DurationDescription { get; set; }

        // Foreign key
        [Required]
        public string MyApplicationUserId { get; set; } = null!;

        // Navigation properties
        [Required]
        public ApplicationUserModel MyApplicationUser { get; set; } = null!;
    }
}
public enum DepositType
{
    Standart = 1,
    Mountly = 2
}
public enum InterestEnumType
{
    Fixed = 1,
    Shifting = 2
}
public enum CurrencyType
{
    BGN = 1,
    EUR = 2,
    USD = 3,
    GBP = 4,
    CHF = 5
}
public enum OwnershipType
{
    Individual = 1,
    Retiree = 2,
    Child = 3
}
public enum InterestPaymentType
{
    Monthly = 1,
    Quarterly = 2,
    Semiannually = 3,
    Annually = 4
}