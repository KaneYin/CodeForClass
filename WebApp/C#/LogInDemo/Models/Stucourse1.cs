using System;
using System.Collections.Generic;

namespace LogInDemo.Models;

public partial class Stucourse1
{
    public string CourseId { get; set; } = null!;

    public string StudentId { get; set; } = null!;

    public string? CourseName { get; set; }

    public int? CourseCredit { get; set; }
}
