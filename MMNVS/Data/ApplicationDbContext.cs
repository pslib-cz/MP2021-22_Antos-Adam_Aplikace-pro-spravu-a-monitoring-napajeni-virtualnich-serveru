using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MMNVS.Model;

namespace MMNVS.Data
{
    public class ApplicationDbContext : IdentityDbContext<MyUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Datastore>? Datastores { get; set; }
        public virtual DbSet<HostServer>? HostServers { get; set; }
        public virtual DbSet<LogItem>? Log { get; set; }
        public virtual DbSet<UPS>? UPS { get; set; }
        public virtual DbSet<UPSLogItem>? UPSLog { get; set; }
        public virtual DbSet<VirtualServer>? VirtualServers { get; set; }
        public virtual DbSet<VirtualStorageServer>? VirtualStorageServers { get; set; }
        public virtual DbSet<AppSettings>? Settings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var hasher = new PasswordHasher<MyUser>();
            modelBuilder.Entity<MyUser>().HasData(
                new MyUser
                {
                    UserName = "administrator",
                    NormalizedUserName = "ADMINISTRATOR",
                    PasswordHash = hasher.HashPassword(null, "MMNVSadmin"),
                }
            ); 
        }
    }
}