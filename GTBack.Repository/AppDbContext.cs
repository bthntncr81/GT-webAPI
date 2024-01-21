using GTBack.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using GTBack.Core.Entities.Restourant;
using Company = GTBack.Core.Entities.Company;
using User = GTBack.Core.Entities.User;

namespace GTBack.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        //Shared Tables   
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<Currency> Currency { get; set; }

        //Randevu
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<SpecialAttributeRelation> SpecialAttributeRelations { get; set; }
        public DbSet<EventTypeCompanyRelation> EventTypeCompanyRelations { get; set; }
        public DbSet<FAQ> FAQs { get; set; }

        //Restourant
        public DbSet<Client> Client { get; set; }
        public DbSet<Addition> Addition { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Device> Device { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<EmployeeOrderRelation> EmployeeOrderRelation { get; set; }
        public DbSet<ExtraMenuItem> ExtraMenuItem { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<MenuItem> MenuItem { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Reservation> Reservation { get; set; }
        public DbSet<ShiftControl> ShiftControl { get; set; }
        public DbSet<Table> Table { get; set; }
        public DbSet<TableArea> TableArea { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Event>()
                .HasOne(m => m.AdminUser)
                .WithMany(t => t.AdminEvent)
                .HasForeignKey(m => m.AdminUserId);


            modelBuilder.Entity<Event>()
                .HasOne(m => m.ClientUser)
                .WithMany(t => t.ClientEvent)
                .HasForeignKey(m => m.ClientUserId);


            modelBuilder.Entity<SpecialAttributeRelation>()
                .HasKey(e => new { e.AdminUserId, e.ClientUserId });

            modelBuilder.Entity<SpecialAttributeRelation>()
                .HasOne(e => e.AdminUser)
                .WithMany(e => e.BlackListUserRelationsClient)
                .HasForeignKey(e => e.AdminUserId);

            modelBuilder.Entity<SpecialAttributeRelation>()
                .HasOne(e => e.ClientUser)
                .WithMany(e => e.BlackListUserRelationsAdmin)
                .HasForeignKey(e => e.ClientUserId);


            modelBuilder.Entity<FAQ>()
                .HasKey(e => new { e.SenderUserId, e.AnsweredUserId });

            modelBuilder.Entity<FAQ>()
                .HasOne(e => e.AnsweredUser)
                .WithMany(e => e.Faq)
                .HasForeignKey(e => e.AnsweredUserId);

            // modelBuilder.Entity<FAQ>()
            //     .HasOne(e => e.SenderUser)
            //     .WithMany(e => e.FAQ)
            //     .HasForeignKey(e => e.SenderUserId);

            modelBuilder.Entity<Company>()
                .HasMany(c => c.User)
                .WithOne(e => e.Company)
                .HasForeignKey(e => e.CompanyId)
                .IsRequired(false);

            modelBuilder.Entity<GTBack.Core.Entities.Restourant.RestoCompany>()
                .HasOne(a => a.Menu)
                .WithOne(a => a.RestoCompany)
                .HasForeignKey<Menu>(c => c.RestoCompanyId);
        }
    }
}