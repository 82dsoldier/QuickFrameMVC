
using QuickFrame.Data.Models.Configurations;

namespace QuickFrame.Data.Attachments.Models.Configurations
{

    // AllRules
    
    public class UploadRuleConfiguration : ConfigurationInt<UploadRule>
    {
        public UploadRuleConfiguration() : base()
        {
			Property(x => x.Name).HasColumnName(@"Name").IsRequired().HasColumnType("nvarchar").HasMaxLength(64);
			Property(x => x.Description).HasColumnName(@"Description").IsOptional().HasColumnType("nvarchar").HasMaxLength(2048);
			Property(x => x.Priority).HasColumnName(@"Priority").IsOptional().HasColumnType("int");
			Property(x => x.IsActive).HasColumnName(@"IsActive").IsRequired().HasColumnType("bit");
			Property(x => x.IsAllow).HasColumnName(@"IsAllow").IsRequired().HasColumnType("bit");
			Property(x => x.FileExtensionId).HasColumnName(@"FileExtensionId").IsOptional().HasColumnType("int");
			Property(x => x.FileHeaderPatternId).HasColumnName(@"FileHeaderPatternId").IsOptional().HasColumnType("int");
			Property(x => x.MimeTypeId).HasColumnName(@"MimeTypeId").IsOptional().HasColumnType("int");

			// Foreign keys
			HasOptional(a => a.FileExtension).WithMany(b => b.UploadRules).HasForeignKey(c => c.FileExtensionId).WillCascadeOnDelete(false); // FK__UploadRul__FileE__412EB0B6
			HasOptional(a => a.FileHeaderPattern).WithMany(b => b.UploadRules).HasForeignKey(c => c.FileHeaderPatternId).WillCascadeOnDelete(false); // FK__UploadRul__FileH__4222D4EF
			HasOptional(a => a.MimeType).WithMany(b => b.UploadRules).HasForeignKey(c => c.MimeTypeId).WillCascadeOnDelete(false); // FK__UploadRul__MimeT__4316F928
		}
	}

}
// </auto-generated>
