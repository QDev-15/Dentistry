using Dentistry.Admin.Common;
using Dentistry.Data.Common.Constants;
using Dentistry.Data.GeneratorDB.EF;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.Data.Storages;
using Dentistry.ViewModels.Catalog.Articles;
using Dentistry.ViewModels.Catalog.Branches;
using Dentistry.ViewModels.Catalog.Categories;
using Dentistry.ViewModels.Catalog.Contacts;
using Dentistry.ViewModels.Catalog.Doctors;
using Dentistry.ViewModels.Catalog.Slide;
using Dentistry.ViewModels.System.Users;
using Dentisty.Data;
using Dentisty.Data.Common;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Repositories;
using Dentisty.Data.Services.System;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
// Add DbContext
builder.Services.AddDbContext<DentistryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(SystemConstants.MainConnectionString)));

builder.Services.AddIdentity<AppUser, AppRole>()
.AddEntityFrameworkStores<DentistryDbContext>()
.AddDefaultTokenProviders();

// Đăng ký HostingConfig
// Configure app settings based on environment
var environment = builder.Environment.EnvironmentName;
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Tải cấu hình UploadSettings từ appsettings
builder.Services.Configure<HostingConfig>(builder.Configuration.GetSection("HostingConfig"));
var hostingConfig = builder.Configuration.GetSection("HostingConfig").Get<HostingConfig>();


var issuer = builder.Configuration.GetValue<string>(SystemConstants.JwtTokens.Issuer);
var audience = builder.Configuration.GetValue<string>(SystemConstants.JwtTokens.Audience);
var signingKey = builder.Configuration.GetValue<string>(SystemConstants.JwtTokens.Key);
byte[] signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(signingKey!);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.LogoutPath = "/User/Logout";
        options.AccessDeniedPath = "/User/Forbidden/";
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = context =>
            {
                context.Response.Redirect("/Login"); // Custom login path
                return Task.CompletedTask;
            },
            OnRedirectToAccessDenied = context =>
            {
                context.Response.Redirect("/Home/AccessDenied"); // Custom access denied path
                return Task.CompletedTask;
            }
        };
    }).AddJwtBearer(options =>
    {

        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            ClockSkew = System.TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
        };
    });
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Chỉ hoạt động với HTTPS
    options.Cookie.SameSite = SameSiteMode.None; // Quan trọng để hỗ trợ nhiều tab/domain
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
});
// add controller views
builder.Services.AddControllersWithViews(options =>
{
    // write log exception controller
    options.Filters.Add<GlobalExceptionFilter>();
})
    .AddRazorRuntimeCompilation()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>();
        fv.RegisterValidatorsFromAssemblyContaining<RegisterRequestValidator>();
        fv.RegisterValidatorsFromAssemblyContaining<SlideVmValidator>();
        fv.RegisterValidatorsFromAssemblyContaining<ArticleVmValidator>();
        fv.RegisterValidatorsFromAssemblyContaining<CategoryVmValidator>();
        fv.RegisterValidatorsFromAssemblyContaining<ContactVmValidator>();
        fv.RegisterValidatorsFromAssemblyContaining<DoctorVmValidator>();
        fv.RegisterValidatorsFromAssemblyContaining<BranchesVmValidator>();
        fv.DisableDataAnnotationsValidation = true;
    });

// add resource validator
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Register Repository  add services
builder.Services.AddSingleton<Logs>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<DentistryDbContext>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<ICategoryReposiroty, CategoryRepository>();
builder.Services.AddScoped<ISlideRepository, SlideRepository>();
builder.Services.AddScoped<IStorageService, FileStorageService>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IBranchesRepository, BranchesRepository>();
builder.Services.AddScoped<IAppSettingRepository, AppSettingRepository>();



builder.Services.AddScoped<LoggerRepository>();
builder.Services.AddHostedService<LoggerBackgroundService>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy =>
        {
            //policy.WithOrigins("https://localhost:7278") // Cho phép website kết nối     // https://nhien.quynhvpit.io.vn
            //policy.WithOrigins("https://nhien.quynhvpit.io.vn") // Cho phép website kết nối     // 
            //policy.WithOrigins("https://annhienmedical.vn") // Cho phép website kết nối     // 
            policy.WithOrigins(hostingConfig!.WebHost) // Cho phép website kết nối     // 
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); // Cần thiết cho SignalR
        });
});
// Đăng ký SignalR
builder.Services.AddSignalR();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Xử lý lỗi cho Production
}
else
{
    app.UseDeveloperExceptionPage(); // Hiển thị lỗi chi tiết khi ở Development Mode
}
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DentistryDbContext>();
    dbContext.Database.Migrate();
}

app.UseForwardedHeaders();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseMiddleware<TimeZoneMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

// Kích hoạt CORS
app.UseCors("AllowSpecificOrigins");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<SignalRHub>("/signalRHub"); // Định tuyến Hub
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
