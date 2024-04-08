namespace LabOOP2.Domain.Services
{
    public interface ILoanRandomizer
    {
        public int GenerateDuration();
        public int GeneratePercentage();
    }
}
