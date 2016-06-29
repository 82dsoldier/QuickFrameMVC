using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using QuickFrame.Configuration;
using QuickFrame.Di;
using QuickFrame.Mapping;
using QuickFrame.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

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
				//builder.AddUserSecrets();
			}

			builder.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		public virtual IServiceProvider ConfigureServices(IServiceCollection services) {


			//bool useFileProvider = true;
			//Boolean.TryParse(Configuration["AppOptions:UseEmbeddedFileProviders"], out useFileProvider);

			//if(useFileProvider)
			services.AddEmbeddedFileProviders();
			//services.AddMapping();
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

			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			ComponentContainer.Builder.Populate(services);

			return ComponentContainer.ServiceProvider;
		}

		public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
			var listener = app.ServerFeatures.Get<WebListener>();
			if(listener != null)
				listener.AuthenticationManager.AuthenticationSchemes = AuthenticationSchemes.NTLM;

			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			if(env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
				app.UseBrowserLink();
			} else
				app.UseExceptionHandler("/Home/Error");

			app.UseStaticFiles();

			app.UseSession();

			app.UseMvc(routes => {
				routes.MapRoute("defaultArea", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
				routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
			});
		}

		protected void RegisterAssembly(Type assemblyType) {
			var assembly = assemblyType.Assembly;
			var module = assembly.GetTypes().FirstOrDefault(t => typeof(Autofac.Module).IsAssignableFrom(t));

			if(module != null)
				ComponentContainer.Builder.RegisterAssemblyModules(assembly);
			else
				ComponentContainer.RegisterAssembly(assembly);

			foreach(var mod in assembly.GetTypes().Where(t => t.GetTypeInfo().GetCustomAttribute<ExpressMapAttribute>() != null)) {
				MapRegistration.Register(mod);
			}
		}
	}
}