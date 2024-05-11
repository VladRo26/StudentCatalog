using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace StudentCatalog.Models;

public class GroupModel
{
    public int Id { get; set; }

    [MaxLength(3, ErrorMessage = "The group should contain 3 characters.")]
    [MinLength(3, ErrorMessage = "The group should contain 3 characters.")]
    public int GroupNumber { get; set; }

}
