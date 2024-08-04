using GTBack.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using GTBack.Core.DTO.Shopping;
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

        
        
        
             
        //Ecommerce
        public DbSet<EcommerceBasket> EcommerceBasket { get; set; }
        public DbSet<EcommerceBasketProductRelation> EcommerceBasketProductRelation { get; set; }
        public DbSet<EcommerceClient> EcommerceClient { get; set; }
        public DbSet<EcommerceClientFavoriteRelation> EcommerceClientFavoriteRelation { get; set; }
        public DbSet<EcommerceCompany> EcommerceCompany { get; set; }
        public DbSet<EcommerceEmployee> EcommerceEmployee { get; set; }
        public DbSet<EcommerceOrder> EcommerceOrder { get; set; }
        public DbSet<EcommerceProduct> EcommerceProduct { get; set; }
        public DbSet<EcommerceProductOrderRelation> EcommerceProductOrderRelation { get; set; }
        public DbSet<EcommerceRefreshToken> EcommerceRefreshToken { get; set; }
        public DbSet<EcommerceVariant> EcommerceVariant { get; set; }
        public DbSet<EcommerceImage> EcommerceImage { get; set; }
        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<EcommerceProductOrderRelation>()
                .HasKey(por => new { por.EcommerceProductId, por.EcommerceOrderId });

            modelBuilder.Entity<EcommerceBasketProductRelation>()
                .HasKey(bpr => new { bpr.EcommerceBasketId, bpr.EcommerceVariantId });

            modelBuilder.Entity<EcommerceClientFavoriteRelation>()
                .HasKey(cfr => new { cfr.EcommerceClientId, cfr.EcommerceProductId });

            base.OnModelCreating(modelBuilder);
            
      
        }
    }
}