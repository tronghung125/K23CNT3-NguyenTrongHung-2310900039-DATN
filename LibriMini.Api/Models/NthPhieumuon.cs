using System;
using System.Collections.Generic;

namespace LibriMini.Api.Models;

public partial class NthPhieumuon
{
    public int MaPhieuMuon { get; set; }

    public int MaDocGia { get; set; }

    public int MaNguoiDung { get; set; }

    public DateOnly NgayMuon { get; set; }

    public string TrangThai { get; set; } = null!;

    public string? GhiChu { get; set; }

    public virtual NthDocgium MaDocGiaNavigation { get; set; } = null!;

    public virtual NthNguoidung MaNguoiDungNavigation { get; set; } = null!;

    public virtual ICollection<NthChitietphieumuon> NthChitietphieumuons { get; set; } = new List<NthChitietphieumuon>();
}
