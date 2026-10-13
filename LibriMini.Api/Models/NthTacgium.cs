using System;
using System.Collections.Generic;

namespace LibriMini.Api.Models;

public partial class NthTacgium
{
    public int MaTacGia { get; set; }

    public string HoTen { get; set; } = null!;

    public string? TieuSu { get; set; }

    public virtual ICollection<NthSach> MaSaches { get; set; } = new List<NthSach>();
}
