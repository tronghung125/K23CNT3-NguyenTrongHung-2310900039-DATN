using System;
using System.Collections.Generic;

namespace LibriMini.Api.Models;

public partial class NthTheloai
{
    public int MaTheLoai { get; set; }

    public string TenTheLoai { get; set; } = null!;

    public string? MoTa { get; set; }

    public virtual ICollection<NthSach> NthSaches { get; set; } = new List<NthSach>();
}
