using System;
using System.Collections.Generic;

namespace LibriMini.Api.Models;

public partial class NthBansach
{
    public int MaBanSach { get; set; }

    public int MaSach { get; set; }

    public string MaVach { get; set; } = null!;

    public string TrangThai { get; set; } = null!;

    public string? ViTriKe { get; set; }

    public virtual NthSach MaSachNavigation { get; set; } = null!;

    public virtual NthChitietphieumuon? NthChitietphieumuon { get; set; }
}
