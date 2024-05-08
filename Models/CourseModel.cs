using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


namespace StudentCatalog.Models;

public class CourseModel
{
    public int CourseModelId { get; set; }
    public string Name {  get; set; }

    public int TeacherId { get; set; }

    public UserModel Teacher { get; set; }

    [Range(1, 6, ErrorMessage = "Please enter a valid year.")]
    public int YearCourse { get; set; }
}