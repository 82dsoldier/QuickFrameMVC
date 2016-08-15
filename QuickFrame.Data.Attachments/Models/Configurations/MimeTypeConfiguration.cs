using QuickFrame.Data.Models.Configurations;

namespace QuickFrame.Data.Attachments.Models.Configurations {
	// MimeTypes

	public class MimeTypeConfiguration : ConfigurationInt<MimeType> {

		public MimeTypeConfiguration() : base() {
			Property(x => x.Name).HasColumnName(@"Name").IsRequired().HasColumnType("nvarchar").HasMaxLength(64);
			Property(x => x.Description).HasColumnName(@"Description").IsOptional().HasColumnType("nvarchar").HasMaxLength(2048);
			Property(x => x.MimeTypeIdentifier).HasColumnName(@"MimeTypeIdentifier").IsRequired().HasColumnType("nvarchar").HasMaxLength(128);
		}
	}
}

// </auto-generated>