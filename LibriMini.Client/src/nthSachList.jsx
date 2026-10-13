
import { useMemo, useState } from "react";

export default function NthSachList({
  danhSach,
  dangTai,
  loi,
  tuKhoa,
  onTuKhoaChange,
  maTheLoai,
  onTheLoaiChange,
  onTimKiem,
  onEdit,
  onDelete,
  onRetry,
}) {
  const [sachCanXoa, setSachCanXoa] = useState(null);

  const danhSachTheLoai = useMemo(() => {
    const ketQua = new Map();

    danhSach.forEach((sach) => {
      if (sach.maTheLoai != null) {
        ketQua.set(
          String(sach.maTheLoai),
          sach.tenTheLoai || `Thể loại ${sach.maTheLoai}`
        );
      }
    });

    return Array.from(ketQua.entries());
  }, [danhSach]);

  async function xacNhanXoa() {
    if (!sachCanXoa) return;

    const sach = sachCanXoa;
    setSachCanXoa(null);
    await onDelete(sach);
  }

  return (
    <>
      <div className="card border-0 shadow-sm">
        <div className="card-body p-3 p-lg-4">
          <form
            className="row g-3 align-items-end mb-4"
            onSubmit={(event) => {
              event.preventDefault();
              onTimKiem();
            }}
          >
            <div className="col-lg-6">
              <label htmlFor="tuKhoa" className="form-label fw-semibold">
                Tìm kiếm sách
              </label>
              <div className="input-group">
                <span className="input-group-text bg-white">
                  <i className="bi bi-search" />
                </span>
                <input
                  id="tuKhoa"
                  className="form-control"
                  value={tuKhoa}
                  onChange={(event) => onTuKhoaChange(event.target.value)}
                  placeholder="Nhập tên sách cần tìm..."
                />
              </div>
            </div>

            <div className="col-lg-3">
              <label htmlFor="locTheLoai" className="form-label fw-semibold">
                Thể loại
              </label>
              <select
                id="locTheLoai"
                className="form-select"
                value={maTheLoai}
                onChange={(event) => onTheLoaiChange(event.target.value)}
              >
                <option value="">Tất cả thể loại</option>
                {danhSachTheLoai.map(([ma, ten]) => (
                  <option key={ma} value={ma}>
                    {ten}
                  </option>
                ))}
              </select>
            </div>

            <div className="col-lg-3 d-flex gap-2">
              <button type="submit" className="btn btn-primary flex-grow-1">
                <i className="bi bi-search me-1" />
                Tìm kiếm
              </button>
              <button
                type="button"
                className="btn btn-outline-secondary"
                title="Xóa bộ lọc"
                onClick={() => {
                  onTuKhoaChange("");
                  onTheLoaiChange("");
                  onTimKiem("", "");
                }}
              >
                <i className="bi bi-arrow-counterclockwise" />
              </button>
            </div>
          </form>

          {loi && (
            <div className="alert alert-danger d-flex justify-content-between align-items-center">
              <span>{loi}</span>
              <button
                type="button"
                className="btn btn-sm btn-outline-danger ms-3"
                onClick={onRetry}
              >
                Thử lại
              </button>
            </div>
          )}

          <div className="d-flex justify-content-between align-items-center mb-3">
            <div>
              <h5 className="mb-1">Danh sách sách</h5>
              <span className="text-secondary small">
                {dangTai
                  ? "Đang tải dữ liệu..."
                  : `${danhSach.length} sách trong kết quả`}
              </span>
            </div>

            <button
              type="button"
              className="btn btn-outline-primary btn-sm"
              onClick={onRetry}
              disabled={dangTai}
            >
              <i className="bi bi-arrow-clockwise me-1" />
              Làm mới
            </button>
          </div>

          <div className="table-responsive">
            <table className="table align-middle book-table">
              <thead>
                <tr>
                  <th className="text-center">Mã</th>
                  <th>Sách</th>
                  <th>Thể loại</th>
                  <th>Nhà xuất bản</th>
                  <th>Năm XB</th>
                  <th className="text-center">Thao tác</th>
                </tr>
              </thead>

              <tbody>
                {dangTai ? (
                  <tr>
                    <td colSpan="6" className="text-center py-5">
                      <div
                        className="spinner-border text-primary mb-2"
                        role="status"
                      />
                      <div>Đang tải danh sách sách...</div>
                    </td>
                  </tr>
                ) : danhSach.length === 0 ? (
                  <tr>
                    <td colSpan="6" className="text-center py-5">
                      <i className="bi bi-inbox empty-icon d-block mb-2" />
                      <div className="fw-semibold">
                        Không tìm thấy sách
                      </div>
                      <div className="text-secondary small">
                        Hãy thử từ khóa khác hoặc thêm sách mới.
                      </div>
                    </td>
                  </tr>
                ) : (
                  danhSach.map((sach) => (
                    <tr key={sach.maSach}>
                      <td className="text-center text-secondary">
                        #{sach.maSach}
                      </td>

                      <td>
                        <div className="d-flex align-items-center gap-3 book-cell">
                          {sach.anhBia ? (
                            <img
                              src={sach.anhBia}
                              alt={sach.tieuDe}
                              className="book-cover"
                              loading="lazy"
                              onError={(event) => {
                                event.currentTarget.style.display = "none";
                              }}
                            />
                          ) : (
                            <div className="book-cover-placeholder">
                              <i className="bi bi-book" />
                            </div>
                          )}

                          <div className="book-title-cell">
                            <div className="fw-semibold">
                              {sach.tieuDe}
                            </div>
                            {sach.moTa && (
                              <div
                                className="small text-secondary book-description"
                                title={sach.moTa}
                              >
                                {sach.moTa}
                              </div>
                            )}
                          </div>
                        </div>
                      </td>

                      <td>
                        <span className="badge text-bg-light border">
                          {sach.tenTheLoai || `Mã ${sach.maTheLoai}`}
                        </span>
                      </td>

                      <td>
                        {sach.tenNhaXuatBan ||
                          `Mã ${sach.maNhaXuatBan}`}
                      </td>

                      <td>{sach.namXuatBan ?? "—"}</td>

                      <td>
                        <div className="d-flex justify-content-center gap-2">
                          <button
                            type="button"
                            className="btn btn-sm btn-outline-primary"
                            title="Sửa sách"
                            onClick={() => onEdit(sach)}
                          >
                            <i className="bi bi-pencil-square" />
                          </button>

                          <button
                            type="button"
                            className="btn btn-sm btn-outline-danger"
                            title="Xóa sách"
                            onClick={() => setSachCanXoa(sach)}
                          >
                            <i className="bi bi-trash" />
                          </button>
                        </div>
                      </td>
                    </tr>
                  ))
                )}
              </tbody>
            </table>
          </div>
        </div>
      </div>

      {sachCanXoa && (
        <div className="modal-backdrop-custom">
          <div
            className="confirm-dialog"
            role="dialog"
            aria-modal="true"
            aria-labelledby="confirmDeleteTitle"
          >
            <div className="confirm-icon">
              <i className="bi bi-exclamation-triangle" />
            </div>

            <h5 id="confirmDeleteTitle">Xác nhận xóa sách</h5>
            <p className="text-secondary">
              Bạn có chắc muốn xóa sách{" "}
              <strong>{sachCanXoa.tieuDe}</strong>? Thao tác này không
              thể hoàn tác.
            </p>

            <div className="d-flex justify-content-end gap-2">
              <button
                className="btn btn-light border"
                onClick={() => setSachCanXoa(null)}
              >
                Hủy
              </button>
              <button
                className="btn btn-danger"
                onClick={xacNhanXoa}
              >
                Xác nhận xóa
              </button>
            </div>
          </div>
        </div>
      )}
    </>
  );
}
