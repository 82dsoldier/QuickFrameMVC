using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Server;
using Microsoft.VisualStudio.Web.CodeGeneration.DotNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using QuickFrame.Configuration;
using QuickFrame.Di;
using QuickFrame.Mapping;
using System;

namespace QuickFrame.Mvc {

	public class StartupCore {
		protected IConfigurationRoot Configuration;

		public StartupCore(IHostingEnvironment env) {
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

			if(env.IsDevelopment()) {
				// For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
				builder.AddUserSecrets();
			}

			builder.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		public virtual IServiceProvider ConfigureServices(IServiceCollection services) {
			services.AddEmbeddedFileProviders();
			services.AddMvc();
			services.AddOptions();
			services.AddSession();
			services.AddMvc().AddJsonOptions(options => {
				options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

				options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
				options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.All;
			});

			services.Configure<DataOptions>(dataConfig => {
				dataConfig.Load(Configuration);
			});

			services.Configure<ViewOptions>(viewOptions => {
				viewOptions.PerPageDefault = Configuration["ViewOptions:PerPageDefault"];
				foreach(var child in Configuration.GetSection("ViewOptions:PerPageList").GetChildren()) {
					viewOptions.PerPageList.Add(new SelectListItem {
						Value = child.Key,
						Text = child.Value,
						Selected = (viewOptions.PerPageDefault == child.Value)
					});
				}
			});

			services.AddMapping();
			services.AddAutofac();

			return ComponentContainer.ServiceProvider;
		}

		public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, ILibraryManager libraryManager) {
			var listener = app.ServerFeatures.Get<WebListener>();
			if(listener != null)
				listener.AuthenticationManager.AuthenticationSchemes = AuthenticationSchemes.NTLM;

			if(env.IsDevelopment())
				app.UseDeveloperExceptionPage();
			else
				app.UseExceptionHandler("/Home/Error");

			app.UseStaticFiles();

			app.UseSession();

			app.UseMvc(routes => {
				routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
				routes.MapRoute("defaultArea", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}