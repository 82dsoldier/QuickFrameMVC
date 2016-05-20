using Microsoft.AspNet.FileProviders;
using QuickFrame.Interfaces;
using System.Reflection;

namespace QuickFrame.Mvc {

	public class EmbeddedFileProviderContainer : IEmbeddedFileProviderContainer {

		public EmbeddedFileProvider FileProvider {
			get {
				return new EmbeddedFileProvider(
					typeof(EmbeddedFileProviderContainer).GetTypeInfo().Assembly,
					"QuickFrame.Attachments.Data");
			}
		}
	}
}