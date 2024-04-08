using LabOOP2.Domain.Services;
using Microsoft.AspNetCore.Routing;
using Moq;
using NHamcrest.Core;
using NHamcrest;
using System.Security.Policy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Xunit.Assert;

namespace Tests
{
    public class CustomerTests
    {
        [Theory]
        [InlineData("name", "surname", "")]
        [InlineData("name", "surname", null)]
        [InlineData("name", "", "address")]
        [InlineData("name", null, "address")]
        [InlineData("", "surname", "address")]
        [InlineData(null, "surname", "address")]
        public void Constructor_NullStringPassed_ShouldThrowArgumentNullException(string name,
            string surname, string address)
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Customer customer = new(
                    Guid.NewGuid(),
                    name,
                    surname,
                    address);
            });
        }

        [Fact]
        public void SetPassport_ValidPassport_ShouldSetPassport()
        {
            Customer customer = TestCustomer();
            Passport passport = TestPassport();

            customer.SetPassport(passport);

            Assert.Equal(passport, customer.Passport);
        }

        [Fact]
        public void SetPassport_NullPassport_ShouldThrowArgumentNullException()
        {
            Customer customer = TestCustomer();

            Assert.Throws<ArgumentNullException>(() =>
            {
                customer.SetPassport(null!) ;
            });
        }

        [Fact]
        public void SetBankAccount_ValidBankAccount_ShouldSetPassport()
        {
            Customer customer = TestCustomer();
            BankAccount bankAccount = TestBankAccount();

            customer.SetBankAccount(bankAccount);

            Assert.Equal(bankAccount, customer.BankAccount);
        }

        [Fact]
        public void SetPassport_NullBankAccount_ShouldThrowArgumentNullException()
        {
            Customer customer = TestCustomer();

            Assert.Throws<ArgumentNullException>(() =>
            {
                customer.SetBankAccount(null!);
            });
        }

        [Fact]
        public void TakeLoan_PassportIsNull_ShouldThrowInvalidOperationException()
        {
            decimal amount = 10;
            Customer customer = TestCustomer();
            BankAccount bankAccount = TestBankAccount();
            customer.SetBankAccount(bankAccount);

            var loanRandomizerMock = new Mock<ILoanRandomizer>();

            Assert.Throws<InvalidOperationException>(() =>
            {
                customer.TakeLoan(amount, loanRandomizerMock.Object);
            });
        }

        [Fact]
        public void TakeLoan_BankAccountIsNull_ShouldThrowInvalidOperationException()
        {
            decimal amount = 10;
            Customer customer = TestCustomer();
            Passport passport = TestPassport();
            customer.SetPassport(passport);

            var loanRandomizerMock = new Mock<ILoanRandomizer>();

            Assert.Throws<InvalidOperationException>(() =>
            {
                customer.TakeLoan(amount, loanRandomizerMock.Object);
            });
        }

        [TestMethod]
        public void TakeLoan_EligibleForLoan_ShouldChangeBalanceAndCreateTransaction_Hamcrest()
        {
            decimal amount = 10;
            int duration = 12;
            int percentage = 15;
            Customer customer = TestCustomer();
            Passport passport = TestPassport();
            BankAccount bankAccount = TestBankAccount();
            customer.SetPassport(passport);
            customer.SetBankAccount(bankAccount);

            var loanRandomizerMock = new Mock<ILoanRandomizer>();
            loanRandomizerMock.Setup(x => x.GeneratePercentage()).Returns(percentage);
            loanRandomizerMock.Setup(x => x.GenerateDuration()).Returns(duration);

            customer.TakeLoan(amount, loanRandomizerMock.Object);

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(amount, customer.Balance);
            CollectionAssert.Contains(customer.Loans.ToList(), new Loan
            {
                Percentage = percentage,
                Amount = amount
            });
            CollectionAssert.Contains(customer.Transactions.ToList(), new Transaction
            {
                FromAccount = AccountType.Loan,
                ToAccount = AccountType.Balance,
                Amount = amount
            });
        }

        [TestMethod]
        public void MoveBalance_ValidRequest_ShouldChangeBalanceAndCallService_Hamcrest()
        {
            decimal loanAmount = 10;
            decimal moveAmount = 10;
            int duration = 12;
            int percentage = 15;
            Customer customer = TestCustomer();
            Passport passport = TestPassport();
            BankAccount bankAccount = TestBankAccount();
            customer.SetBankAccount(bankAccount);
            customer.SetPassport(passport);

            var paymentServiceMock = new Mock<IPaymentService>();
            var loanRandomizerMock = new Mock<ILoanRandomizer>();
            loanRandomizerMock.Setup(x => x.GeneratePercentage()).Returns(percentage);
            loanRandomizerMock.Setup(x => x.GenerateDuration()).Returns(duration);

            customer.TakeLoan(loanAmount, loanRandomizerMock.Object);
            customer.MoveBalance(moveAmount, paymentServiceMock.Object);

            paymentServiceMock.Verify(x => x.MakeTransaction(It.IsAny<Customer>(), It.IsAny<Transaction>()));
            CollectionAssert.AllItemsAreNotNull(customer.Transactions.ToList());
            CollectionAssert.Contains(customer.Transactions.ToList(), new Transaction
            {
                FromAccount = AccountType.Balance,
                ToAccount = AccountType.BankAccount,
                Amount = moveAmount
            });
        }

        [Fact]
        public void PayOffLoan_NotExistingLoan_ShouldThrowException()
        {
            decimal loanAmount = 10;
            decimal payOffAmount = 10;
            int duration = 12;
            int percentage = 15;
            Customer customer = TestCustomer();
            Passport passport = TestPassport();
            BankAccount bankAccount = TestBankAccount();
            customer.SetPassport(passport);
            customer.SetBankAccount(bankAccount);

            var loanRandomizerMock = new Mock<ILoanRandomizer>();
            loanRandomizerMock.Setup(x => x.GeneratePercentage()).Returns(percentage);
            loanRandomizerMock.Setup(x => x.GenerateDuration()).Returns(duration);

            customer.TakeLoan(loanAmount, loanRandomizerMock.Object);

            Assert.Throws<InvalidOperationException>(() =>
            {
                customer.PayOffLoan(Guid.NewGuid(), payOffAmount);
            });
        }

        [Fact]
        public void MoveBalance_BankAccountIsNull_ShouldThrowInvalidOperationException()
        {
            decimal amount = 10;
            Customer customer = TestCustomer();
            var paymentServiceMock = new Mock<IPaymentService>();

            Assert.Throws<InvalidOperationException>(() =>
            {
                customer.MoveBalance(amount, paymentServiceMock.Object);
            });
        }

        [Theory]
        [InlineData(10)]
        [InlineData(0)]
        [InlineData(-5)]
        public void MoveBalance_BalanceIsNotBigEnough_ShouldThrowInvalidOperationException(
            decimal amount)
        {
            Customer customer = TestCustomer();
            BankAccount bankAccount = TestBankAccount();
            customer.SetBankAccount(bankAccount);

            var paymentServiceMock = new Mock<IPaymentService>();

            Assert.Throws<InvalidOperationException>(() =>
            {
                customer.MoveBalance(amount, paymentServiceMock.Object);
            });
        }

        [Fact]
        public void MoveBalance_ValidRequest_ShouldChangeBalanceAndCallService()
        {
            decimal loanAmount = 10;
            decimal moveAmount = 10;
            int duration = 12;
            int percentage = 15;
            Customer customer = TestCustomer();
            Passport passport = TestPassport();
            BankAccount bankAccount = TestBankAccount();
            customer.SetBankAccount(bankAccount);
            customer.SetPassport(passport);

            var paymentServiceMock = new Mock<IPaymentService>();
            var loanRandomizerMock = new Mock<ILoanRandomizer>();
            loanRandomizerMock.Setup(x => x.GeneratePercentage()).Returns(percentage);
            loanRandomizerMock.Setup(x => x.GenerateDuration()).Returns(duration);

            customer.TakeLoan(loanAmount, loanRandomizerMock.Object);
            customer.MoveBalance(moveAmount, paymentServiceMock.Object);

            paymentServiceMock.Verify(x => x.MakeTransaction(It.IsAny<Customer>(), It.IsAny<Transaction>()));
            Assert.NotEmpty(customer.Transactions);
            Assert.Contains(customer.Transactions, transaction =>
            {
                return transaction.FromAccount == AccountType.Balance
                && transaction.ToAccount == AccountType.BankAccount
                && transaction.Amount == moveAmount;
            });
        }



        private Customer TestCustomer()
        {
            return new(
               Guid.NewGuid(),
               "name",
               "surname",
               "address");
        }

        private Passport TestPassport()
        {
            return new(
                Guid.NewGuid(),
                "serialNumber",
                "authority",
                DateTime.Now.AddYears(-5),
                DateTime.Now.AddYears(5));
        }

        private BankAccount TestBankAccount()
        {
            return new(
               Guid.NewGuid(),
               "cardNumber",
               "expiry date",
               "cvv");
        }
    }
}