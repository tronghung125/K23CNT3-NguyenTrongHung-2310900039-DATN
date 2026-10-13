
using System.ComponentModel.DataAnnotations;

namespace LibriMini.Api.DTOs
{
    // trả dữ liệu sách về frontend
    public class SachDto
    {
        public int MaSach { get; set; }

        public string TieuDe { get; set; } = string.Empty;

        public int MaTheLoai { get; set; }

        public string? TenTheLoai { get; set; }

        public int MaNhaXuatBan { get; set; }

        public string? TenNhaXuatBan { get; set; }

        public int? NamXuatBan { get; set; }

        public string? MoTa { get; set; }

        public string? AnhBia { get; set; }
    }

    //  thêm sách
    public class SachTaoDto
    {
        [Required(ErrorMessage = "Tiêu đề sách không được để trống.")]
        [StringLength(200, ErrorMessage = "Tiêu đề sách không được vượt quá 200 ký tự.")]
        public string TieuDe { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn thể loại sách hợp lệ.")]
        public int MaTheLoai { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn nhà xuất bản hợp lệ.")]
        public int MaNhaXuatBan { get; set; }

        [Range(0, 9999, ErrorMessage = "Năm xuất bản không hợp lệ.")]
        public int? NamXuatBan { get; set; }

        public string? MoTa { get; set; }

        [StringLength(500, ErrorMessage = "Đường dẫn ảnh bìa không được vượt quá 500 ký tự.")]
        public string? AnhBia { get; set; }
    }

    // cập nhật sách
    public class SachCapNhatDto : SachTaoDto
    {
    }
}
