using HaberEcommerceSite.Data;
using HaberEcommerceSite.Data.Entities;
using HaberEcommerceSite.Interfaces;
using HaberEcommerceSite.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace HaberEcommerceSite
{
	public class Startup
	{
		private readonly IConfiguration configuration;

		public Startup(IConfiguration configuration)
		{
			this.configuration = configuration;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// Add the identity provider as a service.
			services.AddIdentity<User, Role>()
	
				.AddEntityFrameworkStores<HaberContext>()

				.AddDefaultTokenProviders();


            services.ConfigureApplicationCookie(o =>
            {
                o.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                o.SlidingExpiration = true;
            });

            // Register the given context as a service on the project.
            services.AddDbContext<HaberContext>(configuration => {

				configuration.UseSqlServer(this.configuration.GetConnectionString("HaberConnectionString"));

			});

            //Add SMTP Config
            services.Configure<SmtpConfig>(this.configuration.GetSection("Smtp"));

            // Add the database seeder as a service.
            services.AddScoped<HaberSeeder>();

			// Add the unit of work instance as a services to be served by dependency injection.
			services.AddScoped<IUnitOfWork, UnitOfWork>();

			// Add MVC services to the project.
			services.AddMvc();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder application, IHostingEnvironment environment)
		{
			// Set specific settings for the development environment.
			if (environment.IsDevelopment())
			{
				application.UseDeveloperExceptionPage();

				// Seed the database if is in development only.
				using (var scope = application.ApplicationServices.CreateScope())
				{
					var serviceProvider = scope.ServiceProvider;

					try
					{
						var contextSeeder = serviceProvider.GetService<HaberSeeder>();

						contextSeeder.Seed().Wait();
					}
					catch (Exception exception)
					{
						var logger = serviceProvider.GetService<ILogger<Startup>>();

						logger.LogError(exception, "An error occurred while seeding the database.");
					}

				}
			}

           

            application.UseStaticFiles();

			application.UseAuthentication();        


            application.UseMvc(configuration => {

				// Configure the routes for the Site Controller of the MVC application.
				configuration.MapRoute("Errepar", "Errepar", new { controller = "Site", action = "Errepar" });

                configuration.MapRoute("Erreius", "Erreius", new { controller = "Site", action = "Erreius" });

                configuration.MapRoute("Editorial", "Editorial", new { controller = "Site", action = "Editorial" });

                configuration.MapRoute("Contact", "Contact", new { controller = "Site", action = "Contact" });

                configuration.MapRoute("Shop", "Shop", new { controller = "Site", action = "Shop" });

				// Configure the routes for the Book Controller of the MVC application.
				configuration.MapRoute("Book/Create", "Book/Create", new { controller = "Book", action = "Create" });

                configuration.MapRoute("Book/List", "Book/List/{pageIndex}", new { controller = "Book", action = "List"});

                configuration.MapRoute("Book/Edit", "Book/Edit/{ID}", new { controller = "Book", action = "Edit" });

                configuration.MapRoute("Book/Delete", "Book/Delete/{ID}", new { controller = "Book", action = "Delete" });

                // Configure the routes for the Author Controller of the MVC application.
                configuration.MapRoute("Author/Create", "Author/Create", new { controller = "Author", action = "Create" });

				// Configure the routes for the Account Controller of the MVC application.
				configuration.MapRoute("Login", "Login", new { controller = "Account", action = "Login" });

				configuration.MapRoute("Logout", "Logout", new { controller = "Account", action = "Logout" });
                
				configuration.MapRoute("Default", "{controller}/{action}", new { controller = "Site", action = "Index" });

                configuration.MapRoute("BookList", "Panel/{ID}", new { controller = "Book", action = "List", ID = 1 });

                configuration.MapRoute("PromoList", "Panel/{ID}", new { controller = "Promo", action = "List", ID = 1 });

            });

		}
	}
}
