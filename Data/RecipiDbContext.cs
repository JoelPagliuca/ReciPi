using Microsoft.EntityFrameworkCore;

using recipi.Models;

namespace recipi.Data
{
	public class RecipiDbContext : DbContext
	{
		public RecipiDbContext(DbContextOptions<RecipiDbContext> options) : base(options) {}
		public DbSet<Recipe> Recipes { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			// https://medium.com/front-end-weekly/net-core-web-api-with-docker-compose-postgresql-and-ef-core-21f47351224f ?
		}
	}
}