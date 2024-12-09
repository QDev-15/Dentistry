using Dentistry.Common.Constants;
using Dentistry.Data.GeneratorDB.EF;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.Data.Interfaces;
using Dentistry.Data.Services;
using Dentistry.Data.Storages;
using Dentistry.ViewModels.System.Users;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Repositories;
using Dentisty.Data.Services;
using Dentisty.Data.Services.System;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NhienDentistry.Core.Catalog.Articles;

var builder = WebApplication.CreateBuilder(args);
// Add DbContext
builder.Services.AddDbContext<DentistryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(Constants.MainConnectionString)));

builder.Services.AddIdentity<AppUser, AppRole>()
.AddEntityFrameworkStores<DentistryDbContext>()
.AddDefaultTokenProviders();

var issuer = builder.Configuration.GetValue<string>(Constants.JwtTokens.Issuer);
var audience = builder.Configuration.GetValue<string>(Constants.JwtTokens.Audience);
var signingKey = builder.Configuration.GetValue<string>(Constants.JwtTokens.Key);
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
builder.Services.AddControllersWithViews()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<RegisterRequestValidator>();
        fv.DisableDataAnnotationsValidation = true;
    });

// add resource validator
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

// Register Repository and Service
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<IUserRepository, UserService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(_Repository<>));
builder.Services.AddScoped<IStorageService, FileStorageService>();
builder.Services.AddScoped<LanguagesServices>();
builder.Services.AddScoped<SlideService>();
builder.Services.AddScoped<ArticlesService>();
builder.Services.AddScoped<CategoriesService>();




var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
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
