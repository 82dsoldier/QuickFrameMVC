using QuickFrame.Data.Models.Configurations;

namespace QuickFrame.Data.Attachments.Models.Configurations {
	// FileHeaderPatterns

	public class FileHeaderPatternConfiguration : NamedDataModelConfiguration<FileHeaderPattern> {

		public FileHeaderPatternConfiguration() : base() {
			Property(x => x.Description).HasColumnName(@"Description").IsOptional().HasColumnType("nvarchar").HasMaxLength(2048);
			Property(x => x.Location).HasColumnName(@"Location").IsRequired().HasColumnType("bit");
			Property(x => x.Offset).HasColumnName(@"Offset").IsRequired().HasColumnType("int");
			Property(x => x.FileHeader).HasColumnName(@"FileHeader").IsRequired().HasColumnType("varbinary").HasMaxLength(2048);
		}
	}
}

// </auto-generated>