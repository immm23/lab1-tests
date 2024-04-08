using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Tests
{
    public class PassportTests
    {
        [Theory]
        [InlineData("number", "")]
        [InlineData("number", null)]
        [InlineData("", "authority")]
        [InlineData(null, "authority")]
        public void Constructor_NullString_ShouldThrowArgumentNullException(
            string serialNumber, string issuedByAuthority)
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Passport passport = new(
                    Guid.NewGuid(),
                    serialNumber,
                    issuedByAuthority,
                    DateTime.Now.AddYears(-5),
                    DateTime.Now.AddYears(5));
            });
        }

        [Theory]
        [MemberData(nameof(TestDates))]
        public void Constructor_InvalidDate_ShouldThrowArgumentOutOfRangeException(
            DateTime issuedDate, DateTime expiryDate)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Passport passport = new(
                    Guid.NewGuid(),
                    "number",
                    "authority",
                    issuedDate,
                    expiryDate);
            });
        }

        public static IEnumerable<object[]> TestDates =>
             new List<object[]>
             {
                    new object[] { DateTime.Now.AddYears(-5), DateTime.Now.AddYears(-1) },
                    new object[] { DateTime.Now.AddYears(-5), DateTime.Now.AddYears(-6) },
                    new object[] { DateTime.Now.AddYears(4), DateTime.Now.AddYears(5) }
             };

    }
}
