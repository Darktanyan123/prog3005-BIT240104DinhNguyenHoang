Hướng dẫn kết nối API Quản lý sản phẩm (Dành cho Front-end)

Hệ thống API phục vụ cho cả Web và Mobile sử dụng định dạng dữ liệu JSON.

Thông tin cấu hình chung
- **Base URL:** `https://localhost:XXXX` *(Thay XXXX bằng port dự án ASP.NET chạy thực tế)*
- **Headers bắt buộc:** `Content-Type: application/json`

---

 Các API Endpoints Chi Tiết

1. Thêm mới sản phẩm (POST)
- **Endpoint:** `/api-mobile/products`
- **Method:** `POST`
- **Request Body JSON:**

{
  "name": "iPhone 15 Pro",
  "price": 999.99
}

Xử lý lỗi Validate dữ liệu (HTTP Status: 400 Bad Request)
Khi Front-end gửi dữ liệu sai quy định (Ví dụ: name dưới 3 ký tự hoặc price nhỏ hơn hoặc bằng 0),
 API sẽ trả về mảng danh sách lỗi cụ thể:

Request Body lỗi:

JSON
{
  "name": "Ab",
  "price": -50
}
Response trả về từ API:

JSON
{
  "errors": [
    "Tên sản phẩm phải có tối thiểu 3 ký tự.",
    "Giá sản phẩm phải lớn hơn 0."
  ]
}
