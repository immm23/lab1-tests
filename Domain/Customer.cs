using LabOOP2.Domain.Services;

namespace LabOOP2.Domain
{
    public class Customer
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string Surname { get; private set; } = null!;
        public string Address { get; private set; } = null!;
        public decimal Balance { get; private set; }
        public virtual Passport? Passport { get; private set; }
        public IReadOnlyCollection<Loan> Loans => _loans;
        public IReadOnlyCollection<Transaction> Transactions => _transactions;
        public virtual BankAccount? BankAccount { get; private set; }
        private List<Loan> _loans = new List<Loan>();
        private List<Transaction> _transactions = new List<Transaction>();

        private Customer() { }

        public Customer(Guid id, string name, string surname, string address)
        {
            Id = id;
            Name = string.IsNullOrEmpty(name)
                ? throw new ArgumentNullException(nameof(name))
                : name;
            Surname = string.IsNullOrEmpty(surname)
                ? throw new ArgumentNullException(nameof(surname)) 
                : surname;
            Address = string.IsNullOrEmpty(address)
                ? throw new ArgumentNullException(nameof(address)) 
                : address;
        }

        public void SetPassport(Passport passport)
        {
            Passport = passport ?? throw new ArgumentNullException(nameof(passport));
        }

        public void SetBankAccount(BankAccount bankAccount)
        {
            BankAccount = bankAccount ?? throw new ArgumentNullException(nameof(bankAccount));
        }

        public void MoveBalance(decimal amount, IPaymentService paymentService)
        {
            if (BankAccount is null)
            {
                throw new InvalidOperationException("Customers bank account is null");
            }
            else if (amount > Balance || amount <= 0)
            {
                throw new InvalidOperationException("Requested amount is higher than balance");
            }

            Balance -= amount;
            var transaction = new Transaction(
                Guid.NewGuid(),
                amount,
                AccountType.Balance,
                AccountType.BankAccount,
                DateTime.Now);
            _transactions.Add(transaction);
            paymentService.MakeTransaction(this, transaction);
        }

        public void TakeLoan(decimal amount, ILoanRandomizer randomizer)
        {
            if (!IsEligibleForLoan())
            {
                throw new InvalidOperationException("Customer bank acount or passport are null");
            }

            var percentage = randomizer.GeneratePercentage();
            var duration = randomizer.GenerateDuration();

            var loan = new Loan(
                Guid.NewGuid(),
                amount,
                duration,
                percentage);
            var transaction = new Transaction(
                Guid.NewGuid(),
                amount,
                AccountType.Loan,
                AccountType.Balance,
                DateTime.Now);

            _loans.Add(loan);
            _transactions.Add(transaction);
            Balance += amount;
        }

        public void PayOffLoan(Guid id, decimal amount)
        {
            //ArgumentNullException if no loan found
            Loans.Where(p => p.Id == id).First().PayOff(amount);

            var transaction = new Transaction(
               Guid.NewGuid(),
               amount,
               AccountType.BankAccount,
               AccountType.Loan,
               DateTime.Now);
            _transactions.Add(transaction);
        }

        private bool IsEligibleForLoan()
        {
            return Passport is not null
                && BankAccount is not null;
        }
    }
}
