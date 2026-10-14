using System.ComponentModel.DataAnnotations;

namespace LibriMini.Api.DTOs;

public class NthTheLoaiDto
{
    public int MaTheLoai { get; set; }

    public string TenTheLoai { get; set; } = string.Empty;

    public string? MoTa { get; set; }
}

public class NthTheLoaiTaoDto
{
    [Required(ErrorMessage = "Tên thể loại không được để trống.")]
    [StringLength(100, ErrorMessage = "Tên thể loại không được vượt quá 100 ký tự.")]
    public string TenTheLoai { get; set; } = string.Empty;

    public string? MoTa { get; set; }
}

public class NthTheLoaiCapNhatDto : NthTheLoaiTaoDto
{
}
