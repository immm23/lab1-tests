namespace LabOOP2.Domain
{
    public class Transaction
    {
        public Guid Id { get;  set; }
        public decimal Amount { get;  set; }
        public AccountType FromAccount { get;  set; }
        public AccountType ToAccount { get;  set; }
        public DateTime Time { get;  set; }
        
        public Transaction() { }

        public Transaction(Guid id,
            decimal amount,
            AccountType fromAccount,
            AccountType toAccount,
            DateTime time)
        {
            Id = id;
            Amount = amount;
            FromAccount = fromAccount;
            ToAccount = toAccount;
            Time = time;
        }
    }
}
