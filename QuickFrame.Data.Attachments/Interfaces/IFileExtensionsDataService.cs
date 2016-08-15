﻿using QuickFrame.Data.Attachments.Models;
using QuickFrame.Data.Interfaces;

namespace QuickFrame.Data.Attachments.Interfaces {

	public interface IFileExtensionsDataService : IDataService<FileExtension> {

		bool FilExtensionExists(int id, string name);
	}
}