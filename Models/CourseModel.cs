﻿using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


namespace StudentCatalog.Models;

public class CourseModel
{
    public int CourseModelId { get; set; }

    public int TeacherId { get; set; }

    public UserModel Teacher { get; set; }
}