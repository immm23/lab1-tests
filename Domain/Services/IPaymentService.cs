namespace LabOOP2.Domain.Services
{
    public interface IPaymentService
    {
        public void MakeTransaction(Customer customer, Transaction transaction);
    }
}
