using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class LoanTests
    {
        [Theory]
        [InlineData(11)]
        [InlineData(0)]
        [InlineData(-3)]
        public void PayOff_InvalidAmount_ShouldInvalidOperationException(decimal payOffAmount)
        {
            var loan = TestLoan();

            Assert.Throws<InvalidOperationException>(() =>
            {
                loan.PayOff(payOffAmount);
            });
        }

        [Fact]
        public void PayOff_ValidAmount_ShouldPayOff()
        {
            decimal payOffAmount = 5;
            var loan = TestLoan();

            loan.PayOff(payOffAmount);

            Assert.Equal(payOffAmount, loan.PayedOffAmount);
        }

        [Fact]
        public void PayOff_FullAmount_ShouldPayOffAndMarkAsPayed()
        {
            var loan = TestLoan();

            loan.PayOff(loan.Amount);

            Assert.Equal(loan.Amount, loan.PayedOffAmount);
            Assert.True(loan.IsPayedOff);
        }

        private Loan TestLoan()
        {
            return new (
                Guid.NewGuid(),
                10,
                12,
                0);
        }
    }
}
