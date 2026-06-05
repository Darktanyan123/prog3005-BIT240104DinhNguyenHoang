1. Middleware trong ASP.NET Core dùng để làm gì?

Middleware dùng để xử lý request và response trong pipeline, ví dụ: ghi log, xác thực, phân quyền, xử lý lỗi,...

2. Middleware khác Controller ở điểm nào?

Middleware: xử lý request trước hoặc sau khi vào Controller.
Controller: xử lý nghiệp vụ và trả về View hoặc dữ liệu cho người dùng.

3. Dòng lệnh sau có ý nghĩa gì?

await _next(context);

Chuyển request sang middleware tiếp theo hoặc Controller để tiếp tục xử lý.

4. Vì sao khi middleware trả về return; thì request không đi tiếp vào Controller?
Vì return; kết thúc việc thực thi middleware hiện tại, nên request không được chuyển tiếp xuống pipeline.

5. Nếu đặt middleware sau app.MapControllerRoute(...) thì có thể xảy ra vấn đề gì?
Middleware có thể không được thực thi đối với các request đã được Controller xử lý, dẫn đến không ghi log hoặc không chặn được request.

6. Nếu cần sử dụng thêm middleware khác thì viết tiếp thế nào?

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<AnotherMiddleware>();

Các middleware sẽ được thực thi theo thứ tự khai báo.
