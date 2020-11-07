using DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccess
{
    public class FilmContext : DbContext
    {
        public DbSet<Film> Films { get; set; }
        public DbSet<TopRatedMovie> TopRatedMovies { get; set; }

        public FilmContext(DbContextOptions<FilmContext> options) : base(options)
        {
            //this.Database.SetCommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        public override int SaveChanges()
        {
            SetCreatedDateOrAndUpdatedDate();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            SetCreatedDateOrAndUpdatedDate();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetCreatedDateOrAndUpdatedDate();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            SetCreatedDateOrAndUpdatedDate();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public void SetCreatedDateOrAndUpdatedDate()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdatedDate = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
                }
            }
        }
    }
}
