using System;
using System.Collections.Generic;

namespace LibriMini.Api.Models;

public partial class NthTienphat
{
    public int MaTienPhat { get; set; }

    public int MaChiTiet { get; set; }

    public decimal SoTien { get; set; }

    public string LyDo { get; set; } = null!;

    public DateOnly NgayLap { get; set; }

    public string TrangThai { get; set; } = null!;

    public virtual NthChitietphieumuon MaChiTietNavigation { get; set; } = null!;
}
