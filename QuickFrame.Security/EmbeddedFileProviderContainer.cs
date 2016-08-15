using Microsoft.Extensions.FileProviders;
using QuickFrame.Interfaces;
using System.Reflection;

namespace QuickFrame.Security {

	public class EmbeddedFileProviderContainer : IEmbeddedFileProviderContainer {

		public EmbeddedFileProvider FileProvider
		{
			get
			{
				return new EmbeddedFileProvider(
					typeof(EmbeddedFileProviderContainer).GetTypeInfo().Assembly,
					"QuickFrame.Security");
			}
		}
	}
}