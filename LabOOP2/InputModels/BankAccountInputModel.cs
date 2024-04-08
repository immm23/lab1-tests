namespace LabOOP2.Models
{
    public class BankAccountInputModel
    {
        public Guid Id { get; set; }
        public required string CardNumber { get; set; }
        public required string ExpiryDate { get; set; }
        public required string CVV { get; set; }
    }
}
