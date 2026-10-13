using System;
using System.Collections.Generic;

namespace LibriMini.Api.Models;

public partial class NthSach
{
    public int MaSach { get; set; }

    public string TieuDe { get; set; } = null!;

    public int MaTheLoai { get; set; }

    public int MaNhaXuatBan { get; set; }

    public int? NamXuatBan { get; set; }

    public string? MoTa { get; set; }

    public string? AnhBia { get; set; }

    public virtual NthNhaxuatban MaNhaXuatBanNavigation { get; set; } = null!;

    public virtual NthTheloai MaTheLoaiNavigation { get; set; } = null!;

    public virtual ICollection<NthBansach> NthBansaches { get; set; } = new List<NthBansach>();

    public virtual ICollection<NthTacgium> MaTacGia { get; set; } = new List<NthTacgium>();
}
