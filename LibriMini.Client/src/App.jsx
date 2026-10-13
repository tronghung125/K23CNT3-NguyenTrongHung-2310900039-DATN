
import { useCallback, useEffect, useState } from "react";
import { nthApi } from "./nthApi";
import NthSachList from "./nthSachList";
import NthSachForm from "./nthSachForm";
import "./App.css";

export default function App() {
  const [danhSach, setDanhSach] = useState([]);
  const [dangTai, setDangTai] = useState(true);
  const [dangLuu, setDangLuu] = useState(false);
  const [loi, setLoi] = useState("");
  const [thongBao, setThongBao] = useState("");
  const [tuKhoa, setTuKhoa] = useState("");
  const [maTheLoai, setMaTheLoai] = useState("");
  const [sachDangSua, setSachDangSua] = useState(null);
  const [hienForm, setHienForm] = useState(false);

  const taiDanhSach = useCallback(async (tuKhoaMoi, maTheLoaiMoi) => {
    setDangTai(true);
    setLoi("");

    try {
      const duLieu = await nthApi.layDanhSachSach(
        tuKhoaMoi,
        maTheLoaiMoi
      );

      setDanhSach(Array.isArray(duLieu) ? duLieu : []);
    } catch (error) {
      setLoi(
        `${error.message}. Hãy kiểm tra backend và kết nối API.`
      );
      setDanhSach([]);
    } finally {
      setDangTai(false);
    }
  }, []);

  useEffect(() => {
    taiDanhSach("", "");
  }, [taiDanhSach]);

  function hienThongBao(noiDung) {
    setThongBao(noiDung);
    window.setTimeout(() => setThongBao(""), 3500);
  }

  function timKiem(tuKhoaMoi = tuKhoa, maTheLoaiMoi = maTheLoai) {
    taiDanhSach(tuKhoaMoi, maTheLoaiMoi);
  }

  function moFormThem() {
    setSachDangSua(null);
    setHienForm(true);
  }

  function moFormSua(sach) {
    setSachDangSua(sach);
    setHienForm(true);
  }

  async function luuSach(duLieuSach) {
    setDangLuu(true);

    try {
      if (sachDangSua) {
        await nthApi.capNhatSach(sachDangSua.maSach, duLieuSach);
        hienThongBao("Cập nhật sách thành công.");
      } else {
        await nthApi.themSach(duLieuSach);
        hienThongBao("Thêm sách thành công.");
      }

      setHienForm(false);
      setSachDangSua(null);
      await taiDanhSach(tuKhoa, maTheLoai);
    } catch (error) {
      throw error;
    } finally {
      setDangLuu(false);
    }
  }

  async function xoaSach(sach) {
    setLoi("");

    try {
      await nthApi.xoaSach(sach.maSach);
      hienThongBao("Xóa sách thành công.");
      await taiDanhSach(tuKhoa, maTheLoai);
    } catch (error) {
      setLoi(error.message || "Không thể xóa sách.");
    }
  }

  return (
    <div className="app-shell">
      <aside className="sidebar">
        <a className="brand" href="#" aria-label="LibriMini trang chủ">
          <span className="brand-icon">
            <i className="bi bi-book-half" />
          </span>
          <span>
            <span className="brand-name">LibriMini</span>
            <span className="brand-caption">LIBRARY MANAGEMENT</span>
          </span>
        </a>

        <div className="sidebar-label">TỔNG QUAN</div>

        <a className="sidebar-link active" href="#sach">
          <i className="bi bi-grid-1x2-fill" />
          <span>Quản lý sách</span>
        </a>

        <div className="sidebar-label mt-4">HỆ THỐNG</div>

        <div className="sidebar-link sidebar-link-muted">
          <i className="bi bi-people" />
          <span>Độc giả</span>
          <span className="coming-soon">Sau</span>
        </div>

        <div className="sidebar-link sidebar-link-muted">
          <i className="bi bi-journal-arrow-up" />
          <span>Mượn / trả sách</span>
          <span className="coming-soon">Sau</span>
        </div>

        <div className="sidebar-link sidebar-link-muted">
          <i className="bi bi-tags" />
          <span>Thể loại</span>
          <span className="coming-soon">Sau</span>
        </div>

        <div className="sidebar-bottom">
          <div className="sidebar-bottom-icon">
            <i className="bi bi-lightbulb" />
          </div>
          <div>
            <div className="small fw-semibold">LibriMini</div>
            <div className="small text-secondary">
              Hệ thống quản lý thư viện
            </div>
          </div>
        </div>
      </aside>

      <main className="main-content" id="sach">
        <header className="topbar">
          <div>
            <div className="breadcrumb-text">
              Trang chủ <i className="bi bi-chevron-right" /> Quản lý sách
            </div>
            <h1 className="page-title">Quản lý sách</h1>
          </div>

          <div className="topbar-status">
            <span className="status-dot" />
            <span>Thư viện trực tuyến</span>
          </div>
        </header>

        <section className="page-body">
          <div className="welcome-row">
            <div>
              <h2>Kho sách thư viện</h2>
              <p className="text-secondary mb-0">
                Theo dõi và quản lý danh mục sách của bạn.
              </p>
            </div>

            <button
              type="button"
              className="btn btn-primary add-book-button"
              onClick={moFormThem}
            >
              <i className="bi bi-plus-lg me-2" />
              Thêm sách mới
            </button>
          </div>

          {thongBao && (
            <div
              className="alert alert-success alert-dismissible fade show"
              role="status"
            >
              <i className="bi bi-check-circle-fill me-2" />
              {thongBao}
              <button
                type="button"
                className="btn-close"
                onClick={() => setThongBao("")}
                aria-label="Đóng"
              />
            </div>
          )}

          <div className="row g-3 mb-4">
            <div className="col-sm-6 col-xl-4">
              <div className="stat-card">
                <div className="stat-icon stat-icon-blue">
                  <i className="bi bi-book" />
                </div>
                <div>
                  <div className="stat-label">Sách trong kết quả</div>
                  <div className="stat-value">
                    {dangTai ? "..." : danhSach.length}
                  </div>
                </div>
              </div>
            </div>

            <div className="col-sm-6 col-xl-4">
              <div className="stat-card">
                <div className="stat-icon stat-icon-green">
                  <i className="bi bi-database-check" />
                </div>
                <div>
                  <div className="stat-label">Kết nối dữ liệu</div>
                  <div className="stat-value stat-value-small">
                    {loi ? "Cần kiểm tra" : dangTai ? "Đang tải..." : "Đã truy vấn"}
                  </div>
                </div>
              </div>
            </div>

            <div className="col-sm-6 col-xl-4">
              <div className="stat-card">
                <div className="stat-icon stat-icon-purple">
                  <i className="bi bi-funnel" />
                </div>
                <div>
                  <div className="stat-label">Bộ lọc</div>
                  <div className="stat-value stat-value-small">
                    {maTheLoai ? "Theo thể loại" : "Tất cả thể loại"}
                  </div>
                </div>
              </div>
            </div>
          </div>

          <NthSachList
            danhSach={danhSach}
            dangTai={dangTai}
            loi={loi}
            tuKhoa={tuKhoa}
            onTuKhoaChange={setTuKhoa}
            maTheLoai={maTheLoai}
            onTheLoaiChange={setMaTheLoai}
            onTimKiem={timKiem}
            onEdit={moFormSua}
            onDelete={xoaSach}
            onRetry={() => timKiem()}
          />

          <footer className="page-footer">
            <span>© 2026 LibriMini</span>
            <span>Hệ thống quản lý thư viện</span>
          </footer>
        </section>
      </main>

      {hienForm && (
        <div className="modal-backdrop-custom">
          <div
            className="book-form-dialog"
            role="dialog"
            aria-modal="true"
            aria-labelledby="bookFormTitle"
          >
            <div className="modal-heading">
              <div>
                <h4 id="bookFormTitle">
                  {sachDangSua ? "Cập nhật sách" : "Thêm sách mới"}
                </h4>
                <p className="text-secondary small mb-0">
                  Nhập thông tin sách vào hệ thống thư viện.
                </p>
              </div>

              <button
                type="button"
                className="btn-close"
                onClick={() => setHienForm(false)}
                disabled={dangLuu}
                aria-label="Đóng"
              />
            </div>

            <div className="modal-form-body">
              <NthSachForm
                key={sachDangSua?.maSach ?? "new"}
                sach={sachDangSua}
                dangLuu={dangLuu}
                onSave={luuSach}
                onCancel={() => setHienForm(false)}
              />
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
