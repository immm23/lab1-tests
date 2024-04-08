using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class BankAccountTests
    {
        [Theory]
        [InlineData("number", "date", "")]
        [InlineData("number", "date", null)]
        [InlineData("number", "", "cvv")]
        [InlineData("number", null, "cvv")]
        [InlineData("", "date", "cvv")]
        [InlineData(null, "date", "cvv")]
        public void Constructor_NullStringPassed_ShouldThrowArgumentNullException(string cardNumber,
            string expiryDate, string cvv)
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                BankAccount bankAccount = new(
                    Guid.NewGuid(),
                    cardNumber,
                    expiryDate,
                    cvv);
            });
        }
    }
}
