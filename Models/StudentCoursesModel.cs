using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCatalog.Models
{
    public class StudentCoursesModel
    {
        public int Id { get; set; }

        [Required]
        public int StudentId {  get; set; }
        //[ForeignKey("StudentId")]
        public StudentModel? Student { get; set; }

        [ForeignKey("CourseId")]
        public int? CourseId {  get; set; }
        public CourseModel? Course { get; set; }
        [Range(0, 10, ErrorMessage = "Grade should be between 0 and 10.")]
        public float Grade { get; set; }

    }
}
