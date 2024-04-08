namespace LabOOP2.Models
{
    public class PassportInputModel
    {
        public Guid Id { get; set; }
        public required string SerialNumber { get; set; }
        public required string IssuedByAuthority { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime IssuedDate { get; set; }
    }
}
