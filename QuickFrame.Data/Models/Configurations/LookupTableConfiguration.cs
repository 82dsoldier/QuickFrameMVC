using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data.Models.Configurations
{
	public class LookupTableConfiguration<TModel> :
		ConfigurationInt<TModel>
		where TModel : LookupTable
    {
		public LookupTableConfiguration(string tableName) : base() {
			Property(x => x.Name).HasColumnName(@"Name")
				.HasColumnType("nvarchar")
				.IsRequired()
				.HasMaxLength(256)
				.HasColumnAnnotation(IndexAnnotation.AnnotationName, 
				new IndexAnnotation(
					new IndexAttribute($"UQ_{tableName}_Name") { IsUnique = true, IsClustered = false }));
		}
    }
}
