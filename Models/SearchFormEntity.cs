namespace GroupProjectDepositCatalogWebApp.Models
{
    public class SearchFormEntity
    {
        public DepositType TypeOfDeposit { get; set; }
        public float InvestmentSum { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public int DurrationPlanned { get; set; }
        public InterestPaymentType InterestPaymentType { get; set; }
        public OwnershipType OwnershipType { get; set; }
        public InterestEnumType TypeOfInterest { get; set; }
        public bool AllTimeDeposits { get; set; }
        public bool TimeDeposit { get; set; }
        public bool AllOverdraftPossabilities { get; set; }
        public bool OverdraftPossability { get; set; }
        public bool AllCreditPossabilities { get; set; }
        public bool CreditPossability { get; set; }//those are bool varibles in db so only in this model we are using more data
    }
}
