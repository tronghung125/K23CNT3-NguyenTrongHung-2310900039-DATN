
import { useEffect, useState } from "react";

const taoDuLieuBanDau = (sach) => ({
  tieuDe: sach?.tieuDe ?? sach?.TieuDe ?? "",
  maTheLoai: String(sach?.maTheLoai ?? sach?.MaTheLoai ?? ""),
  maNhaXuatBan: String(
    sach?.maNhaXuatBan ?? sach?.MaNhaXuatBan ?? ""
  ),
  namXuatBan: String(
    sach?.namXuatBan ?? sach?.NamXuatBan ?? ""
  ),
  moTa: sach?.moTa ?? sach?.MoTa ?? "",
  anhBia: sach?.anhBia ?? sach?.AnhBia ?? "",
});

export default function NthSachForm({
  sach,
  dangLuu,
  onSave,
  onCancel,
}) {
  const [form, setForm] = useState(() => taoDuLieuBanDau(sach));
  const [loi, setLoi] = useState("");

  useEffect(() => {
    setForm(taoDuLieuBanDau(sach));
    setLoi("");
  }, [sach]);

  function capNhatTruong(event) {
    const { name, value } = event.target;

    setForm((prev) => ({
      ...prev,
      [name]: value,
    }));
  }

  async function xuLySubmit(event) {
    event.preventDefault();
    setLoi("");

    if (!form.tieuDe.trim()) {
      setLoi("Vui lòng nhập tiêu đề sách.");
      return;
    }

    const maTheLoai = Number(form.maTheLoai);
    const maNhaXuatBan = Number(form.maNhaXuatBan);

    if (!Number.isInteger(maTheLoai) || maTheLoai < 1) {
      setLoi("Mã thể loại phải là số nguyên lớn hơn 0.");
      return;
    }

    if (
      !Number.isInteger(maNhaXuatBan) ||
      maNhaXuatBan < 1
    ) {
      setLoi("Mã nhà xuất bản phải là số nguyên lớn hơn 0.");
      return;
    }

    const namXuatBan =
      form.namXuatBan.trim() === ""
        ? null
        : Number(form.namXuatBan);

    if (
      namXuatBan !== null &&
      (!Number.isInteger(namXuatBan) ||
        namXuatBan < 0 ||
        namXuatBan > 9999)
    ) {
      setLoi("Năm xuất bản phải nằm trong khoảng 0 đến 9999.");
      return;
    }

    const duLieuSach = {
      tieuDe: form.tieuDe.trim(),
      maTheLoai,
      maNhaXuatBan,
      namXuatBan,
      moTa: form.moTa.trim() || null,
      anhBia: form.anhBia.trim() || null,
    };

    try {
      await onSave(duLieuSach);
    } catch (error) {
      setLoi(error.message || "Không thể lưu thông tin sách.");
    }
  }

  return (
    <form onSubmit={xuLySubmit}>
      {loi && (
        <div className="alert alert-danger" role="alert">
          {loi}
        </div>
      )}

      <div className="mb-3">
        <label htmlFor="tieuDe" className="form-label">
          Tiêu đề sách <span className="text-danger">*</span>
        </label>
        <input
          id="tieuDe"
          name="tieuDe"
          type="text"
          className="form-control"
          maxLength={200}
          value={form.tieuDe}
          onChange={capNhatTruong}
          placeholder="Nhập tiêu đề sách"
          required
        />
      </div>

      <div className="row">
        <div className="col-md-6 mb-3">
          <label htmlFor="maTheLoai" className="form-label">
            Mã thể loại <span className="text-danger">*</span>
          </label>
          <input
            id="maTheLoai"
            name="maTheLoai"
            type="number"
            min="1"
            step="1"
            className="form-control"
            value={form.maTheLoai}
            onChange={capNhatTruong}
            placeholder="Ví dụ: 1"
            required
          />
        </div>

        <div className="col-md-6 mb-3">
          <label htmlFor="maNhaXuatBan" className="form-label">
            Mã nhà xuất bản <span className="text-danger">*</span>
          </label>
          <input
            id="maNhaXuatBan"
            name="maNhaXuatBan"
            type="number"
            min="1"
            step="1"
            className="form-control"
            value={form.maNhaXuatBan}
            onChange={capNhatTruong}
            placeholder="Ví dụ: 1"
            required
          />
        </div>
      </div>

      <div className="mb-3">
        <label htmlFor="namXuatBan" className="form-label">
          Năm xuất bản
        </label>
        <input
          id="namXuatBan"
          name="namXuatBan"
          type="number"
          min="0"
          max="9999"
          step="1"
          className="form-control"
          value={form.namXuatBan}
          onChange={capNhatTruong}
          placeholder="Ví dụ: 2025"
        />
      </div>

      <div className="mb-3">
        <label htmlFor="moTa" className="form-label">
          Mô tả
        </label>
        <textarea
          id="moTa"
          name="moTa"
          className="form-control"
          rows="3"
          value={form.moTa}
          onChange={capNhatTruong}
          placeholder="Nhập mô tả sách"
        />
      </div>

      <div className="mb-4">
        <label htmlFor="anhBia" className="form-label">
          Đường dẫn ảnh bìa
        </label>
        <input
          id="anhBia"
          name="anhBia"
          type="url"
          className="form-control"
          maxLength={500}
          value={form.anhBia}
          onChange={capNhatTruong}
          placeholder="https://example.com/anh-bia.jpg"
        />

        {form.anhBia.trim() && (
          <div className="book-cover-preview mt-3">
            <img
              src={form.anhBia}
              alt="Xem trước ảnh bìa"
              onError={(event) => {
                event.currentTarget.style.display = "none";
              }}
              onLoad={(event) => {
                event.currentTarget.style.display = "block";
              }}
            />
          </div>
        )}
      </div>

      <div className="d-flex justify-content-end gap-2">
        <button
          type="button"
          className="btn btn-light border"
          onClick={onCancel}
          disabled={dangLuu}
        >
          Hủy
        </button>

        <button
          type="submit"
          className="btn btn-primary"
          disabled={dangLuu}
        >
          {dangLuu ? (
            <>
              <span className="spinner-border spinner-border-sm me-2" />
              Đang lưu...
            </>
          ) : (
            <>
              <i className="bi bi-check2-circle me-2" />
              {sach ? "Lưu thay đổi" : "Thêm sách"}
            </>
          )}
        </button>
      </div>
    </form>
  );
}
