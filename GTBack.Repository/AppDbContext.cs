using GTBack.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using GTBack.Core.DTO.Shopping;
using GTBack.Core.Entities.Coach;
using GTBack.Core.Entities.Ecommerce;
using GTBack.Core.Entities.Restourant;
using GTBack.Core.Entities.SharedEntities;
using GTBack.Core.Entities.Shopping;


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

        //Shopping
        public DbSet<Image> Image { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ShoppingCompany> ShoppingCompany { get; set; }
        public DbSet<ShoppingOrder> ShoppingOrder { get; set; }
        public DbSet<ShoppingUser> ShoppingUser { get; set; }
        public DbSet<GlobalProductModel> GlobalProductModels { get; set; }


        //Coach
        public DbSet<Student> Students { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<SubLesson> SubLessons { get; set; }
        public DbSet<SubjectScheduleRelation> SubjectScheduleRelations { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Parent> Parents { get; set; }


        //Ecommerce
        public DbSet<EcommerceBasket> EcommerceBasket { get; set; }
        public DbSet<EcommerceBasketProductRelation> EcommerceBasketProductRelation { get; set; }
        public DbSet<EcommerceClient> EcommerceClient { get; set; }
        public DbSet<EcommerceClientFavoriteRelation> EcommerceClientFavoriteRelation { get; set; }
        public DbSet<EcommerceCompany> EcommerceCompany { get; set; }
        public DbSet<EcommerceEmployee> EcommerceEmployee { get; set; }
        public DbSet<EcommerceOrder> EcommerceOrder { get; set; }
        public DbSet<EcommerceProduct> EcommerceProduct { get; set; }
        public DbSet<EcommerceVariantOrderRelation> EcommerceVariantOrderRelation { get; set; }
        public DbSet<EcommerceRefreshToken> EcommerceRefreshToken { get; set; }
        public DbSet<EcommerceVariant> EcommerceVariant { get; set; }
        public DbSet<EcommerceImage> EcommerceImage { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<EcommerceVariantOrderRelation>()
       .HasKey(por => por.Id); // Set Id as the primary key

            modelBuilder.Entity<EcommerceBasketProductRelation>()
                .HasKey(bpr => new { bpr.EcommerceBasketId, bpr.EcommerceVariantId });

            modelBuilder.Entity<EcommerceClientFavoriteRelation>()
                .HasKey(cfr => new { cfr.EcommerceClientId, cfr.EcommerceProductId });


            modelBuilder.Entity<EcommerceVariantOrderRelation>()
           .HasKey(v => v.Id);
            // Öğrenci ve Koç ilişkisi
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Coach)
                .WithMany(c => c.Students)
                .HasForeignKey(s => s.CoachId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Parent)
                .WithOne(c => c.Student).HasForeignKey<Student>(s => s.ParentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Parent>()
                .HasOne(s => s.Student)
                .WithOne(c => c.Parent).HasForeignKey<Parent>(s => s.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Schedule ile Subject ve Student ilişkisi
            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Student)
                .WithMany(st => st.Schedules)
                .HasForeignKey(s => s.StudentId);

            // Subject -> SubjectScheduleRelation (One-to-Many)
            modelBuilder.Entity<Subject>()
                .HasMany(s => s.SubjectScheduleRelations)
                .WithOne(ssr => ssr.Subject)
                .HasForeignKey(ssr => ssr.SubjectId)
                .OnDelete(DeleteBehavior.Cascade); // If a Subject is deleted, delete related SubjectScheduleRelations

            // Schedule -> SubjectScheduleRelation (One-to-Many)
            modelBuilder.Entity<Schedule>()
                .HasMany(s => s.SubjectScheduleRelations)
                .WithOne(ssr => ssr.Schedule)
                .HasForeignKey(ssr => ssr.ScheduleId)
                .OnDelete(DeleteBehavior.Cascade); // If a Schedule is deleted, delete related SubjectScheduleRelations





        }
    }
}