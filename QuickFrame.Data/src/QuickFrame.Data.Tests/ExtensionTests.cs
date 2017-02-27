using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using QuickFrame.Data;

namespace QuickFrame.Data.Tests
{
    public class ExtensionTests
    {
		[Fact]
		public void TestIsDeletedExtension() {
			var testList = new List<ClassForIsDeletedExtensionTest> {
				new ClassForIsDeletedExtensionTest { Id = 1, Name ="One", IsDeleted = false },
				new ClassForIsDeletedExtensionTest { Id = 2, Name ="Two", IsDeleted = true },
				new ClassForIsDeletedExtensionTest { Id = 3, Name ="Three", IsDeleted = false },
				new ClassForIsDeletedExtensionTest { Id = 1, Name ="Four", IsDeleted = true },
				new ClassForIsDeletedExtensionTest { Id = 3, Name ="Five", IsDeleted = false },
			};

			var query = (testList.AsQueryable()).IsDeleted(true).AsEnumerable();

			Assert.True(query.AsEnumerable().Count() == 2);
			var first = query.First();
			Assert.True(first.Id == 2);

			query = (testList.AsQueryable()).IsDeleted(false).AsEnumerable();

			Assert.True(query.AsEnumerable().Count() == 3);
			first = query.First();
			Assert.True(first.Id == 1);

		}
	}
}
