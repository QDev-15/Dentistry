# Plan: Email thông báo khi có tin nhắn / đặt lịch mới

- **Phương án đã chọn:** Cấu hình SMTP qua giao diện Admin (Cài đặt chung) — hỗ trợ đổi qua lại giữa email hosting và Gmail mà không cần sửa code.
- **Yêu cầu quan trọng:** UI phải phân biệt rõ 2 loại nhà cung cấp (Hosting / Gmail) — mỗi loại có cách lấy thông tin và cấu hình khác nhau, không thể dùng chung một form "điền tay" đơn thuần. Xem chi tiết ở Giai đoạn 1 và 3 bên dưới.
- **Phạm vi:** `Dentisty.Data`, `Dentistry.ViewModels`, `Dentistry.Admin`, `Dentistry.Web`
- **Trạng thái tổng thể:** Chưa bắt đầu
- **Tổng công sức ước tính:** ~15h (tăng từ 13h do bổ sung UI phân biệt Hosting/Gmail ở Giai đoạn 1 &amp; 3)
- **Tài liệu tham khảo:** đặc tả & báo giá đầy đủ tại artifact "Email Thông Báo Liên Hệ"

Cách dùng file này: đánh dấu `[x]` khi hoàn thành từng mục, cập nhật "Trạng thái tổng thể" và ghi log ở cuối file mỗi lần có tiến triển.

---

## 0. Đầu vào cần từ khách hàng (chặn trước khi triển khai)

- [ ] Quyết định phương án gửi: email hosting hay Gmail
- [ ] Danh sách email nhận thông báo
- [ ] Tên hiển thị người gửi (vd "Phòng khám Nha khoa ABC")
- [ ] Quyền truy cập server/hosting production để deploy
- [ ] Nếu chọn hosting: tài khoản email hosting (hoặc quyền cPanel/Plesk) + quyền chỉnh DNS (SPF/DKIM)
- [ ] Nếu chọn Gmail: tài khoản Gmail + đã bật 2-Step Verification + App Password 16 ký tự

---

## Giai đoạn 1 — Cơ sở dữ liệu & tầng dữ liệu (~2.5h)

- [ ] Thêm 8 field vào `Dentisty.Data/GeneratorDB/Entities/AppSetting.cs`:
  `SmtpProvider` (enum/nvarchar: `Hosting` hoặc `Gmail` — quyết định UI hiển thị/điền sẵn thế nào), `SmtpHost`, `SmtpPort`, `SmtpUseSsl`, `SmtpUsername`, `SmtpPasswordEncrypted`, `SmtpSenderName`, `NotificationEmails`
- [ ] Tạo migration EF Core: `AddSmtpSettingsToAppSetting`
- [ ] Cập nhật `Dentisty.Data/Repositories/AppSettingRepository.cs` — map field mới khi đọc/ghi, mã hoá mật khẩu trước khi lưu
- [ ] Cập nhật `Dentistry.ViewModels/Catalog/AppSettings/AppSettingVm.cs` — thêm field tương ứng, validate theo `SmtpProvider` (vd bắt buộc `SmtpHost`/`SmtpPort` khi chọn Hosting)

## Giai đoạn 2 — Dịch vụ gửi email (~3h)

- [ ] Thêm NuGet package `MailKit` vào `Dentisty.Data.csproj`
- [ ] Tạo `Dentisty.Data/Services/Email/IEmailService.cs`
- [ ] Tạo `Dentisty.Data/Services/Email/EmailService.cs` (build & gửi mail qua SMTP bằng MailKit)
- [ ] Tạo `Dentisty.Data/Services/Email/SmtpOptions.cs` (đọc cấu hình từ AppSetting)
- [ ] Cơ chế mã hoá/giải mã mật khẩu SMTP (dùng `IDataProtectionProvider` có sẵn của ASP.NET Core)

## Giai đoạn 3 — Giao diện Cài đặt trong Admin (~5.5h)

Khối "Cấu hình Email thông báo" trong `Dentistry.Admin/Views/AppSetting/Partial/_get_settings.cshtml`, thêm vào:

- [ ] **Bước chọn loại nhà cung cấp** — 2 lựa chọn dạng radio/toggle rõ ràng, không gộp chung:
  - "Email theo tên miền (Hosting)"
  - "Gmail"
- [ ] **Nhánh Hosting** (hiện khi chọn Hosting): các ô nhập tay `SMTP Host`, `Port`, `Bật SSL/TLS`, `Tài khoản`, `Mật khẩu` — để trống, admin tự điền theo thông tin nhà cung cấp hosting cấp. Kèm dòng ghi chú: *"Lấy thông tin này trong cPanel/Plesk của hosting → mục Email Accounts → Connect Devices."*
- [ ] **Nhánh Gmail** (hiện khi chọn Gmail): `Host` = `smtp.gmail.com`, `Port` = `587`, `SSL` = bật — **tự động điền sẵn** (vẫn cho sửa nếu cần), chỉ yêu cầu admin nhập `Tài khoản Gmail` và `App Password`. Kèm dòng ghi chú + link: *"Bật Xác minh 2 bước rồi tạo App Password tại myaccount.google.com/apppasswords — dán mã 16 ký tự vào đây, không dùng mật khẩu Gmail thường."*
- [ ] Ô `Tên hiển thị người gửi` và `Email nhận thông báo` (dùng chung cho cả 2 nhánh)
- [ ] JS xử lý: ẩn/hiện đúng nhánh theo lựa chọn, tự động điền giá trị mặc định khi chuyển sang Gmail, không mất dữ liệu đã nhập khi chuyển qua lại
- [ ] Nút **"Gửi thử"** gọi action test, hiển thị lỗi cụ thể theo ngữ cảnh (vd Gmail báo lỗi xác thực → gợi ý kiểm tra đã dùng App Password chưa; Hosting báo lỗi kết nối → gợi ý kiểm tra lại Host/Port hoặc port bị hosting chặn)
- [ ] Cập nhật `Dentistry.Admin/Controllers/Components/AppSettingsViewComponent.cs` — truyền dữ liệu ra view theo đúng `SmtpProvider`, ẩn mật khẩu thật (chỉ hiện placeholder)
- [ ] Thêm action `TestSmtp(...)` trong `Dentistry.Admin/Controllers/AppSettingController.cs` — nhận diện `SmtpProvider` để trả thông báo lỗi phù hợp
- [ ] Đăng ký `IEmailService` vào DI trong `Dentistry.Admin/Program.cs`

## Giai đoạn 4 — Tích hợp vào luồng liên hệ / đặt lịch (~2h)

- [ ] Inject `IEmailService` vào `Dentistry.Web/Controllers/ContactController.cs`
- [ ] Gọi gửi email sau khi `_contactRepository.Create(model)` thành công trong action `AddMessage`
- [ ] Gọi gửi email sau khi `_contactRepository.Create(model)` thành công trong action `Book`
- [ ] Gửi bất đồng bộ / không chặn phản hồi cho khách; log lỗi nếu gửi thất bại, không làm hỏng việc lưu tin nhắn
- [ ] Đăng ký `IEmailService` vào DI trong `Dentistry.Web/Program.cs`

## Giai đoạn 5 — Kiểm thử & triển khai (~2h)

- [ ] Test nút "Gửi thử" trong Admin với cấu hình thật
- [ ] Test luồng `AddMessage` (form liên hệ) đầu-cuối
- [ ] Test luồng `Book` (đặt lịch) đầu-cuối
- [ ] Test khi SMTP sai / mất mạng — xác nhận tin nhắn vẫn lưu, không crash
- [ ] Cấu hình SPF/DKIM cho tên miền (nếu áp dụng)
- [ ] Deploy lên production, kiểm tra port outbound (587/465) không bị chặn
- [ ] Bàn giao & hướng dẫn khách hàng sử dụng màn Cài đặt

---

## Rủi ro & lưu ý

- Mật khẩu SMTP phải mã hoá khi lưu DB, không lưu dạng thô.
- Cần SPF/DKIM để giảm khả năng mail rơi vào Spam.
- Gmail cá nhân giới hạn ~500 mail/ngày (đủ dùng), cần App Password thay vì mật khẩu thường.
- Một số hosting giá rẻ có thể chặn outbound port SMTP mặc định — cần kiểm tra trước khi deploy.

## Hạng mục phát sinh thêm (ngoài phạm vi plan này, làm sau nếu khách đồng ý)

- [ ] Theo dõi trạng thái gửi mail + tự động thử lại khi lỗi (thêm cột `IsEmailSent` vào `Contact`)
- [ ] Gửi kèm thông báo qua Zalo OA / Telegram

---

## Log cập nhật

- 2026-09-11: Tạo file plan, chốt phương án 2 (cấu hình qua UI Admin).
- 2026-09-11: Cập nhật plan — bổ sung yêu cầu UI phải phân biệt rõ 2 nhánh cấu hình Hosting/Gmail (field `SmtpProvider`, form 2 nhánh, JS điền sẵn giá trị Gmail, thông báo lỗi test theo ngữ cảnh). Tổng công sức tăng 13h → 15h.
- 2026-09-11: Khách hàng đồng ý mức tăng công sức. Đã cập nhật artifact báo giá khớp 15h — tổng **5.300.000đ** trọn gói.
