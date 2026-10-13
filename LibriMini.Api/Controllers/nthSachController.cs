
using LibriMini.Api.Data;
using LibriMini.Api.DTOs;
using LibriMini.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibriMini.Api.Controllers
{
    [Route("api/Sach")]
    [ApiController]
    public class NthSachController : ControllerBase
    {
        private readonly LibriMiniDbContext _context;

        public NthSachController(LibriMiniDbContext context)
        {
            _context = context;
        }

        // GET: api/Sach
        // GET: api/Sach?tuKhoa=lap%20trinh
        // GET: api/Sach?maTheLoai=1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SachDto>>> LayDanhSachSach(
            [FromQuery] string? tuKhoa,
            [FromQuery] int? maTheLoai)
        {
            var query = _context.NthSaches
                .AsNoTracking()
                .AsQueryable();

            // tìm theo tiêu đề sách
            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                string tuKhoaDaTrim = tuKhoa.Trim();

                query = query.Where(s =>
                    s.TieuDe.Contains(tuKhoaDaTrim));
            }

            // lọc thể loại
            if (maTheLoai.HasValue)
            {
                query = query.Where(s =>
                    s.MaTheLoai == maTheLoai.Value);
            }

            var danhSachSach = await query
                .OrderBy(s => s.MaSach)
                .Select(s => new SachDto
                {
                    MaSach = s.MaSach,
                    TieuDe = s.TieuDe,
                    MaTheLoai = s.MaTheLoai,
                    TenTheLoai = s.MaTheLoaiNavigation.TenTheLoai,
                    MaNhaXuatBan = s.MaNhaXuatBan,
                    TenNhaXuatBan = s.MaNhaXuatBanNavigation.TenNhaXuatBan,
                    NamXuatBan = s.NamXuatBan,
                    MoTa = s.MoTa,
                    AnhBia = s.AnhBia
                })
                .ToListAsync();

            return Ok(danhSachSach);
        }

        // GET: api/Sach/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<SachDto>> LaySachTheoMa(int id)
        {
            var sach = await _context.NthSaches
                .AsNoTracking()
                .Where(s => s.MaSach == id)
                .Select(s => new SachDto
                {
                    MaSach = s.MaSach,
                    TieuDe = s.TieuDe,
                    MaTheLoai = s.MaTheLoai,
                    TenTheLoai = s.MaTheLoaiNavigation.TenTheLoai,
                    MaNhaXuatBan = s.MaNhaXuatBan,
                    TenNhaXuatBan = s.MaNhaXuatBanNavigation.TenNhaXuatBan,
                    NamXuatBan = s.NamXuatBan,
                    MoTa = s.MoTa,
                    AnhBia = s.AnhBia
                })
                .FirstOrDefaultAsync();

            if (sach == null)
            {
                return NotFound(new
                {
                    message = $"Không tìm thấy sách có mã {id}."
                });
            }

            return Ok(sach);
        }

        // POST: api/Sach
        [HttpPost]
        public async Task<ActionResult<SachDto>> ThemSach(
            [FromBody] SachTaoDto dto)
        {
            // kiểm tra thể loại có / không
            bool theLoaiTonTai = await _context.NthTheloais
                .AnyAsync(t => t.MaTheLoai == dto.MaTheLoai);

            if (!theLoaiTonTai)
            {
                return BadRequest(new
                {
                    message = "Thể loại sách không tồn tại."
                });
            }

            // kiểm tra nhà xuất bản có / không
            bool nhaXuatBanTonTai = await _context.NthNhaxuatbans
                .AnyAsync(n => n.MaNhaXuatBan == dto.MaNhaXuatBan);

            if (!nhaXuatBanTonTai)
            {
                return BadRequest(new
                {
                    message = "Nhà xuất bản không tồn tại."
                });
            }

            var sach = new NthSach
            {
                TieuDe = dto.TieuDe.Trim(),
                MaTheLoai = dto.MaTheLoai,
                MaNhaXuatBan = dto.MaNhaXuatBan,
                NamXuatBan = dto.NamXuatBan,
                MoTa = dto.MoTa,
                AnhBia = dto.AnhBia
            };

            _context.NthSaches.Add(sach);
            await _context.SaveChangesAsync();

            var sachDto = await TaoSachDtoTheoMa(sach.MaSach);

            return CreatedAtAction(
                nameof(LaySachTheoMa),
                new { id = sach.MaSach },
                sachDto);
        }

        // PUT: api/Sach/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> CapNhatSach(
            int id,
            [FromBody] SachCapNhatDto dto)
        {
            var sach = await _context.NthSaches
                .FirstOrDefaultAsync(s => s.MaSach == id);

            if (sach == null)
            {
                return NotFound(new
                {
                    message = $"Không tìm thấy sách có mã {id}."
                });
            }

            bool theLoaiTonTai = await _context.NthTheloais
                .AnyAsync(t => t.MaTheLoai == dto.MaTheLoai);

            if (!theLoaiTonTai)
            {
                return BadRequest(new
                {
                    message = "Thể loại sách không tồn tại."
                });
            }

            bool nhaXuatBanTonTai = await _context.NthNhaxuatbans
                .AnyAsync(n => n.MaNhaXuatBan == dto.MaNhaXuatBan);

            if (!nhaXuatBanTonTai)
            {
                return BadRequest(new
                {
                    message = "Nhà xuất bản không tồn tại."
                });
            }

            sach.TieuDe = dto.TieuDe.Trim();
            sach.MaTheLoai = dto.MaTheLoai;
            sach.MaNhaXuatBan = dto.MaNhaXuatBan;
            sach.NamXuatBan = dto.NamXuatBan;
            sach.MoTa = dto.MoTa;
            sach.AnhBia = dto.AnhBia;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Sach/1
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> XoaSach(int id)
        {
            var sach = await _context.NthSaches
                .FirstOrDefaultAsync(s => s.MaSach == id);

            if (sach == null)
            {
                return NotFound(new
                {
                    message = $"Không tìm thấy sách có mã {id}."
                });
            }

            // 0 xóa sách khi còn bản sách trong kho.
            bool coBanSach = await _context.NthBansaches
                .AnyAsync(b => b.MaSach == id);

            if (coBanSach)
            {
                return Conflict(new
                {
                    message = "Không thể xóa sách vì vẫn còn bản sách liên quan. Hãy xử lý các bản sách trước."
                });
            }

            _context.NthSaches.Remove(sach);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict(new
                {
                    message = "Không thể xóa sách vì sách đang được tham chiếu bởi dữ liệu khác."
                });
            }

            return NoContent();
        }

        // tạo DTO từ mã sách vừa thêm
        private async Task<SachDto?> TaoSachDtoTheoMa(int maSach)
        {
            return await _context.NthSaches
                .AsNoTracking()
                .Where(s => s.MaSach == maSach)
                .Select(s => new SachDto
                {
                    MaSach = s.MaSach,
                    TieuDe = s.TieuDe,
                    MaTheLoai = s.MaTheLoai,
                    TenTheLoai = s.MaTheLoaiNavigation.TenTheLoai,
                    MaNhaXuatBan = s.MaNhaXuatBan,
                    TenNhaXuatBan = s.MaNhaXuatBanNavigation.TenNhaXuatBan,
                    NamXuatBan = s.NamXuatBan,
                    MoTa = s.MoTa,
                    AnhBia = s.AnhBia
                })
                .FirstOrDefaultAsync();
        }
    }
}
