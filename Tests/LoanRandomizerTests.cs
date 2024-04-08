using Microsoft.VisualStudio.TestTools.UnitTesting;
using LabOOP2.Services;

namespace LabOOP2.Tests
{
    [TestClass]
    public class LoanRandomizerTests
    {
        private LoanRandomizer _loanRandomizer = null!;

        [TestInitialize]
        public void Setup()
        {
            _loanRandomizer = new LoanRandomizer();
        }

        [TestMethod]
        public void GenerateDuration_ShouldReturnValidDuration()
        {
            // Act
            int duration = _loanRandomizer.GenerateDuration();

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(duration >= _loanRandomizer.MinMonthAmount && duration < _loanRandomizer.MaxMonthAmount);
        }

        [TestMethod]
        public void GeneratePercentage_ShouldReturnValidPercentage()
        {
            // Act
            int percentage = _loanRandomizer.GeneratePercentage();

            // Assert
            Xunit.Assert.True(percentage >= _loanRandomizer.MinPercentageAmount && percentage < _loanRandomizer.MaxPercentageAmount);
        }
    }
}
