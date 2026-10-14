using System.ComponentModel.DataAnnotations;

namespace LibriMini.Api.DTOs;

public class NthNhaXuatBanDto
{
    public int MaNhaXuatBan { get; set; }

    public string TenNhaXuatBan { get; set; } = string.Empty;

    public string? DiaChi { get; set; }

    public string? SoDienThoai { get; set; }

    public string? Email { get; set; }
}

public class NthNhaXuatBanTaoDto
{
    [Required(ErrorMessage = "Tên nhà xuất bản không được để trống.")]
    [StringLength(200, ErrorMessage = "Tên nhà xuất bản không được vượt quá 200 ký tự.")]
    public string TenNhaXuatBan { get; set; } = string.Empty;

    [StringLength(300, ErrorMessage = "Địa chỉ không được vượt quá 300 ký tự.")]
    public string? DiaChi { get; set; }

    [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
    [StringLength(20, ErrorMessage = "Số điện thoại không được vượt quá 20 ký tự.")]
    public string? SoDienThoai { get; set; }

    [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
    [StringLength(150, ErrorMessage = "Email không được vượt quá 150 ký tự.")]
    public string? Email { get; set; }
}

public class NthNhaXuatBanCapNhatDto : NthNhaXuatBanTaoDto
{
}
