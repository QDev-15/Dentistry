using Dentistry.Admin.Common;
using Dentistry.Admin.Middlewares;
using Dentistry.Common;
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
using Dentisty.Data.Interfaces;
using Dentisty.Data.Repositories;
using Dentisty.Data.Services;
using Dentisty.Data.Services.Interfaces;
using Dentisty.Data.Services.System;
using Dentisty.Data.Storages;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
// Add DbContext
builder.Services.AddDbContext<DentistryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(SystemConstants.MainConnectionString), sql => sql.UseCompatibilityLevel(120)));
builder.Services.AddHttpClient();

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
builder.Services.Configure<JwtTokens>(builder.Configuration.GetSection("JwtTokens"));
var hostingConfig = builder.Configuration.GetSection("HostingConfig").Get<HostingConfig>();
var jwtTokens = builder.Configuration.GetSection("JwtTokens").Get<JwtTokens>();


byte[] signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(jwtTokens!.Key);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {

        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtTokens.Issuer,
            ValidAudience = jwtTokens.Audience,
            ClockSkew = System.TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
        };
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

// Đăng ký SignalR
builder.Services.AddSignalR();

// Register Repository  add services
builder.Services.AddSingleton<Logs>();
builder.Services.AddSingleton<AppConfigService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<ITimezoneService, TimezoneService>();
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
builder.Services.AddScoped<IAccessRepository, AccessRepository>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<CacheNotificationService>();

builder.Services.AddScoped<LoggerRepository>();
builder.Services.AddHostedService<LoggerBackgroundService>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<DatabaseBackupService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins(hostingConfig!.WebHost) // Cho phép website kết nối     // 
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); // Cần thiết cho SignalR
        });
});
builder.Services.AddHttpContextAccessor();


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
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 401)
    {
        context.Response.Redirect("/Login");
    }
});
app.UseForwardedHeaders();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseMiddleware<JwtFromCookieMiddleware>();
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
