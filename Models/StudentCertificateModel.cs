using System;
using System.ComponentModel.DataAnnotations;

namespace StudentCatalog.Models
{
    public class StudentCertificateModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Reason for the certificate is required.")]
        public required string  Reason { get; set; }

        public int StudentId {  get; set; }
        public required StudentModel Student { get; set; }

        public required string FirstName { get; set; }
        public string LastName { get; set; }
        [RegularExpression(@"^07[0-9]{8}$", ErrorMessage = "Phone number must start with 07 and be 10 digits long")]
        public required string PhoneNumber { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
