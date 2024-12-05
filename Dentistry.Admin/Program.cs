using Dentistry.Common.Constants;
using Dentistry.Data.GeneratorDB.EF;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.Data.Interfaces;
using Dentistry.Data.Services;
using Dentistry.Data.Storages;
using Dentisty.Data.Repositories;
using Dentisty.Data.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NhienDentistry.Core.Catalog.Articles;
using System;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);
// Add DbContext
builder.Services.AddDbContext<DentistryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(SystemConstants.MainConnectionString)));

builder.Services.AddIdentity<AppUser, AppRole>()
.AddEntityFrameworkStores<DentistryDbContext>()
.AddDefaultTokenProviders();

// Register Repository and Service
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IStorageService, FileStorageService>();
builder.Services.AddScoped<LanguagesServices>();
builder.Services.AddScoped<SlideService>();
builder.Services.AddScoped<ArticlesService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/Login/Index";
}).AddJwtBearer(options =>
 {
    var jwtSettings = builder.Configuration.GetSection("JwtTokens");
    var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
    options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
     options.Events = new JwtBearerEvents
     {
         OnMessageReceived = context =>
         {
             var token = context.HttpContext.Request.Cookies[SystemConstants.AppSettings.AuthToken];
             if (!string.IsNullOrEmpty(token))
             {
                 context.Token = token;
             }
             return Task.CompletedTask;
         },
         OnChallenge = context =>
         {
             var token = context.HttpContext.Request.Cookies[SystemConstants.AppSettings.AuthToken];
             if (!string.IsNullOrEmpty(token))
             {
                 return Task.CompletedTask;
             }
             
             context.HandleResponse(); // Prevent default 401
             context.Response.Redirect("/Login/Index");
             return Task.CompletedTask;
         },
         OnAuthenticationFailed = context =>
        {
            // Log the exception
            Console.WriteLine("Authentication failed: " + context.Exception.Message);
            return Task.CompletedTask;
        }
     };
 });
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
});



builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();


var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.Use(async (context, next) =>
{
    var token = context.Request.Cookies[SystemConstants.AppSettings.AuthToken];

    if (string.IsNullOrEmpty(token))
    {
        context.Response.Redirect("/Login/Index");
        return;
    }

    await next.Invoke();
});
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
