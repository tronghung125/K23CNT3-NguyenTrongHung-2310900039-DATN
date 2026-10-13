using System;
using System.Collections.Generic;

namespace LibriMini.Api.Models;

public partial class NthNhaxuatban
{
    public int MaNhaXuatBan { get; set; }

    public string TenNhaXuatBan { get; set; } = null!;

    public string? DiaChi { get; set; }

    public string? SoDienThoai { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<NthSach> NthSaches { get; set; } = new List<NthSach>();
}
