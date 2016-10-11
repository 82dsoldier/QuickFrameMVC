using QuickFrame.Data.Models.Configurations;

namespace QuickFrame.Data.Attachments.Models.Configurations {

	public class FileExtensionConfiguration : NamedDataModelConfiguration<FileExtension> {

		public FileExtensionConfiguration() : base() {
			Property(x => x.Description).HasColumnName(@"Description").IsOptional().HasColumnType("nvarchar").HasMaxLength(2048);
			Property(x => x.Extension).HasColumnName(@"Extension").IsOptional().HasColumnType("nvarchar").HasMaxLength(32);
		}
	}
}