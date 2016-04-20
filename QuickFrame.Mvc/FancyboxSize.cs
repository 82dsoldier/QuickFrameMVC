using System.Collections.Generic;

namespace QuickFrame.Mvc {

	public enum FancyBoxSize {
		None,
		XSmall,
		LetterBoxXSmall,
		Small,
		LetterBoxSmall,
		Medium,
		LetterBoxMedium,
		Large,
		LetterBoxLarge
	}

	internal static class FancyBoxSizeInfo {

		internal static Dictionary<FancyBoxSize, FbPoint> FancyBoxSizes = new Dictionary<FancyBoxSize, FbPoint> {
			{ FancyBoxSize.XSmall, new FbPoint { Width = "640px", Height = "480px" } },
			{ FancyBoxSize.LetterBoxXSmall, new FbPoint { Width = "640px", Height = "240px" } },
			{ FancyBoxSize.Small, new FbPoint { Width = "800px", Height = "600px" } },
			{ FancyBoxSize.LetterBoxSmall, new FbPoint { Width = "800px", Height = "480px" } },
			{ FancyBoxSize.Medium, new FbPoint { Width = "1024px", Height = "768px" } },
			{ FancyBoxSize.LetterBoxMedium, new FbPoint { Width = "1024px", Height = "600px" } },
			{ FancyBoxSize.Large, new FbPoint { Width = "1280px", Height = "1024px" } },
			{ FancyBoxSize.LetterBoxLarge, new FbPoint { Width = "1280px", Height = "768px" } }
		};
	}

	internal struct FbPoint {
		internal string Width { get; set; }
		internal string Height { get; set; }
	}
}