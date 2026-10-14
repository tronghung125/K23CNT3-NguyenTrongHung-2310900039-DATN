using LibriMini.Api.Data;
using LibriMini.Api.DTOs;
using LibriMini.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibriMini.Api.Controllers;

[ApiController]
[Route("api/TheLoai")]
public class NthTheLoaiController : ControllerBase
{
    private readonly LibriMiniDbContext _context;

    public NthTheLoaiController(LibriMiniDbContext context)
    {
        _context = context;
    }

    // GET: api/TheLoai
    [HttpGet]
    public async Task<ActionResult<IEnumerable<NthTheLoaiDto>>> LayDanhSach(
        [FromQuery] string? tuKhoa)
    {
        var query = _context.NthTheloais.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            var tuKhoaDaCat = tuKhoa.Trim();

            query = query.Where(x =>
                x.TenTheLoai.Contains(tuKhoaDaCat));
        }

        var danhSach = await query
            .OrderBy(x => x.TenTheLoai)
            .Select(x => new NthTheLoaiDto
            {
                MaTheLoai = x.MaTheLoai,
                TenTheLoai = x.TenTheLoai,
                MoTa = x.MoTa
            })
            .ToListAsync();

        return Ok(danhSach);
    }

    // GET: api/TheLoai/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<NthTheLoaiDto>> LayTheoMa(int id)
    {
        var theLoai = await _context.NthTheloais
            .AsNoTracking()
            .Where(x => x.MaTheLoai == id)
            .Select(x => new NthTheLoaiDto
            {
                MaTheLoai = x.MaTheLoai,
                TenTheLoai = x.TenTheLoai,
                MoTa = x.MoTa
            })
            .FirstOrDefaultAsync();

        if (theLoai == null)
        {
            return NotFound(new { message = "Không tìm thấy thể loại." });
        }

        return Ok(theLoai);
    }

    // POST: api/TheLoai
    [HttpPost]
    public async Task<ActionResult<NthTheLoaiDto>> Them(
        [FromBody] NthTheLoaiTaoDto dto)
    {
        var ten = dto.TenTheLoai.Trim();

        var daTonTai = await _context.NthTheloais
            .AnyAsync(x => x.TenTheLoai == ten);

        if (daTonTai)
        {
            return Conflict(new
            {
                message = "Tên thể loại này đã tồn tại."
            });
        }

        var theLoai = new NthTheloai
        {
            TenTheLoai = ten,
            MoTa = string.IsNullOrWhiteSpace(dto.MoTa)
                ? null
                : dto.MoTa.Trim()
        };

        _context.NthTheloais.Add(theLoai);
        await _context.SaveChangesAsync();

        var ketQua = new NthTheLoaiDto
        {
            MaTheLoai = theLoai.MaTheLoai,
            TenTheLoai = theLoai.TenTheLoai,
            MoTa = theLoai.MoTa
        };

        return CreatedAtAction(
            nameof(LayTheoMa),
            new { id = theLoai.MaTheLoai },
            ketQua);
    }

    // PUT: api/TheLoai/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> CapNhat(
        int id,
        [FromBody] NthTheLoaiCapNhatDto dto)
    {
        var theLoai = await _context.NthTheloais
            .FirstOrDefaultAsync(x => x.MaTheLoai == id);

        if (theLoai == null)
        {
            return NotFound(new { message = "Không tìm thấy thể loại." });
        }

        var ten = dto.TenTheLoai.Trim();

        var trungTen = await _context.NthTheloais.AnyAsync(
            x => x.TenTheLoai == ten && x.MaTheLoai != id);

        if (trungTen)
        {
            return Conflict(new
            {
                message = "Tên thể loại này đã tồn tại."
            });
        }

        theLoai.TenTheLoai = ten;
        theLoai.MoTa = string.IsNullOrWhiteSpace(dto.MoTa)
            ? null
            : dto.MoTa.Trim();

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/TheLoai/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Xoa(int id)
    {
        var theLoai = await _context.NthTheloais
            .FirstOrDefaultAsync(x => x.MaTheLoai == id);

        if (theLoai == null)
        {
            return NotFound(new { message = "Không tìm thấy thể loại." });
        }

        var dangDuocSuDung = await _context.NthSaches
            .AnyAsync(x => x.MaTheLoai == id);

        if (dangDuocSuDung)
        {
            return Conflict(new
            {
                message = "Không thể xóa thể loại đang được sách sử dụng."
            });
        }

        _context.NthTheloais.Remove(theLoai);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
