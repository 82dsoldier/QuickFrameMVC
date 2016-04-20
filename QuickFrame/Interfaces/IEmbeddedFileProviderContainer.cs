using Microsoft.AspNet.FileProviders;

namespace QuickFrame.Interfaces {

	public interface IEmbeddedFileProviderContainer {
		EmbeddedFileProvider FileProvider { get; }
	}
}