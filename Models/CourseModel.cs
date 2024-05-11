using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;


namespace StudentCatalog.Models;

public class CourseModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Course name is required")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
    public string Name {  get; set; }

    [ForeignKey("Teacher")]
    public int? TeacherId { get; set; }

    public UserModel? Teacher { get; set; }

    [Required(ErrorMessage = "Year of the course is required")]
    [Range(1, 4, ErrorMessage = "Year must be between 2000 and 2100")]

    public int YearCourse { get; set; }
}