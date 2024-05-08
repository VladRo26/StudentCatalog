namespace StudentCatalog.Models;

public class StudentModel
{
   public int Id {get; set;}
    public int UserId { get; set;}
    public UserModel User { get; set;}
    public int YearOfStudy { get; set;}
    public int GroupId { get; set;}
    public bool IsEnrolled { get; set;}
    
}
