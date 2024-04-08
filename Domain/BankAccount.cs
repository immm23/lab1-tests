namespace LabOOP2.Domain
{
    public class BankAccount
    {
        public Guid Id { get; private set; }
        public string CardNumber { get; private set; } = null!;
        public string ExpiryDate { get; private set; } = null!;
        public string CVV { get; private set; } = null!;

        private BankAccount() { }

        public BankAccount(Guid id, string cardNumber, string expiryDate, string cVV)
        {
            Id = id;
            CardNumber = string.IsNullOrEmpty(cardNumber)
                ? throw new ArgumentNullException(nameof(cardNumber))
                : cardNumber;
            ExpiryDate = string.IsNullOrEmpty(expiryDate)
                ? throw new ArgumentNullException(nameof(expiryDate)) 
                : expiryDate;
            CVV = string.IsNullOrEmpty(cVV)
                ? throw new ArgumentNullException(nameof(cVV)) 
                : cVV;
        }
    }
}
