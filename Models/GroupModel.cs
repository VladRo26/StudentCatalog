using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace StudentCatalog.Models;

public class GroupModel
{
    public int Id { get; set; }

    [Range(100, 999, ErrorMessage = "The group number should be a 3-digit number.")]
    public int GroupNumber { get; set; }

}
