using StudentCatalog.Logic;

namespace StudentCatalog.Models;

public class UserModel
{
    public int Id_User { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string PasswordConfirm { get; set; }

    public string Phone { get; set; }

    public UserType Role { get; set; }



}
