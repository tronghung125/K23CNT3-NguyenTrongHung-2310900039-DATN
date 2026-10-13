using System;
using System.Collections.Generic;
using LibriMini.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LibriMini.Api.Data;

public partial class LibriMiniDbContext : DbContext
{
    public LibriMiniDbContext(DbContextOptions<LibriMiniDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<NthBansach> NthBansaches { get; set; }

    public virtual DbSet<NthChitietphieumuon> NthChitietphieumuons { get; set; }

    public virtual DbSet<NthDocgium> NthDocgia { get; set; }

    public virtual DbSet<NthNguoidung> NthNguoidungs { get; set; }

    public virtual DbSet<NthNhaxuatban> NthNhaxuatbans { get; set; }

    public virtual DbSet<NthPhieumuon> NthPhieumuons { get; set; }

    public virtual DbSet<NthSach> NthSaches { get; set; }

    public virtual DbSet<NthTacgium> NthTacgia { get; set; }

    public virtual DbSet<NthTheloai> NthTheloais { get; set; }

    public virtual DbSet<NthTienphat> NthTienphats { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NthBansach>(entity =>
        {
            entity.HasKey(e => e.MaBanSach).HasName("PK__nth_bans__CA8BF613064CB55C");

            entity.ToTable("nth_bansach");

            entity.HasIndex(e => e.MaVach, "UQ__nth_bans__8BBF4A1CEEF30E0F").IsUnique();

            entity.Property(e => e.MaVach).HasMaxLength(30);
            entity.Property(e => e.TrangThai)
                .HasMaxLength(30)
                .HasDefaultValue("SanSang");
            entity.Property(e => e.ViTriKe).HasMaxLength(50);

            entity.HasOne(d => d.MaSachNavigation).WithMany(p => p.NthBansaches)
                .HasForeignKey(d => d.MaSach)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_bansach_sach");
        });

        modelBuilder.Entity<NthChitietphieumuon>(entity =>
        {
            entity.HasKey(e => e.MaChiTiet).HasName("PK__nth_chit__CDF0A11446DBDEF6");

            entity.ToTable("nth_chitietphieumuon");

            entity.HasIndex(e => new { e.HanTra, e.NgayTra }, "IX_chitiet_HanTra_NgayTra");

            entity.HasIndex(e => e.MaBanSach, "UX_chitiet_BanSach_DangMuon")
                .IsUnique()
                .HasFilter("([NgayTra] IS NULL)");

            entity.Property(e => e.GhiChu).HasMaxLength(500);

            entity.HasOne(d => d.MaBanSachNavigation).WithOne(p => p.NthChitietphieumuon)
                .HasForeignKey<NthChitietphieumuon>(d => d.MaBanSach)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_chitiet_bansach");

            entity.HasOne(d => d.MaPhieuMuonNavigation).WithMany(p => p.NthChitietphieumuons)
                .HasForeignKey(d => d.MaPhieuMuon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_chitiet_phieumuon");
        });

        modelBuilder.Entity<NthDocgium>(entity =>
        {
            entity.HasKey(e => e.MaDocGia).HasName("PK__nth_docg__F165F9454B70C5F5");

            entity.ToTable("nth_docgia");

            entity.HasIndex(e => e.HoTen, "IX_docgia_HoTen");

            entity.HasIndex(e => e.MaTheDocGia, "UQ__nth_docg__BA67A41FC036765E").IsUnique();

            entity.Property(e => e.DiaChi).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.MaTheDocGia).HasMaxLength(20);
            entity.Property(e => e.NgayDangKy).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.SoDienThoai).HasMaxLength(15);
            entity.Property(e => e.TrangThai).HasDefaultValue(true);
        });

        modelBuilder.Entity<NthNguoidung>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PK__nth_nguo__C539D762FD7B48C2");

            entity.ToTable("nth_nguoidung");

            entity.HasIndex(e => e.TenDangNhap, "UQ__nth_nguo__55F68FC077539233").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.MatKhauHash).HasMaxLength(255);
            entity.Property(e => e.TenDangNhap).HasMaxLength(50);
            entity.Property(e => e.TrangThai).HasDefaultValue(true);
            entity.Property(e => e.VaiTro).HasMaxLength(30);
        });

        modelBuilder.Entity<NthNhaxuatban>(entity =>
        {
            entity.HasKey(e => e.MaNhaXuatBan).HasName("PK__nth_nhax__1AED0BFA86C0EC29");

            entity.ToTable("nth_nhaxuatban");

            entity.Property(e => e.DiaChi).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.SoDienThoai).HasMaxLength(15);
            entity.Property(e => e.TenNhaXuatBan).HasMaxLength(150);
        });

        modelBuilder.Entity<NthPhieumuon>(entity =>
        {
            entity.HasKey(e => e.MaPhieuMuon).HasName("PK__nth_phie__C4C8222286656AFB");

            entity.ToTable("nth_phieumuon");

            entity.HasIndex(e => new { e.MaDocGia, e.NgayMuon }, "IX_phieumuon_MaDocGia_NgayMuon");

            entity.Property(e => e.GhiChu).HasMaxLength(500);
            entity.Property(e => e.NgayMuon).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(30)
                .HasDefaultValue("DangMuon");

            entity.HasOne(d => d.MaDocGiaNavigation).WithMany(p => p.NthPhieumuons)
                .HasForeignKey(d => d.MaDocGia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_phieumuon_docgia");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.NthPhieumuons)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_phieumuon_nguoidung");
        });

        modelBuilder.Entity<NthSach>(entity =>
        {
            entity.HasKey(e => e.MaSach).HasName("PK__nth_sach__B235742D78AB9662");

            entity.ToTable("nth_sach");

            entity.HasIndex(e => e.TieuDe, "IX_sach_TieuDe");

            entity.Property(e => e.AnhBia).HasMaxLength(500);
            entity.Property(e => e.MoTa).HasMaxLength(1000);
            entity.Property(e => e.TieuDe).HasMaxLength(200);

            entity.HasOne(d => d.MaNhaXuatBanNavigation).WithMany(p => p.NthSaches)
                .HasForeignKey(d => d.MaNhaXuatBan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_sach_nhaxuatban");

            entity.HasOne(d => d.MaTheLoaiNavigation).WithMany(p => p.NthSaches)
                .HasForeignKey(d => d.MaTheLoai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_sach_theloai");

            entity.HasMany(d => d.MaTacGia).WithMany(p => p.MaSaches)
                .UsingEntity<Dictionary<string, object>>(
                    "NthSachTacgium",
                    r => r.HasOne<NthTacgium>().WithMany()
                        .HasForeignKey("MaTacGia")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_sach_tacgia_tacgia"),
                    l => l.HasOne<NthSach>().WithMany()
                        .HasForeignKey("MaSach")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_sach_tacgia_sach"),
                    j =>
                    {
                        j.HasKey("MaSach", "MaTacGia").HasName("PK_sach_tacgia");
                        j.ToTable("nth_sach_tacgia");
                    });
        });

        modelBuilder.Entity<NthTacgium>(entity =>
        {
            entity.HasKey(e => e.MaTacGia).HasName("PK__nth_tacg__F24E6756457326AE");

            entity.ToTable("nth_tacgia");

            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.TieuSu).HasMaxLength(500);
        });

        modelBuilder.Entity<NthTheloai>(entity =>
        {
            entity.HasKey(e => e.MaTheLoai).HasName("PK__nth_thel__D73FF34A7FBCF014");

            entity.ToTable("nth_theloai");

            entity.HasIndex(e => e.TenTheLoai, "UQ__nth_thel__327F958F9C1A24EE").IsUnique();

            entity.Property(e => e.MoTa).HasMaxLength(255);
            entity.Property(e => e.TenTheLoai).HasMaxLength(100);
        });

        modelBuilder.Entity<NthTienphat>(entity =>
        {
            entity.HasKey(e => e.MaTienPhat).HasName("PK__nth_tien__3CB5ED385F4C8A33");

            entity.ToTable("nth_tienphat");

            entity.HasIndex(e => e.TrangThai, "IX_tienphat_TrangThai");

            entity.Property(e => e.LyDo).HasMaxLength(255);
            entity.Property(e => e.NgayLap).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.SoTien).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(30)
                .HasDefaultValue("ChuaThanhToan");

            entity.HasOne(d => d.MaChiTietNavigation).WithMany(p => p.NthTienphats)
                .HasForeignKey(d => d.MaChiTiet)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tienphat_chitiet");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
