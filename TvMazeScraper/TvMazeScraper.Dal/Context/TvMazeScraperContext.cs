using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TvMazeScraper.Dal.Dto;

namespace TvMazeScraper.Dal.Context
{
    public class TvMazeScraperContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private const string DefaultSchema = "dbo";

        public TvMazeScraperContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<ShowDto> Shows { get; set; }

        public DbSet<CastDto> Cast { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ShowDto>().ToTable("Shows", DefaultSchema)
                .HasMany(o => o.Cast)
                .WithOne()
                .HasForeignKey(c => c.ShowId);

            modelBuilder.Entity<CastDto>().ToTable("Cast", DefaultSchema)
                .HasKey(c => new { c.Id, c.ShowId });
        }
    }
}
