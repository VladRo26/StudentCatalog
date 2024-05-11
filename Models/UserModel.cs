using StudentCatalog.Logic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace StudentCatalog.Models
{
    public class UserModel
    {
        public int Id { get; set; }

         [Required(ErrorMessage = "First name is required")]
         public string FirstName { get; set; }

         [Required(ErrorMessage = "Last name is required")]
         public string LastName { get; set; }

         [EmailAddress(ErrorMessage = "Invalid Email Address")]
         public string Email { get; set; }

         [Required(ErrorMessage = "Username is required")]
         public string Username { get; set; }

         [Required(ErrorMessage = "Password is required")]
         [DataType(DataType.Password)]
         public string Password { get; set; }

         [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
         [DataType(DataType.Password)]
         public string PasswordConfirm { get; set; }

         [RegularExpression(@"^07[0-9]{8}$", ErrorMessage = "Phone number must start with 07 and be 10 digits long")]
         public string Phone { get; set; }

         [Required(ErrorMessage = "Role is required")]
         public UserType Role { get; set; }

       


    }
}
