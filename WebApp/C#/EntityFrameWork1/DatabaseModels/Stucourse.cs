using System;
using System.Collections.Generic;

namespace EntityFrameWork1.DatabaseModels;

/// <summary>
/// courses that students have selected
/// </summary>
public partial class Stucourse
{
    public string StudentId { get; set; } = null!;

    public string CourseId { get; set; } = null!;

    public virtual Course Course { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
