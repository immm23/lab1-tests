namespace LabOOP2.Domain
{
    public class Passport
    {
        public Guid Id { get; private set; }
        public string SerialNumber { get; private set; } = null!;
        public string IssuedByAuthority { get; private set; } = null!;
        public DateTime ExpiryDate { get; private set; }
        public DateTime IssuedDate { get; private set; }
        
        private Passport() { }

        public Passport(Guid id,
            string serialNumber,
            string issuedByAuthority,
            DateTime issuedDate,
            DateTime expiryDate) 
        {
            Id = id;
            SerialNumber = string.IsNullOrEmpty(serialNumber)
                ? throw new ArgumentNullException(nameof(serialNumber)) 
                : serialNumber;
            IssuedByAuthority = string.IsNullOrEmpty(issuedByAuthority)
                ? throw new ArgumentNullException(nameof(issuedByAuthority)) 
                : issuedByAuthority;
            ExpiryDate = expiryDate < DateTime.Now 
                ? throw new ArgumentOutOfRangeException($"{nameof(expiryDate)} is out of range. Passport is expired" )
                : expiryDate;
            IssuedDate = issuedDate > DateTime.Now || issuedDate > expiryDate
                ? throw new ArgumentOutOfRangeException($"{nameof(issuedDate)} is out of range")
                : issuedDate;
        }
    }
}
