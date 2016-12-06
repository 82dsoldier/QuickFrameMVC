using ExpressMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace QuickFrame.Mvc {

	/// <summary>
	/// Provides a base startup file for .net core web projects. Initializes most-used objects and services.
	/// </summary>
	public class StartupCore {

		/// <summary>
		/// An <see
		/// cref="Microsoft.Extensions.Configuration.IConfigurationRoot">IConfigurationRoot</see>
		/// object used to store configuration settings from appsettings.json.
		/// </summary>
		protected IConfigurationRoot Configuration;
		protected IConfigurationBuilder ConfigurationBuilder;
		/// <summary>
		/// Constructor for the StartupCore class
		/// </summary>
		/// <param name="env">
		/// An <see cref="Microsoft.AspNetCore.Hosting.IHostingEnvironment">IHostingEnvironment</see>
		/// that provides startup options and configuration information.
		/// </param>
		public StartupCore(IHostingEnvironment env) {
			ConfigurationBuilder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

			ConfigurationBuilder.AddEnvironmentVariables();
			Configuration = ConfigurationBuilder.Build();
		}

		/// <summary>
		/// Called to configure necessary services before application start.
		/// </summary>
		/// <param name="services">The collection of services to be loaded for this application.</param>
		/// <returns>
		/// An <see cref="System.IServiceProvider">IServiceProvider</see>. This return value is
		/// necessary if using a third-party dependency injection container.
		/// </returns>
		public virtual IServiceProvider ConfigureServices(IServiceCollection services) {
			services
				.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
				//.AddTransient<IUrlHelper, UrlHelper>()
				.AddSingleton<IActionContextAccessor, ActionContextAccessor>()
				.AddSingleton<IUrlHelperFactory, UrlHelperFactory>()
				.AddMvc()
				.AddJsonOptions(options => {
					options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
					options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
					options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.All;
				});

			bool useSession = false;
			Boolean.TryParse(Configuration["ServiceOptions:useSession"], out useSession);

			if(useSession) {
				int sessionTimeout = 15;
				Int32.TryParse(Configuration["ServiceOptions:sessionTimeout"], out sessionTimeout);

				services.AddDistributedMemoryCache()
					.AddMemoryCache()
					.AddSession(options => {
						options.IdleTimeout = TimeSpan.FromMinutes(sessionTimeout);
						options.CookieName = Configuration["ServiceOptions:cookieName"];
					});
			}

			return services.BuildServiceProvider();
		}

		/// <summary>
		/// Called to configure the application before startup.
		/// </summary>
		/// <param name="app">An <see cref="Microsoft.AspNetCore.Builder.IApplicationBuilder">IApplicationBuilder</see></param>
		/// <param name="env">
		/// An <see cref="Microsoft.AspNetCore.Hosting.IHostingEnvironment">IHostingEnvironment</see>
		/// that provides startup options and configuration information.
		/// </param>
		/// <param name="loggerFactory">An <see cref="Microsoft.Extensions.Logging.ILoggerFactory"/></param>
		public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			if(env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			} else
				app.UseExceptionHandler("/Home/Error");

			//app.UseStatusCodePagesWithReExecute("/StatusCodes/StatusCode{0}");

			app.UseStaticFiles();

			app.UseSession();

			app.UseMvc(routes => {
				routes.MapRoute("defaultArea", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
				routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
			});

			Mapper.Compile();
		}
	}
}
