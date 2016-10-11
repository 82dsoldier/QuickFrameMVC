using Microsoft.Extensions.DependencyInjection;
using QuickFrame.Data.Attachments.Interfaces;
using QuickFrame.Data.Attachments.Services;
using QuickFrame.Data.Interfaces;
using System;
using System.Linq;
using System.Reflection;

namespace QuickFrame.Data.Attachments {

	public static class ServiceExtensions {

		public static IServiceCollection AddQuickFrameAttachments(this IServiceCollection services) {
			services.AddSingleton<AttachmentsContext>();
			services.AddTransient<IAttachmentsDataService, AttachmentsDataService>();
			services.AddTransient<IFileExtensionsDataService, FileExtensionsDataService>();
			services.AddTransient<IFileHeaderPatternsDataService, FileHeaderPatternsDataService>();
			services.AddTransient<IMimeTypesDataService, MimeTypesDataService>();
			services.AddTransient<IUploadRulesDataService, UploadRulesDataService>();


			return services;
		}
	}
}