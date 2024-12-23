using Dentistry.Admin.Common;
using Dentistry.Common.Constants;
using Dentistry.Data.GeneratorDB.EF;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.Data.Storages;
using Dentistry.ViewModels.Catalog.Articles;
using Dentistry.ViewModels.Catalog.Contacts;
using Dentistry.ViewModels.Catalog.Doctors;
using Dentistry.ViewModels.Catalog.Slide;
using Dentistry.ViewModels.System.Users;
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
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
// Tải cấu hình UploadSettings từ appsettings
builder.Services.Configure<HostingConfig>(builder.Configuration.GetSection("HostingConfig"));

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
        fv.RegisterValidatorsFromAssemblyContaining<ContactVmValidator>();
        fv.RegisterValidatorsFromAssemblyContaining<DoctorVmValidator>();
        fv.DisableDataAnnotationsValidation = true;
    });

// add resource validator
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

// Register Repository  add services
builder.Services.AddSingleton<Logs>();
builder.Services.AddScoped<DentistryDbContext>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<ICategoryReposiroty, CategoryRepository>();
builder.Services.AddScoped<ISlideRepository, SlideRepository>();
builder.Services.AddScoped<IStorageService, FileStorageService>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IAppSettingRepository, AppSettingRepository>();



builder.Services.AddScoped<LoggerRepository>();
builder.Services.AddHostedService<LoggerBackgroundService>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoleService>();


var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else if (app.Environment.IsProduction())
{
    app.UseExceptionHandler("/Home/Error");
    Console.WriteLine("Running in Production environment.");
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

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
