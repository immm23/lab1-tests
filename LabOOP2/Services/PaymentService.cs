using LabOOP2.Domain;
using LabOOP2.Domain.Services;
using LabOOP2.Models;

namespace LabOOP2.Services
{
    public class PaymentService : IPaymentService
    {
        public void MakeTransaction(Customer customer, Transaction transaction)
        {
            Console.WriteLine($"Customer with name {customer.Name} {customer.Surname} " +
                $"made transaction from {transaction.FromAccount} to {transaction.ToAccount} " +
                $"with amount {transaction.Amount}");
        }
    }
}
