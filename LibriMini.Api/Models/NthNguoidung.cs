using System;
using System.Collections.Generic;

namespace LibriMini.Api.Models;

public partial class NthNguoidung
{
    public int MaNguoiDung { get; set; }

    public string TenDangNhap { get; set; } = null!;

    public string MatKhauHash { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public string? Email { get; set; }

    public string VaiTro { get; set; } = null!;

    public bool TrangThai { get; set; }

    public virtual ICollection<NthPhieumuon> NthPhieumuons { get; set; } = new List<NthPhieumuon>();
}
