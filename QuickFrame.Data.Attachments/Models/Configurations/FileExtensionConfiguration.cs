using QuickFrame.Data.Models.Configurations;

namespace QuickFrame.Data.Attachments.Models.Configurations {

    public class FileExtensionConfiguration : ConfigurationInt<FileExtension>
    {
        public FileExtensionConfiguration() : base()
        {
			Property(x => x.Name).HasColumnName(@"Name").IsRequired().HasColumnType("nvarchar").HasMaxLength(64);
			Property(x => x.Description).HasColumnName(@"Description").IsOptional().HasColumnType("nvarchar").HasMaxLength(2048);
			Property(x => x.Extension).HasColumnName(@"Extension").IsOptional().HasColumnType("nvarchar").HasMaxLength(32);
		}
	}

}
