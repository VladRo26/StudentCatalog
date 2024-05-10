using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


namespace StudentCatalog.Models;

public class CourseModel
{
    public int Id { get; set; }
    public string Name {  get; set; }

    public int? TeacherId { get; set; }

    public UserModel? Teacher { get; set; }
}