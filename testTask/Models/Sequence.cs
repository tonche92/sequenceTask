using System;
using System.Collections.Generic;

namespace testTask.Models;

public partial class Sequence
{
    public int Id { get; set; }

    public string Prefix { get; set; } = null!;

    public string CurrentValue { get; set; } = null!;

    public int MinDigits { get; set; }

    public int? MaxCounter { get; set; }
}
