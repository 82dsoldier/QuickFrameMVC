using Microsoft.AspNet.Http;
using Microsoft.Extensions.OptionsModel;
using QuickFrame.Attachments.Data.Models;
using QuickFrame.Configuration;
using QuickFrame.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Attachments.Data
{
    public class AttachmentsContext : TrackingContext
    {
		public static bool UseFilestream { get; set; } = false;
		public AttachmentsContext(IOptions<DataOptions> dataOptions, IHttpContextAccessor contextAccessor)
			:base(dataOptions.Value.ConnectionString["AttachmentsConnection"], contextAccessor) {
			UseFilestream = dataOptions.Value.UseFilestream;
		}
		public DbSet<Attachment> Attachments { get; set; }
		public DbSet<MimeType> MimeTypes { get; set; }
		public DbSet<MimeTypeRule> MimeTypeRules { get; set; }
		public DbSet<FileHeaderRule> FileHeaderRules { get; set; }
		protected override void OnModelCreating(DbModelBuilder modelBuilder) {
			modelBuilder.Entity<Attachment>()
				.HasRequired(att => att.MimeType)
				.WithMany(mt => mt.Attachments)
				.HasForeignKey(att => att.MimeTypeId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<MimeTypeRule>()
				.HasRequired(val => val.MimeType)
				.WithMany(mt => mt.MimeTypeValidations)
				.HasForeignKey(val => val.MimeTypeId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<FileHeaderRule>()
				.HasOptional(fh => fh.MimeType)
				.WithMany(mt => mt.FileHeaderRules)
				.HasForeignKey(fh => fh.MimeTypeId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<FileHeaderRule>()
				.Property(fh => fh.FileHeader)
				.HasMaxLength(1024);
		}
	}
}
