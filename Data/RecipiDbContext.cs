using Microsoft.EntityFrameworkCore;

using recipi.Models;

namespace recipi.Data
{
	public class RecipiDbContext : DbContext
	{
		public RecipiDbContext(DbContextOptions<RecipiDbContext> options) : base(options) {}
		public DbSet<Recipe> Recipes { get; set; }
		public DbSet<Step> Steps { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			
			modelBuilder.Entity<Step>()
				.HasKey(s => new { s.Number, s.RecipeId });
			modelBuilder.Entity<Step>()
				.HasOne(s => s.Recipe)
				.WithMany(r => r.Steps)
				.OnDelete(DeleteBehavior.Cascade);

			// https://medium.com/front-end-weekly/net-core-web-api-with-docker-compose-postgresql-and-ef-core-21f47351224f ?
		}
	}
}