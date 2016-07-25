
using QuickFrame.Data.Models.Configurations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickFrame.Data.Attachments.Models.Configurations
{

    // Attachments
    
    public class AttachmentConfiguration : ConfigurationGuid<Attachment>
    {
        public AttachmentConfiguration() : base()
        {
			Property(x => x.Data).HasColumnName(@"Data").IsRequired().HasColumnType("varbinary");
			Property(x => x.FileName).HasColumnName(@"Name").IsRequired().HasColumnType("nvarchar").HasMaxLength(256);
			Property(x => x.DocumentId).HasColumnName(@"DocumentId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
			Property(x => x.Description).HasColumnName(@"Description").IsOptional().HasColumnType("nvarchar").HasMaxLength(2048);
			Property(x => x.ParentId).HasColumnName(@"ParentId").IsOptional().HasColumnType("uniqueidentifier");
			Property(x => x.PreviousId).HasColumnName(@"PreviousId").IsOptional().HasColumnType("uniqueidentifier");
			Property(x => x.UploadDate).HasColumnName(@"UploadDate").IsRequired().HasColumnType("datetime").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
			// Foreign keys
			HasOptional(a => a.Parent).WithMany(b => b.Children).HasForeignKey(c => c.ParentId).WillCascadeOnDelete(false); // FK__Attachmen__Paren__46E78A0C
		}
	}

}
// </auto-generated>
