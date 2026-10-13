
const API_URL = "http://localhost:5295/api/Sach";

async function xuLyPhanHoi(response) {
  if (!response.ok) {
    let thongBao = `Lỗi HTTP ${response.status}`;

    try {
      const duLieu = await response.json();
      thongBao =
        duLieu.message ||
        duLieu.title ||
        (typeof duLieu === "string" ? duLieu : thongBao);
    } catch {
      // Giữ thông báo HTTP mặc định nếu response không phải JSON.
    }

    throw new Error(thongBao);
  }

  if (response.status === 204) {
    return null;
  }

  return response.json();
}

export const nthApi = {
  // Lấy danh sách sách, có thể tìm kiếm và lọc thể loại.
  async layDanhSachSach(tuKhoa = "", maTheLoai = "") {
    const params = new URLSearchParams();

    if (tuKhoa.trim()) {
      params.set("tuKhoa", tuKhoa.trim());
    }

    if (maTheLoai !== "") {
      params.set("maTheLoai", maTheLoai);
    }

    const query = params.toString();
    const url = query ? `${API_URL}?${query}` : API_URL;

    const response = await fetch(url);
    return xuLyPhanHoi(response);
  },

  // Lấy chi tiết một cuốn sách.
  async laySachTheoMa(maSach) {
    const response = await fetch(`${API_URL}/${maSach}`);
    return xuLyPhanHoi(response);
  },

  // Thêm sách.
  async themSach(sach) {
    const response = await fetch(API_URL, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(sach),
    });

    return xuLyPhanHoi(response);
  },

  // Cập nhật sách.
  async capNhatSach(maSach, sach) {
    const response = await fetch(`${API_URL}/${maSach}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(sach),
    });

    return xuLyPhanHoi(response);
  },

  // Xóa sách.
  async xoaSach(maSach) {
    const response = await fetch(`${API_URL}/${maSach}`, {
      method: "DELETE",
    });

    return xuLyPhanHoi(response);
  },
};
