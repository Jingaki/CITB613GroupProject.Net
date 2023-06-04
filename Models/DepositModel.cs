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

        public float EffectiveAnnualInterestRate { get; set; }

        public string? WebLinkToOffer { get; set; }

        public string? DescriptionOfNegotiatedInterestRate { get; set; }

        public float MinSum { get; set; }
        public string? MinSumDescription { get; set; }
        public int MinDuration { get; set; }

        public float MaxSum { get; set; }
        public string? MaxSumDescription { get; set; }
        public int MaxDuration { get; set; }

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
    StandartTermDeposit = 1,
    MountlyDeposit = 2
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
    Annually
}