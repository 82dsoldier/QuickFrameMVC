using Microsoft.Extensions.FileProviders;

namespace QuickFrame.Interfaces {

	public interface IEmbeddedFileProviderContainer {
		EmbeddedFileProvider FileProvider { get; }
	}
}