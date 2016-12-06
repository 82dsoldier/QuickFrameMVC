using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace QuickFrame.Utilities
{
    public class PdfDocument
    {
		public byte[] ToPdf(string fileName) {
			var doc = new Aspose.Words.Document(FilePath);
			using(var ms = new MemoryStream()) {
				doc.Save(ms, Aspose.Words.SaveFormat.Pdf);
				return ms.ToArray();
			}
		}

		static PdfDocument() {
			using(var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("QuickFrame.Resources.Aspose.Words.lic")) {
				new Aspose.Words.License().SetLicense(resourceStream);
			}
		}
	}
}
