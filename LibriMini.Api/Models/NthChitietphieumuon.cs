using System;
using System.Collections.Generic;

namespace LibriMini.Api.Models;

public partial class NthChitietphieumuon
{
    public int MaChiTiet { get; set; }

    public int MaPhieuMuon { get; set; }

    public int MaBanSach { get; set; }

    public DateOnly HanTra { get; set; }

    public DateOnly? NgayTra { get; set; }

    public string? GhiChu { get; set; }

    public virtual NthBansach MaBanSachNavigation { get; set; } = null!;

    public virtual NthPhieumuon MaPhieuMuonNavigation { get; set; } = null!;

    public virtual ICollection<NthTienphat> NthTienphats { get; set; } = new List<NthTienphat>();
}
