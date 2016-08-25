using Autofac.Extensions.DependencyInjection;
using ExpressMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using QuickFrame.Core.Services;
using QuickFrame.Data;
using QuickFrame.Data.Attachments;

//using static QuickFrame.Data.Attachments.Ui.ServiceExtensions;
using QuickFrame.Di;
using QuickFrame.Mvc;
using QuickFrame.Security;
using QuickFrame.Security.AccountControl.ActiveDirectory;
using System;
using static QuickFrame.Core.Services.Extensions;

namespace QuickFrame.Core.Mvc {

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
			services.AddOptions();

			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddTransient<IUrlHelper, UrlHelper>();

			services.AddDistributedMemoryCache();
			services.AddMemoryCache();
			services.AddSession(options => {
				options.IdleTimeout = TimeSpan.FromMinutes(30);
				options.CookieName = "ASMT";
			});

			services.AddMvc().AddJsonOptions(options => {
				options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

				options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
				options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.All;
			});

			services.AddQuickFrameMvc(Configuration);

			services.AddApplicationAssemblies();

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
			} else
				app.UseExceptionHandler("/Home/Error");

			app.UseStatusCodePagesWithReExecute("/StatusCodes/StatusCode{0}");

			//app.UseQuickFrameAttachmentsUi();
			app.UseStaticFiles();
			app.UseEmbeddedFileProviders();
			app.UseSession();

			app.UseMvc(routes => {
				routes.MapRoute("defaultArea", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
				routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
			});

			Mapper.Compile();
		}
	}
}