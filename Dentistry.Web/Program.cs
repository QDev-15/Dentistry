using Dentistry.Common;
using Dentistry.Data.GeneratorDB.EF;
using Dentistry.Data.Storages;
using Dentistry.ViewModels.Catalog.Contacts;
using Dentistry.Web.Middleware;
using Dentistry.Web.Services;
using Dentisty.Common;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Repositories;
using Dentisty.Data.Services;
using Dentisty.Data.Services.Interfaces;
using Dentisty.Data.Services.System;
using Dentisty.Web.Services;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Tải cấu hình UploadSettings từ appsettings
var environment = builder.Environment.EnvironmentName;
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();
builder.Services.Configure<HostingConfig>(builder.Configuration.GetSection("HostingConfig"));
var hostingConfig = builder.Configuration.GetSection("HostingConfig").Get<HostingConfig>();

// Add SignalR
builder.Services.AddSignalR();

// Register DbContext
builder.Services.AddDbContext<DentistryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(SystemConstants.MainConnectionString)));
// Register Repository  add services
builder.Services.AddSingleton<Logs>();
builder.Services.AddSingleton<CacheInvalidationListener>();
builder.Services.AddScoped<ApplicationService>();
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddScoped<DentistryDbContext>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<ICategoryReposiroty, CategoryRepository>();
builder.Services.AddScoped<ISlideRepository, SlideRepository>();
builder.Services.AddScoped<IStorageService, FileStorageService>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IAppSettingRepository, AppSettingRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IBranchesRepository, BranchesRepository>();
builder.Services.AddScoped<LoggerRepository>();

// Background Services
builder.Services.AddHostedService<LoggerBackgroundService>();
builder.Services.AddHostedService<ActiveUserCleanupService>();

// Add controller
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation()
    .AddFluentValidation(fv => {
        fv.RegisterValidatorsFromAssemblyContaining<ContactVmValidator>();
        fv.RegisterValidatorsFromAssemblyContaining<BookFormVmValidator>();
        fv.DisableDataAnnotationsValidation = true;
    });
// add HttpContext
//builder.Services.AddHttpContextAccessor();
// add Memory Cache
builder.Services.AddMemoryCache();
// add resource validator
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Minify file
builder.Services.AddWebOptimizer(options =>
{
    options.MinifyCssFiles("/css/*.css");
    options.MinifyJsFiles("/js/*.js");
    options.MinifyJsFiles("/lib/*.js");
    options.MinifyJsFiles("/plugins/*.js");
});


var app = builder.Build();
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value.ToLower();

    // Nếu request là ảnh nhưng không bắt đầu bằng "http" hoặc "/assets/"
    if ((path.EndsWith(".jpg") || path.EndsWith(".png") || path.EndsWith(".gif") ||
         path.EndsWith(".jpeg") || path.EndsWith(".svg") || path.EndsWith(".webp"))
        && !path.StartsWith("http") && !path.StartsWith("/assets/"))
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("Not Found");
        return;
    }

    await next();
});
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // Xử lý lỗi trang không tìm thấy, chuyển hướng đến trang tùy chỉnh
    app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");

    // HSTS from appsettings.json
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseMiddleware<MinifyHtmlMiddleware>(); // Bật Minify HTML Middleware
app.UseMiddleware<VisitorTrackingMiddleware>();



var listener = app.Services.GetRequiredService<CacheInvalidationListener>();
await listener.StartListeningAsync(hostingConfig.AdminHost + "/signalRHub");
app.UseStaticFiles();

app.UseRouting();;

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
File.AppendAllText("wwwroot/logs.txt", $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {hostingConfig.AdminHost}");
app.Run();
File.AppendAllText("wwwroot/logs1.txt", $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {hostingConfig.AdminHost}");
