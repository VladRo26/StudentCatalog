
namespace StudentCatalog.Models
{
    public class AlertModel
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public StudentModel Student { get; set; }

        public string Alert { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;

        public AlertModel(StudentModel student,string alertMessage)
        {
            this.Student = student;
            this.StudentId=student.Id;
            this.Alert = alertMessage;
            this.CreationTime = DateTime.Now;
            this.IsRead = false;
        }

        public AlertModel()
        {
        }

    }
}