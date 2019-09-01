using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Swashbuckle.AspNetCore.Swagger;

using recipi.Data;

namespace recipi
{
	public class Startup
	{
		private readonly ILogger _logger;
		public Startup(IConfiguration configuration, ILogger<Startup> logger)
		{
			Configuration = configuration;
			_logger = logger;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors(options =>
			{
				options.AddPolicy("EnableCors", builder => {
					builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
				});
			});

			var connectionString = Configuration["DB:ConnectionString"];
			services.AddDbContext<RecipiDbContext>(options => options.UseNpgsql(connectionString));

			services.AddMvc()
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
				.AddJsonOptions(
					options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
				);
			
			
			services.AddAuthentication(options => 
			{
				// options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				// options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
			})
				.AddCookie()
				.AddGoogle(ops => 
			{
				IConfigurationSection googleAuthConfig = Configuration.GetSection("Authentication:Google");
				ops.ClientId = googleAuthConfig["ClientId"];
				ops.ClientSecret = googleAuthConfig["ClientSecret"];
			});


			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Info { Title = "Recipi API", Version = "v1" });
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				// Apply database migrations automatically, but only when in non-prod.
				using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
				{
					var dbCtx = scope.ServiceProvider.GetService<RecipiDbContext>();
					dbCtx.Database.Migrate();
					if (SeedHelper.SeedRequired(dbCtx))
					{
						_logger.LogInformation("Seeding non-prod database");
						SeedHelper.SeedDummyData(dbCtx);
					}
				}
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Recipi API");
			});

			app.UseHttpsRedirection();
			app.UseCors("EnableCors");
			app.UseAuthentication();
			app.UseMvc();
		}
	}
}
