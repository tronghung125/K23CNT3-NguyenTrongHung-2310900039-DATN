using LibriMini.Api.Data;
using LibriMini.Api.DTOs;
using LibriMini.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibriMini.Api.Controllers;

[ApiController]
[Route("api/NhaXuatBan")]
public class NthNhaXuatBanController : ControllerBase
{
    private readonly LibriMiniDbContext _context;

    public NthNhaXuatBanController(LibriMiniDbContext context)
    {
        _context = context;
    }

    // GET: api/NhaXuatBan
    [HttpGet]
    public async Task<ActionResult<IEnumerable<NthNhaXuatBanDto>>> LayDanhSach(
        [FromQuery] string? tuKhoa)
    {
        var query = _context.NthNhaxuatbans.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            var tuKhoaDaCat = tuKhoa.Trim();

            query = query.Where(x =>
                x.TenNhaXuatBan.Contains(tuKhoaDaCat));
        }

        var danhSach = await query
            .OrderBy(x => x.TenNhaXuatBan)
            .Select(x => new NthNhaXuatBanDto
            {
                MaNhaXuatBan = x.MaNhaXuatBan,
                TenNhaXuatBan = x.TenNhaXuatBan,
                DiaChi = x.DiaChi,
                SoDienThoai = x.SoDienThoai,
                Email = x.Email
            })
            .ToListAsync();

        return Ok(danhSach);
    }

    // GET: api/NhaXuatBan/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<NthNhaXuatBanDto>> LayTheoMa(int id)
    {
        var nhaXuatBan = await _context.NthNhaxuatbans
            .AsNoTracking()
            .Where(x => x.MaNhaXuatBan == id)
            .Select(x => new NthNhaXuatBanDto
            {
                MaNhaXuatBan = x.MaNhaXuatBan,
                TenNhaXuatBan = x.TenNhaXuatBan,
                DiaChi = x.DiaChi,
                SoDienThoai = x.SoDienThoai,
                Email = x.Email
            })
            .FirstOrDefaultAsync();

        if (nhaXuatBan == null)
        {
            return NotFound(new
            {
                message = "Không tìm thấy nhà xuất bản."
            });
        }

        return Ok(nhaXuatBan);
    }

    // POST: api/NhaXuatBan
    [HttpPost]
    public async Task<ActionResult<NthNhaXuatBanDto>> Them(
        [FromBody] NthNhaXuatBanTaoDto dto)
    {
        var ten = dto.TenNhaXuatBan.Trim();

        var daTonTai = await _context.NthNhaxuatbans
            .AnyAsync(x => x.TenNhaXuatBan == ten);

        if (daTonTai)
        {
            return Conflict(new
            {
                message = "Tên nhà xuất bản này đã tồn tại."
            });
        }

        var nhaXuatBan = new NthNhaxuatban
        {
            TenNhaXuatBan = ten,
            DiaChi = string.IsNullOrWhiteSpace(dto.DiaChi)
                ? null
                : dto.DiaChi.Trim(),
            SoDienThoai = string.IsNullOrWhiteSpace(dto.SoDienThoai)
                ? null
                : dto.SoDienThoai.Trim(),
            Email = string.IsNullOrWhiteSpace(dto.Email)
                ? null
                : dto.Email.Trim()
        };

        _context.NthNhaxuatbans.Add(nhaXuatBan);
        await _context.SaveChangesAsync();

        var ketQua = new NthNhaXuatBanDto
        {
            MaNhaXuatBan = nhaXuatBan.MaNhaXuatBan,
            TenNhaXuatBan = nhaXuatBan.TenNhaXuatBan,
            DiaChi = nhaXuatBan.DiaChi,
            SoDienThoai = nhaXuatBan.SoDienThoai,
            Email = nhaXuatBan.Email
        };

        return CreatedAtAction(
            nameof(LayTheoMa),
            new { id = nhaXuatBan.MaNhaXuatBan },
            ketQua);
    }

    // PUT: api/NhaXuatBan/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> CapNhat(
        int id,
        [FromBody] NthNhaXuatBanCapNhatDto dto)
    {
        var nhaXuatBan = await _context.NthNhaxuatbans
            .FirstOrDefaultAsync(x => x.MaNhaXuatBan == id);

        if (nhaXuatBan == null)
        {
            return NotFound(new
            {
                message = "Không tìm thấy nhà xuất bản."
            });
        }

        var ten = dto.TenNhaXuatBan.Trim();

        var trungTen = await _context.NthNhaxuatbans.AnyAsync(
            x => x.TenNhaXuatBan == ten && x.MaNhaXuatBan != id);

        if (trungTen)
        {
            return Conflict(new
            {
                message = "Tên nhà xuất bản này đã tồn tại."
            });
        }

        nhaXuatBan.TenNhaXuatBan = ten;
        nhaXuatBan.DiaChi = string.IsNullOrWhiteSpace(dto.DiaChi)
            ? null
            : dto.DiaChi.Trim();
        nhaXuatBan.SoDienThoai =
            string.IsNullOrWhiteSpace(dto.SoDienThoai)
                ? null
                : dto.SoDienThoai.Trim();
        nhaXuatBan.Email = string.IsNullOrWhiteSpace(dto.Email)
            ? null
            : dto.Email.Trim();

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/NhaXuatBan/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Xoa(int id)
    {
        var nhaXuatBan = await _context.NthNhaxuatbans
            .FirstOrDefaultAsync(x => x.MaNhaXuatBan == id);

        if (nhaXuatBan == null)
        {
            return NotFound(new
            {
                message = "Không tìm thấy nhà xuất bản."
            });
        }

        var dangDuocSuDung = await _context.NthSaches
            .AnyAsync(x => x.MaNhaXuatBan == id);

        if (dangDuocSuDung)
        {
            return Conflict(new
            {
                message = "Không thể xóa nhà xuất bản đang được sách sử dụng."
            });
        }

        _context.NthNhaxuatbans.Remove(nhaXuatBan);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}