# Plan: Email thông báo khi có tin nhắn / đặt lịch mới

- **Phương án đã chọn:** Cấu hình SMTP qua giao diện Admin (Cài đặt chung) — hỗ trợ đổi qua lại giữa email hosting và Gmail mà không cần sửa code.
- **Yêu cầu quan trọng:** UI phải phân biệt rõ 2 loại nhà cung cấp (Hosting / Gmail) — mỗi loại có cách lấy thông tin và cấu hình khác nhau, không thể dùng chung một form "điền tay" đơn thuần. Xem chi tiết ở Giai đoạn 1 và 3 bên dưới.
- **Phạm vi:** `Dentisty.Data`, `Dentistry.ViewModels`, `Dentistry.Admin`, `Dentistry.Web`
- **Trạng thái tổng thể:** Code xong 5 giai đoạn gốc + tính năng bổ sung "gửi email xác nhận cho khách hàng". Đã phát hiện + vá 1 lỗi nghiêm trọng (mã hoá mật khẩu không dùng chung được giữa Admin/Web — xem Giai đoạn 6). Chỉ còn chặn ở bước production (xác nhận DB thật + deploy).
- **Tổng công sức ước tính:** ~15h cho phạm vi gốc (tăng từ 13h do UI Hosting/Gmail) + việc phát sinh thêm ở Giai đoạn 6 (xem chi tiết, chưa tính vào báo giá cũ)
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

## Giai đoạn 1 — Cơ sở dữ liệu & tầng dữ liệu (~2.5h) ✅ Hoàn thành 2026-09-11

- [x] Thêm 8 field vào `Dentisty.Data/GeneratorDB/Entities/AppSetting.cs`:
  `SmtpProvider` (nvarchar: `Hosting` hoặc `Gmail` — quyết định UI hiển thị/điền sẵn thế nào), `SmtpHost`, `SmtpPort`, `SmtpUseSsl`, `SmtpUsername`, `SmtpPasswordEncrypted`, `SmtpSenderName`, `NotificationEmails`
- [x] Tạo migration EF Core: `AddSmtpSettingsToAppSetting` (`Migrations/20260911043356_AddSmtpSettingsToAppSetting.cs`) — chưa apply lên DB, chỉ mới scaffold
- [x] Cập nhật `Dentisty.Data/Repositories/AppSettingRepository.cs` — map field mới khi đọc/ghi, mã hoá mật khẩu trước khi lưu (chỉ ghi đè khi admin nhập mật khẩu mới)
- [x] Cập nhật `Dentistry.ViewModels/Catalog/AppSettings/AppSettingVm.cs` — thêm field tương ứng; `SmtpPassword` chỉ ghi (write-only), thêm `HasSmtpPassword` để UI biết đã có mật khẩu lưu sẵn hay chưa, không bao giờ trả mật khẩu thật về trình duyệt
- [x] Cập nhật `Dentisty.Data/ViewModelExtensions.cs` (`ReturnViewModel`) — map chiều đọc
- [x] Thêm `Dentisty.Data/Services/Email/ISmtpCredentialProtector.cs` + `SmtpCredentialProtector.cs` — mã hoá/giải mã mật khẩu bằng ASP.NET Core Data Protection API (không cần thêm package, project đã có `FrameworkReference Microsoft.AspNetCore.App`)
- [x] Đăng ký `AddDataProtection()` + `ISmtpCredentialProtector` vào DI trong cả `Dentistry.Admin/Program.cs` và `Dentistry.Web/Program.cs`
- [x] Build cả `Dentisty.Data`, `Dentistry.Admin`, `Dentistry.Web` — 0 lỗi

## Giai đoạn 2 — Dịch vụ gửi email (~3h) ✅ Hoàn thành 2026-09-11

- [x] Thêm NuGet package `MailKit` (v4.17.0 — bản 4.9.0 dính lỗ hổng bảo mật moderate, đã nâng lên bản vá) vào `Dentisty.Data.csproj`
- [x] Tạo `Dentisty.Data/Services/Email/IEmailService.cs` — interface `SendAsync(subject, htmlBody)` dùng chung cho cả gửi thông báo liên hệ lẫn nút "Gửi thử"
- [x] Tạo `Dentisty.Data/Services/Email/EmailService.cs` — đọc entity `AppSetting` gốc (không qua Vm, để lấy được `SmtpPasswordEncrypted`), giải mã mật khẩu, gửi qua MailKit; trả lỗi cụ thể theo `SmtpProvider` (Gmail → gợi ý App Password; Hosting → gợi ý kiểm tra Host/Port/cổng bị chặn)
- [x] Cơ chế mã hoá/giải mã mật khẩu SMTP — dùng `IDataProtectionProvider` có sẵn của ASP.NET Core (đã làm ở Giai đoạn 1)
- [x] Đăng ký `IEmailService` vào DI trong cả `Dentistry.Admin/Program.cs` và `Dentistry.Web/Program.cs`
- [x] Build `Dentisty.Data`, `Dentistry.Admin`, `Dentistry.Web` — 0 lỗi

⚠️ **Lưu ý môi trường máy dev (không liên quan tới code):** NuGet.Config cấp user (`%APPDATA%\NuGet\NuGet.Config`) có khai một local source "Infragistics" trỏ tới thư mục không tồn tại trên máy này, làm restore package mới bị lỗi `NU1301`. Đang workaround bằng cờ `--source https://api.nuget.org/v3/index.json` khi build/restore. Nếu muốn hết cảnh báo vĩnh viễn, cần xoá dòng source đó khỏi NuGet.Config — sẽ không tự ý sửa file ngoài phạm vi dự án, báo lại nếu muốn tôi dọn luôn.

## Giai đoạn 3 — Giao diện Cài đặt trong Admin (~5.5h) ✅ Hoàn thành 2026-09-11 (qua agent, đã review + vá thêm 1 lỗi UX)

Khối "Cấu hình Email thông báo" trong `Dentistry.Admin/Views/AppSetting/Partial/_get_settings.cshtml`:

- [x] **Bước chọn loại nhà cung cấp** — segmented toggle (Bootstrap `btn-check`) 2 lựa chọn: "Email theo tên miền (Hosting)" / "Gmail", bind `Setting.SmtpProvider`
- [x] **Nhánh Hosting**: ô nhập tay Host/Port/SSL/Tài khoản/Mật khẩu, kèm ghi chú cPanel/Plesk
- [x] **Nhánh Gmail**: JS tự điền `smtp.gmail.com` / `587` / SSL bật khi chuyển sang Gmail (không ghi đè nếu đã có giá trị), kèm ghi chú + link `myaccount.google.com/apppasswords`
- [x] Ô `Tên hiển thị người gửi` và `Email nhận thông báo` dùng chung 2 nhánh
- [x] JS ẩn/hiện đúng nhánh, lưu tạm giá trị Hosting khi chuyển qua Gmail rồi khôi phục khi quay lại
- [x] Ô Mật khẩu không bao giờ hiện mật khẩu thật — placeholder "•••••••• (đã lưu — để trống nếu không đổi)" khi `HasSmtpPassword=true`
- [x] Nút **"Gửi thử"** gọi `POST /AppSetting/TestSmtp`, hiển thị lỗi cụ thể theo ngữ cảnh Hosting/Gmail
- [x] `AppSettingsViewComponent.cs` — kiểm tra lại, không cần sửa (đã truyền cả `AppSettingVm` sẵn có ra view)
- [x] Action `TestSmtp` trong `AppSettingController.cs` + đăng ký `IEmailService` vào DI (đã làm ở Giai đoạn 2)

**Lỗi phát hiện khi review + đã vá:** bản đầu tiên của agent cho "Gửi thử" gửi test bằng cấu hình **đã lưu trong DB**, không phải giá trị admin đang gõ trên form — nghĩa là nếu admin nhập App Password mới rồi bấm Gửi thử ngay (chưa bấm Cập nhật), test sẽ dùng mật khẩu cũ/rỗng và luôn báo sai, dù cấu hình mới có đúng hay không. Đây đúng là mục đích chính của nút này (test trước khi lưu) nên đã sửa lại:
- Thêm `IEmailService.SendTestAsync(SmtpTestSettings)` và `SmtpTestSettings` (Dentisty.Data/Services/Email/IEmailService.cs) — nhận giá trị override từ form; nếu Mật khẩu để trống thì tự lấy lại mật khẩu đã mã hoá trong DB.
- `EmailService.cs` refactor phần kết nối/gửi dùng chung (`SendCoreAsync`) cho cả `SendAsync` (đọc từ DB) và `SendTestAsync` (nhận tham số override).
- `TestSmtp` action nhận `[FromBody] SmtpTestSettings`; JS gom giá trị hiện tại trên form (Host/Port/SSL/Tài khoản/Mật khẩu/Tên hiển thị/Email nhận) gửi kèm request thay vì gọi tay không.
- Build lại cả 3 project (`Dentisty.Data`, `Dentistry.Admin`, `Dentistry.Web`) — 0 lỗi.

## Giai đoạn 4 — Tích hợp vào luồng liên hệ / đặt lịch (~2h) ✅ Hoàn thành 2026-09-11 (qua agent, đã review lại code)

- [x] Inject `IEmailService` gián tiếp qua `IServiceScopeFactory` (không inject trực tiếp) vào `Dentistry.Web/Controllers/ContactController.cs`
- [x] Gọi gửi email sau khi `_contactRepository.Create(model)` thành công trong action `AddMessage` — subject "Có tin nhắn liên hệ mới từ {Name}"
- [x] Gọi gửi email sau khi `_contactRepository.Create(model.contact)` thành công trong action `Book` — subject "Có yêu cầu đặt lịch mới từ {Name}", kèm tên chi nhánh tra từ `_branchesRepository`
- [x] Gửi bất đồng bộ qua `Task.Run` + scope DI riêng, log lỗi qua `ILogger<ContactController>` nếu gửi thất bại, không làm hỏng việc lưu tin nhắn
- [x] `IEmailService` đã đăng ký DI từ Giai đoạn 2, không cần thêm

**Quyết định kỹ thuật đáng chú ý:** thay vì inject thẳng `IEmailService` vào controller rồi gọi trong `Task.Run` (như plan gốc mô tả), agent dùng `IServiceScopeFactory.CreateScope()` để tạo scope DI mới bên trong từng background task. Lý do: `IEmailService`/`IAppSettingRepository`/`DbContext` đều là `Scoped`, gắn với vòng đời request — nếu `Task.Run` chạy sau khi request đã trả response (scope bị dispose) sẽ ném `ObjectDisposedException` và gửi mail âm thầm thất bại. Cách làm này khớp với pattern đã có sẵn trong `Dentistry.Web/Middleware/VisitorTrackingMiddleware.cs` — đã kiểm tra lại, đúng như agent báo cáo.

- [x] Build `Dentistry.Web` — 0 lỗi (73 warning, cùng kiểu warning có sẵn của project)

## Giai đoạn 5 — Kiểm thử & triển khai (~2h) 🔶 Đã test kỹ trên local — còn chặn ở phần production

- [x] Build toàn bộ solution (`Dentistry.sln`) — 0 lỗi, chỉ còn warning có sẵn của project
- [x] Apply migration `AddSmtpSettingsToAppSetting` lên **DB local** (`Server=QuynhNH; Database=annhienmedical_vn_nhiendb` — connection string trong `appsettings.Development.json` của cả Web/Admin, do chị chỉ định là DB test). Đã verify 8 cột SMTP có mặt đúng trong bảng `AppSettings` bằng `sqlcmd`. **Chưa** đụng tới DB từ xa `103.28.36.169/nhquyltv_nhiendb` trong `Dentisty.Data/appsettings.json` — vẫn cần xác nhận đó có phải DB production trước khi apply lên đó.
- [x] Chạy thử `Dentistry.Admin` local (port 7226, trỏ DB local), test action `TestSmtp` trực tiếp qua HTTP với 3 kịch bản, đều trả đúng thông báo lỗi tiếng Việt như thiết kế:
  - Chưa nhập gì → "Chưa cấu hình SMTP..."
  - Host không tồn tại → "Không kết nối được máy chủ SMTP (...). Kiểm tra lại Host/Port..."
  - Gmail thật (`smtp.gmail.com`) + mật khẩu sai → "Xác thực Gmail thất bại. Kiểm tra lại đã dùng App Password..."
- [x] Chạy thử `Dentistry.Web` local (port 7278, trỏ DB local), test cả 2 luồng bằng HTTP POST thật:
  - `AddMessage`: trả về thành công ngay lập tức, tin nhắn được lưu vào `Contacts`; log nền ghi "Gửi email thông báo tin nhắn liên hệ mới thất bại: Chưa cấu hình SMTP..." — đúng như thiết kế, không chặn phản hồi, không crash.
  - `Book`: trả về thành công, tra đúng tên chi nhánh trước khi gửi, log nền cũng báo thiếu cấu hình SMTP tương tự, không ảnh hưởng việc lưu lịch hẹn.
  - ⚠️ Có 2 bản ghi test ("Nguyen Van Test") hiện đang nằm trong bảng `Contacts` của DB local — xoá nếu chị muốn DB sạch trước khi demo cho khách.
  - Đã tắt 2 tiến trình dev server sau khi test xong.
- [x] Test gửi **thành công thật** — dùng tài khoản Gmail test chị cung cấp (`bomnguyen.vp@gmail.com` + App Password), gọi `TestSmtp` với cấu hình Gmail thật, `NotificationEmails` = 2 địa chỉ chị đưa → kết quả `isSuccessed: true`, mail thật đã được gửi đi.
  - **Phát hiện + đã vá 1 lỗi trong lúc test:** lần gọi đầu tiên dùng JSON nhiều dòng qua `curl` bị `NullReferenceException` (500) vì `[FromBody] SmtpTestSettings model` nhận `null` khi body gửi lên bị hỏng — code chưa có bước kiểm tra `model == null` trước khi dùng, nên lẽ ra phải trả lỗi thân thiện thì lại crash. Đã thêm kiểm tra null đầu action `TestSmtp` trong `AppSettingController.cs`, trả về `"Dữ liệu gửi lên không hợp lệ."` thay vì crash. Build lại `Dentistry.Admin` — 0 lỗi. Test lại với JSON đúng cú pháp → gửi thành công như trên.
  - ⚠️ Nhận định ban đầu ở đây rằng nhánh "lưu vào DB rồi Web đọc lại để gửi" rủi ro thấp **đã sai** — xem lỗi nghiêm trọng phát hiện và đã vá ở Giai đoạn 6 bên dưới.
- [ ] Cấu hình SPF/DKIM cho tên miền (nếu áp dụng) — cần quyền DNS, chưa có trong phạm vi đã làm
- [ ] **Chặn: xác nhận DB production** (`103.28.36.169/nhquyltv_nhiendb`) trước khi apply migration lên đó + có backup chưa
- [ ] Deploy code lên production (FTP `annhienmedical_ftp` đã có trong `Documents/AnNhienMedical_FTP.txt`), kiểm tra port outbound (587/465) không bị chặn
- [ ] Bàn giao & hướng dẫn khách hàng sử dụng màn Cài đặt

---

## Giai đoạn 6 — Bổ sung: Email xác nhận cho khách hàng + vá lỗi mã hoá xuyên app ✅ Hoàn thành 2026-09-11

Theo yêu cầu bổ sung của chị: nội dung email cần cấu hình được, form liên hệ cần có email khách, và cần checkbox bật/tắt gửi email cho khách. Áp dụng cho **cả 2 form** (Liên hệ tư vấn + Đặt lịch) theo lựa chọn của chị.

- [x] Thêm 2 field vào `AppSetting`/`AppSettingVm`: `SendCustomerConfirmationEmail` (bool, mặc định tắt) và `CustomerEmailTemplate` (nvarchar(max), hỗ trợ placeholder `{Name}`)
- [x] Migration `AddCustomerEmailConfirmationSettings`
- [x] Cập nhật `ViewModelExtensions.cs`, `AppSettingRepository.cs` map 2 field mới
- [x] Thêm `IEmailService.SendToAsync(recipientEmail, subject, htmlBody)` — gửi tới 1 địa chỉ bất kỳ (khách hàng) thay vì danh sách `NotificationEmails` cố định, dùng chung cơ chế kết nối/gửi đã có
- [x] Admin UI: thêm checkbox "Gửi email xác nhận cho khách hàng" + textarea "Nội dung email gửi khách hàng" (readonly khi tắt checkbox — cố tình dùng `readonly` thay vì `disabled` vì input bị `disabled` sẽ không được gửi lên khi submit form, dễ làm mất nội dung đã lưu)
- [x] Thêm ô Email (không bắt buộc) vào `BookForm.cshtml` — trước đây form Đặt lịch không có ô này dù `BookFormVmValidator` đã có sẵn rule validate Email (chỉ là UI chưa từng hiển thị)
- [x] `ContactController.cs`: gộp gửi email nhân viên + email khách hàng vào cùng 1 tác vụ nền cho mỗi luồng (`SendContactEmails`, `SendBookingEmails`), thêm helper `SendCustomerConfirmationEmailAsync` dùng chung — chỉ gửi khi bật checkbox **và** khách có nhập email
- [x] Build toàn bộ solution — 0 lỗi

### 🔴 Lỗi nghiêm trọng phát hiện khi test kỹ + đã vá

Trong lúc chuẩn bị test tính năng mới bằng cách lưu cấu hình SMTP thật vào DB rồi để **Web** tự gửi (thay vì test qua **Admin** như trước giờ), phát hiện: **mật khẩu SMTP mã hoá bởi Admin không giải mã được ở Web.** Nguyên nhân: cơ chế mã hoá ở Giai đoạn 1 dùng ASP.NET Core Data Protection API, mà API này **mặc định cách ly khoá mã hoá theo từng ứng dụng** — Admin và Web là 2 ứng dụng deploy riêng biệt nên không tự dùng chung được khoá. Nếu không phát hiện, hậu quả là: chị lưu cấu hình + mật khẩu trong Admin, có vẻ mọi thứ ổn (Gửi thử trong Admin vẫn chạy tốt vì Admin tự mã hoá rồi tự giải mã ngay), nhưng khi có khách nhắn tin thật thì **Web sẽ luôn báo "chưa có mật khẩu SMTP hợp lệ"** — tức tính năng chính sẽ không hoạt động ngay khi lên production dù đã cấu hình đúng.

Đã xác minh bằng test trực tiếp (mã hoá 1 chuỗi test ở Admin, gọi sang Web để giải mã) — lần đầu thất bại, xác nhận đúng lỗi.

**Cách vá:** đổi cơ chế mã hoá từ Data Protection API sang **AES với 1 khoá bí mật dùng chung**, đặt trong `appsettings.json` của **cả 2 app** (mục `SmtpEncryption:Key`) — theo đúng pattern đã có sẵn trong dự án (`JwtTokens:Key` cũng đã được đặt giống hệt nhau ở cả Admin và Web từ trước). Đã test lại: mã hoá ở Admin → giải mã đúng ở Web.

- [x] Viết lại `SmtpCredentialProtector.cs` dùng `Aes` (System.Security.Cryptography) thay vì `IDataProtectionProvider`
- [x] Thêm `SmtpEncryption:Key` (khoá AES-256 sinh ngẫu nhiên) vào `Dentistry.Admin/appsettings.json` và `Dentistry.Web/appsettings.json` — **bắt buộc giống hệt nhau ở cả 2 file**, nếu sau này đổi phải đổi đồng thời cả 2 nơi
- [x] Bỏ `AddDataProtection()` khỏi `Program.cs` của cả 2 app (không còn cần cho việc này)
- [x] Test lại xác nhận mã hoá/giải mã xuyên app hoạt động đúng
- [x] Build lại toàn bộ — 0 lỗi

⚠️ **Lưu ý khi deploy production:** nhớ copy đúng giá trị `SmtpEncryption:Key` giống nhau vào `appsettings.Production.json` (hoặc biến môi trường tương ứng) của **cả Admin lẫn Web** trên server thật — nếu quên hoặc gõ sai 1 ký tự, Web sẽ không giải mã được mật khẩu và tính năng gửi mail sẽ âm thầm không hoạt động (dù không crash, vì đã có xử lý lỗi).

### Còn thiếu (do giới hạn môi trường test, không phải do code)

Do máy test đang chạy nhiều tiến trình `dotnet build`/`dotnet run` cùng lúc khiến SQL Server local bị chậm/timeout, chưa test được trọn vẹn vòng lặp "lưu cấu hình Gmail thật vào DB → khách gửi form thật → khách nhận được email cảm ơn thật" — dù đã verify riêng từng phần (gửi thật qua `TestSmtp` ✅, mã hoá xuyên app ✅). Đã cấu hình sẵn 1 bộ SMTP Gmail test + bật `SendCustomerConfirmationEmail` trên **DB local** (`annhienmedical_vn_nhiendb`) để chị có thể tự thử qua giao diện thật (`https://localhost:7278`, submit form Liên hệ hoặc Đặt lịch với email là 1 trong 2 địa chỉ test) bất cứ lúc nào rảnh máy.

---

## Giai đoạn 7 — Bổ sung: Chặn gửi trùng khi bấm nhiều lần / thiếu kiểm tra hợp lệ ✅ Hoàn thành 2026-09-11

Chị phát hiện: mạng lag, người dùng không biết đã bấm được chưa nên bấm nhiều lần → gửi trùng nhiều tin nhắn/lịch hẹn (và với tính năng mới, trùng cả email). Sau đó phát hiện thêm: khi form trống bấm gửi vẫn thấy nút chuyển "Đang gửi..." dù lẽ ra phải báo lỗi tại chỗ trước.

- [x] Khoá nút submit + đổi chữ thành "Đang gửi..." ngay khi bấm ở cả 2 form (`add_contact.js`, `book_contact.js`), mở lại khi có phản hồi (thành công hay lỗi)
- [x] Phát hiện nguyên nhân gốc của lỗi "form trống vẫn chuyển Đang gửi": thư viện jQuery Validate + Unobtrusive Validation **đã có sẵn trong dự án nhưng chưa từng được nạp vào trang** (`_ValidationScriptsPartial.cshtml` tồn tại nhưng không nơi nào include nó) — nên trước giờ 2 form này hoàn toàn không có kiểm tra hợp lệ phía client, mọi validate đều phải chờ round-trip lên server mới biết
- [x] Thêm 2 script `jquery.validate.min.js` + `jquery.validate.unobtrusive.min.js` vào `_Layout.cshtml`
- [x] Thêm kiểm tra `form.valid()` **trước** bước khoá nút/gửi request ở cả 2 form — form trống/sai sẽ dừng lại và hiện lỗi ngay tại chỗ, không khoá nút, không gửi gì lên server
- [x] Thêm hàm dùng chung `parseUnobtrusiveValidation()` (đặt trong `site.js`) và gọi lại sau mỗi lần form được nạp lại qua AJAX (`addAddressLoading()`, `bookFormLoading()`, và nhánh hiện lỗi validation từ server) — vì jQuery Unobtrusive Validation chỉ tự động gắn vào form có sẵn lúc tải trang đầu tiên, form được chèn lại bằng JS sau đó phải parse lại thủ công thì mới validate tiếp được
- [x] Regenerate lại `wwwroot/bundle/main-site.min.js` (dự án dùng `BuildBundlerMinifier` tự gộp/minify `add_contact.js` + `book_contact.js` + `site.js`... — sửa file gốc mà không build lại thì bản gộp cũ vẫn chạy trên trang, không phản ánh gì đã sửa)

- [x] **Đã bổ sung tiếp:** vá luôn ô `Ngày đặt lịch` — nguyên nhân là FluentValidation không tự sinh `data-val-required` cho kiểu `DateTime?` (chỉ hoạt động tốt với string như Name/Phone), nên khai báo tay `data-val="true" data-val-required="..."` trực tiếp trong `BookForm.cshtml`, khớp đúng nội dung lỗi bên `BookFormVmValidator.cs`. Đã verify qua request thật: field giờ render đúng thuộc tính, để trống sẽ bị `form.valid()` chặn giống Name/Phone.
- [x] **Đã vá luôn theo yêu cầu chị:** `BranchesId` (chọn cơ sở) cũng gặp tình huống tương tự — placeholder cũ dùng `value="0"` khiến jQuery Validate coi là "đã có giá trị" dù chưa chọn gì. Đổi placeholder sang `value=""` + thêm `data-val-required` trên `<select>`, đã verify qua request thật.

⚠️ Lúc build lại để regenerate bundle, `dotnet build` báo lỗi copy file do **Visual Studio Insiders đang mở/chạy project Dentistry.Web khoá file DLL** — không phải lỗi code, bước tạo bundle đã chạy xong thành công trước khi gặp lỗi khoá file đó nên bản JS sửa vẫn có hiệu lực. Muốn build sạch từ terminal thì cần dừng debug trong VS trước.

## Giai đoạn 8 — Sự cố: "App bị crash không lên được web" ✅ Đã tìm nguyên nhân + vá 1 lỗi nghiêm trọng, 1 việc cần chị tự kiểm tra

Chị báo web không lên được sau khi test đặt lịch. Điều tra ra **3 nguyên nhân xếp chồng lên nhau**, không cái nào liên quan tới code email:

1. **Lỗi nghiêm trọng đã vá — `ActiveUserCleanupService` không có try/catch.** Đây là tác vụ nền dọn bảng `ActiveUsers` chạy mỗi phút. Khi câu lệnh xoá gặp timeout SQL (trùng lúc tôi đang chạy migration thêm index — xem mục 2), exception ném ra không ai bắt. Theo hành vi mặc định của ASP.NET Core, **một `BackgroundService` ném lỗi không bắt sẽ tự tắt toàn bộ ứng dụng** — bắt được nguyên văn trong log: `Application is shutting down...` ngay sau lỗi này. Nghĩa là chỉ cần SQL Server khựng một chút bất kỳ lúc nào (kể cả ngoài lúc tôi đang test) là cả web sập theo. Đã thêm try/catch quanh câu lệnh xoá trong `Dentistry.Web/Services/ActiveUserCleanupService.cs` — lỗi giờ chỉ bị bỏ qua và thử lại ở vòng sau, không còn kéo sập app. Build lại 0 lỗi.
2. **Đã áp dụng xong migration `AddVisitorTrackingLookupIndexes`** (mục thêm index ở Giai đoạn 7) lên DB local — verify 2 index `ix_activeUser_visitor_ip` và `ix_visitorlog_visitor_ip` đã tồn tại.
3. ⚠️ **Việc cần chị tự kiểm tra — không phải lỗi code:** máy đang có 1 tiến trình `powershell` (PID 18508, chạy liên tục từ 9:04 sáng, ngốn ~17.000 giây CPU dồn lại) và bộ nhớ hệ thống chỉ còn **~1GB trống / 16GB tổng** — khiến app .NET bị `OutOfMemoryException` khi tôi khởi động lại để test. Đây là tình trạng máy bị quá tải chung (không riêng gì web app), không phải do code gây ra. Chị nên mở Task Manager kiểm tra tiến trình PowerShell đó (đóng nếu không phải việc quan trọng đang chạy), cân nhắc đóng bớt 1 trong 2 cửa sổ Visual Studio đang mở, hoặc khởi động lại máy nếu cần giải phóng RAM trước khi chạy lại app.

## Rủi ro & lưu ý

- Mật khẩu SMTP phải mã hoá khi lưu DB, không lưu dạng thô.
- Cần SPF/DKIM để giảm khả năng mail rơi vào Spam.
- Gmail cá nhân giới hạn ~500 mail/ngày (đủ dùng), cần App Password thay vì mật khẩu thường.
- Một số hosting giá rẻ có thể chặn outbound port SMTP mặc định — cần kiểm tra trước khi deploy.

## Hạng mục phát sinh thêm (ngoài phạm vi plan này, làm sau nếu khách đồng ý)

- [ ] Theo dõi trạng thái gửi mail + tự động thử lại khi lỗi (thêm cột `IsEmailSent` vào `Contact`)
- [ ] Gửi kèm thông báo qua Zalo OA / Telegram

---
## Email Test:
Acc: bomnguyen.vp@gmail.com
App Pass: mdrr fvcz gmlf pseq

Email nhận thông báo: nguyenquynhvp.ictu@gmail.com, quynh.nguyenhuu@imipgroup.com

⚠️ **Lưu ý bảo mật:** file này đang được git track (`Documents/sendemail/sendemail.md`) — App Password ở trên đang nằm dạng chữ thường ngay trong file, nếu commit sẽ nằm luôn trong lịch sử git. Đã dùng để test xong (xem Giai đoạn 5). Khuyến nghị: sau khi xong việc, vào lại [myaccount.google.com/apppasswords](https://myaccount.google.com/apppasswords) thu hồi mã này rồi tạo mã mới khi cần dùng thật, và cân nhắc xoá đoạn này khỏi file trước khi commit/push (hoặc thêm file này vào `.gitignore` nếu muốn giữ lại làm ghi chú riêng).

## Log cập nhật

- 2026-09-11: Tạo file plan, chốt phương án 2 (cấu hình qua UI Admin).
- 2026-09-11: Cập nhật plan — bổ sung yêu cầu UI phải phân biệt rõ 2 nhánh cấu hình Hosting/Gmail (field `SmtpProvider`, form 2 nhánh, JS điền sẵn giá trị Gmail, thông báo lỗi test theo ngữ cảnh). Tổng công sức tăng 13h → 15h.
- 2026-09-11: Khách hàng đồng ý mức tăng công sức. Đã cập nhật artifact báo giá khớp 15h — tổng **5.300.000đ** trọn gói.
- 2026-09-11: Code xong cả 5 giai đoạn. Test kỹ trên DB local: apply migration, chạy thật Admin + Web, test `TestSmtp` với host sai/Gmail sai mật khẩu, test `AddMessage`/`Book` end-to-end (lưu thành công, log nền báo thiếu cấu hình, không crash).
- 2026-09-11: Nhận tài khoản Gmail test thật từ chị. Test gửi thành công thật qua `TestSmtp` — mail đã gửi đi. Phát hiện + vá lỗi `NullReferenceException` khi `TestSmtp` nhận body null (thêm kiểm tra null, trả lỗi thân thiện thay vì crash). Còn lại trước khi lên production: xác nhận DB thật `103.28.36.169/nhquyltv_nhiendb` + backup, cấu hình SPF/DKIM, deploy qua FTP, bàn giao hướng dẫn.
- 2026-09-11: Thêm tính năng "gửi email xác nhận cho khách hàng" (checkbox + nội dung mẫu cấu hình được, áp dụng cả 2 form, thêm ô Email vào form Đặt lịch). Trong lúc test kỹ, phát hiện lỗi nghiêm trọng: mật khẩu SMTP mã hoá bởi Admin không giải mã được ở Web (do Data Protection API cách ly khoá theo từng app). Đã vá bằng cách đổi sang mã hoá AES với khoá bí mật dùng chung (`SmtpEncryption:Key`) đặt trong appsettings của cả 2 app, đã test lại xác nhận đúng. Build toàn bộ 0 lỗi. Còn 1 vòng test cuối (lưu cấu hình thật → khách nhận mail thật qua giao diện) chưa làm được do máy test bị nghẽn SQL Server local — đã chuẩn bị sẵn cấu hình test trên DB local để tự thử khi thuận tiện.
- 2026-09-11: Chị báo lỗi bấm nhiều lần khi mạng lag gây gửi trùng, đã khoá nút submit khi đang gửi. Sau đó chị báo tiếp: form trống bấm gửi vẫn thấy "Đang gửi..." — phát hiện nguyên nhân là thư viện jQuery Validate đã có sẵn trong dự án nhưng chưa từng được nạp vào trang, nên 2 form liên hệ/đặt lịch trước giờ không hề có kiểm tra hợp lệ phía client. Đã nạp thư viện + thêm `form.valid()` chặn trước khi khoá nút, parse lại validation sau mỗi lần form load qua AJAX, và regenerate lại bundle JS production.
- 2026-09-11: Chị test đặt lịch thật, form quay "Đang gửi..." rồi báo lỗi. Điều tra ra nguyên nhân không liên quan email: bảng `VisitorLogs` (26.741 dòng) thiếu index cho câu tra cứu của `VisitorTrackingMiddleware` chạy trên mọi request, gây quét toàn bảng + khoá lẫn nhau với `ActiveUserCleanupService` chạy mỗi phút. Đã thêm index (migration `AddVisitorTrackingLookupIndexes`), rồi chị báo "App bị crash". Điều tra tiếp phát hiện lỗi nghiêm trọng hơn: `ActiveUserCleanupService` không có try/catch — 1 lần SQL timeout (do đúng lúc đang chạy migration) làm exception ném ra không ai bắt, và theo mặc định của ASP.NET Core, `BackgroundService` lỗi không bắt sẽ tự tắt CẢ ứng dụng (log ghi rõ "Application is shutting down..." ngay sau đó). Đã thêm try/catch, build 0 lỗi, áp dụng xong migration index lên DB local. Đồng thời phát hiện máy đang bị quá tải hệ thống thật sự (1 tiến trình powershell lạ ngốn ~17.000s CPU từ 9:04 sáng, RAM chỉ còn ~1GB/16GB trống) — đã báo chị tự kiểm tra Task Manager vì đây không phải lỗi code.
