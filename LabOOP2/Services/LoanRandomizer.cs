using LabOOP2.Domain.Services;

namespace LabOOP2.Services
{
    public class LoanRandomizer : ILoanRandomizer
    {
        private readonly Random _randomizer;
        public int MinMonthAmount = 1;
        public int MaxMonthAmount = 36;
        public int MinPercentageAmount = 0;
        public int MaxPercentageAmount = 250;

        public LoanRandomizer()
        {
            _randomizer = new Random();
        }

        public int GenerateDuration()
        {
            return _randomizer.Next(MinMonthAmount, MaxMonthAmount);
        }

        public int GeneratePercentage()
        {
            return _randomizer.Next(MinPercentageAmount, MaxPercentageAmount);
        }
    }
}
