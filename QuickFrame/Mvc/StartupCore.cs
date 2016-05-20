using Microsoft.AspNet.Builder;
using Microsoft.AspNet.FileProviders;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http.Features;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Net.Http.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using QuickFrame.Configuration;
using QuickFrame.Di;
using QuickFrame.Mapping;
using System;
using System.Collections.Generic;
using System.Web.WebPages.Html;
using static QuickFrame.Extensions;

namespace QuickFrame.Mvc {

	public class StartupCore {
		protected IConfigurationRoot Configuration;

		public StartupCore(IHostingEnvironment env) {
			var builder = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		public virtual IServiceProvider ConfigureServices(IServiceCollection services) {
			services.AddEmbeddedFileProviders();
			services.AddMvc();
			services.AddOptions();
			services.AddSession();
			services.AddCaching();
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
				foreach (var child in Configuration.GetSection("ViewOptions:PerPageList").GetChildren()) {
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

		public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, ILibraryManager libraryManager, IAssemblyLoadContextAccessor assemblyLoadContextAccessor) {
			var listener = app.ServerFeatures.Get<WebListener>();
			if (listener != null)
				listener.AuthenticationManager.AuthenticationSchemes = AuthenticationSchemes.NTLM;

			if (env.IsDevelopment())
				app.UseDeveloperExceptionPage();
			else
				app.UseExceptionHandler("/Home/Error");


			app.UseIISPlatformHandler();

			app.UseStaticFiles();

			app.UseSession();

			//app.UseEmbeddedFileProviders(assemblyLoadContextAccessor, libraryManager);

			app.UseMvc(routes => {
				routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
				routes.MapRoute("defaultArea", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}