using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using QuickFrame.Data.Attachments.Models;
using QuickFrame.Data.Attachments.Models.Configurations;
using QuickFrame.Security.Data;
using System.Data.Entity;

namespace QuickFrame.Data.Attachments {

	[System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.20.1.0")]
	public class AttachmentsContext : TrackingContext {
		public DbSet<UploadRule> UploadRules { get; set; } // AllRules
		public DbSet<Attachment> Attachments { get; set; } // Attachments
		public DbSet<FileExtension> FileExtensions { get; set; } // FileExtensions
		public DbSet<FileHeaderPattern> FileHeaderPatterns { get; set; } // FileHeaderPatterns
		public DbSet<MimeType> MimeTypes { get; set; } // MimeTypes

		public AttachmentsContext(IOptions<DataOptions> configOptions, IHttpContextAccessor contextAccessor)
			: base(configOptions.Value.ConnectionString.Attachments, contextAccessor) {
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder) {
			base.OnModelCreating(modelBuilder);

			modelBuilder.Configurations.Add(new UploadRuleConfiguration());
			modelBuilder.Configurations.Add(new AttachmentConfiguration());
			modelBuilder.Configurations.Add(new FileExtensionConfiguration());
			modelBuilder.Configurations.Add(new FileHeaderPatternConfiguration());
			modelBuilder.Configurations.Add(new MimeTypeConfiguration());
		}
	}
}