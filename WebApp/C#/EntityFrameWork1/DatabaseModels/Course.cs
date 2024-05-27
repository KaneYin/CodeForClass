using System;
using System.Collections.Generic;

namespace EntityFrameWork1.DatabaseModels;

public partial class Course
{
    public string CourseId { get; set; } = null!;

    public string? CourseName { get; set; }

    public int? CourseCredit { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
