﻿using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using QuickFrame.Di;
using QuickFrame.Mvc;
using System.Reflection;

namespace QuickFrame.Data.Attachments.Ui {

	public static class ServiceExtensions {
		private static ContainerBuilder _builder => ComponentContainer.Builder;

		public static IServiceCollection AddQuickFrameAttachmentsUi(this IServiceCollection collection) {
			collection.Configure<RazorViewEngineOptions>(options => {
				options.FileProviders.Add(new EmbeddedFileProvider(
					typeof(EmbeddedFileProviderContainer).GetTypeInfo().Assembly,
					"QuickFrame.Data.Attachments.Ui"));
			});

			return collection;
		}

		public static IApplicationBuilder UseQuickFrameAttachmentsUi(this IApplicationBuilder app) {
			IOptions<RazorViewEngineOptions> razorViewEngineOptions =
				app.ApplicationServices.GetService<IOptions<RazorViewEngineOptions>>();
			razorViewEngineOptions.Value.FileProviders.Add(new EmbeddedFileProvider(
					typeof(EmbeddedFileProviderContainer).GetTypeInfo().Assembly,
					"QuickFrame.Data.Attachments.Ui"));
			return app;
		}
	}
}