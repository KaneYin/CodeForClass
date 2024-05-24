using System;
using System.Collections.Generic;

namespace EntityFrameWork1.DatabaseModels;

public partial class Student
{
    public string StudentId { get; set; } = null!;

    public string? StudentName { get; set; }

    public string? StudentClass { get; set; }

    public string? InitPassword { get; set; }
}
