using System;
using System.Collections.Generic;

namespace LibriMini.Api.Models;

public partial class NthDocgium
{
    public int MaDocGia { get; set; }

    public string MaTheDocGia { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public DateOnly? NgaySinh { get; set; }

    public string? SoDienThoai { get; set; }

    public string? Email { get; set; }

    public string? DiaChi { get; set; }

    public DateOnly NgayDangKy { get; set; }

    public bool TrangThai { get; set; }

    public virtual ICollection<NthPhieumuon> NthPhieumuons { get; set; } = new List<NthPhieumuon>();
}
