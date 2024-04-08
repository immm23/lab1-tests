namespace LabOOP2.Models
{
    public class CustomerInputModel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Address { get; set; }
    }
}
