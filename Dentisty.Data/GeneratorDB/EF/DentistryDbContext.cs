
using Dentistry.Data.GeneratorDB.Configurations;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.Data.GeneratorDB.Extensions;
using Dentisty.Data.GeneratorDB.Configurations;
using Dentisty.Data.GeneratorDB.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dentistry.Data.GeneratorDB.EF
{
    public class DentistryDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        private readonly IConfiguration _configuration;
        public DentistryDbContext(DbContextOptions<DentistryDbContext> options) : base(options)
        {
            //_configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure using Fluent API

            modelBuilder.ApplyConfiguration(new AppConfigConfiguration());
            modelBuilder.ApplyConfiguration(new AppRoleConfiguration());
            modelBuilder.ApplyConfiguration(new AppUserConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryTranslationConfiguration());
            modelBuilder.ApplyConfiguration(new ContactConfiguration());
            modelBuilder.ApplyConfiguration(new BranchesConfiguration());
            modelBuilder.ApplyConfiguration(new ImageConfiguration());
            modelBuilder.ApplyConfiguration(new LanguageConfiguration());
            modelBuilder.ApplyConfiguration(new LoggerConfiguration());
            modelBuilder.ApplyConfiguration(new ArticlesConfiguration());
            modelBuilder.ApplyConfiguration(new SlideConfiguration());
            modelBuilder.ApplyConfiguration(new DoctorConfiguration());
            modelBuilder.ApplyConfiguration(new AppSettingfiguration());
            modelBuilder.ApplyConfiguration(new ActiveUserConfiguration());
            modelBuilder.ApplyConfiguration(new VisitorLogConfiguration());

            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);

            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => x.UserId);

            //Data seeding
            modelBuilder.Seed();
            //base.OnModelCreating(modelBuilder);
        }
        

        public DbSet<AppConfig> AppConfigs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Branches> Branches { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }    
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Image> Images { get; set; }    
        public DbSet<Language> Languages { get; set; }
        public DbSet<Logger> Loggers { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Slide> Slides { get; set;}
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<AppSetting> AppSettings { get; set; }
        public DbSet<ActiveUser> ActiveUsers { get; set; }
        public DbSet<VisitorLog> VisitorLogs { get; set; }

    }
}