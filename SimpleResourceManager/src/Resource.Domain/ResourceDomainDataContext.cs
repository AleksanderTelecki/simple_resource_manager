using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Resource.Domain.Entity;

namespace Resource.Domain
{
    public class ResourceDomainDataContext : IdentityDbContext<ReservationHolder>
    {
        public ResourceDomainDataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Entity.Resource> Resources { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Entity.Resource>()
                .HasOne(r => r.ReservationHolder)
                .WithMany(u => u.Resources)
                .HasForeignKey(r => r.ReservationHolderId);
        }
    }
}
