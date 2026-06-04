# Luồng hoạt động của chương trình

## 1. Mô hình MVC

Ứng dụng được xây dựng theo mô hình MVC:

```text
Người dùng → Controller → Model → View → Trình duyệt
```

* **Model (Book):** Chứa dữ liệu sách và các quy tắc validation.
* **Controller (BookController):** Xử lý request từ người dùng.
* **View:** Hiển thị giao diện và dữ liệu.

---

## 2. Xem danh sách sách

### URL

```text
/Book
```

### Luồng xử lý

1. Người dùng truy cập trang danh sách sách.
2. Action `Index()` được gọi.
3. Danh sách sách được truyền sang View.
4. View hiển thị danh sách sách.

---

## 3. Xem chi tiết sách

### URL

```text
/Book/Detail/{id}
```

Ví dụ:

```text
/Book/Detail/1
```

### Luồng xử lý

1. Controller nhận `id`.
2. Tìm sách tương ứng trong danh sách.
3. Truyền đối tượng Book sang View.
4. View hiển thị thông tin chi tiết sách.

---

## 4. Thêm sách

### URL

```text
/Book/Create
```

### Luồng xử lý

1. Người dùng nhập thông tin sách.
2. Dữ liệu được gửi đến Action `Create()` bằng phương thức POST.
3. ASP.NET Core tự động ánh xạ dữ liệu vào đối tượng `Book`.
4. Hệ thống kiểm tra validation:

   * Tên sách không được để trống.
   * Giá sách phải lớn hơn 0.
5. Nếu dữ liệu không hợp lệ, hiển thị lỗi.
6. Nếu hợp lệ, hiển thị thông báo:

```text
Thêm sách thành công
```

---

## Kết luận

Chương trình sử dụng mô hình MVC để quản lý sách. Controller xử lý yêu cầu, Model quản lý dữ liệu và validation, còn View hiển thị kết quả cho người dùng.
