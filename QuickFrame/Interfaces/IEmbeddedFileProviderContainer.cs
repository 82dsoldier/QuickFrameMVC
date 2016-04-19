using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.FileProviders;

namespace QuickFrame.Interfaces
{
    public interface IEmbeddedFileProviderContainer
    {
		EmbeddedFileProvider FileProvider { get; }
	}
}
