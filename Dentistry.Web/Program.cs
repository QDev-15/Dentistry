using Dentistry.Common.Constants;
using Dentistry.Data.GeneratorDB.EF;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Repositories;
using Dentistry.Data.Storages;
using Microsoft.EntityFrameworkCore;
using Dentisty.Data.Services.System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DentistryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(SystemConstants.MainConnectionString)));
// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();
builder.Services.AddHttpContextAccessor();

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

builder.Services.AddScoped<LoggerRepository>();
builder.Services.AddHostedService<LoggerBackgroundService>();

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
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
