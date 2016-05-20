using QuickFrame.Attachments.Data.Interfaces;
using QuickFrame.Attachments.Data.Models;
using QuickFrame.Data;
using QuickFrame.Di;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Attachments.Data.Services
{
	[Export]
    public class FileHeaderRulesDataService : DataService<AttachmentsContext, FileHeaderRule>, IFileHeaderRulesDataService
    {
		public bool IsFileAllowed(byte[] data, string fileExtension = "", int mimeTypeId = 0) {
			byte[] buffer = null;
			if(data.Length > 1024) {
				buffer = new byte[1024];
				Buffer.BlockCopy(data, 0, buffer, 0, 1024);
			} else {
				buffer = new byte[data.Length];
				Buffer.BlockCopy(data, 0, buffer, 0, data.Length);
			}

			using (var contextFactory = ComponentContainer.Component<AttachmentsContext>()) {
				var rules = contextFactory.Component.FileHeaderRules.Where(fh => fh.FileHeader == buffer && fh.IsAllowed == false);

				foreach(var rule in rules) {
					if(rule.MustMatchExtension) {
						if (!fileExtension.Equals(rule.FileExtension, StringComparison.CurrentCultureIgnoreCase))
							return true;
					}

					if (rule.MustMatchMimeType) {
						if (mimeTypeId == 0) {
							if (!fileExtension.Equals(rule.MimeType.FileExtension, StringComparison.CurrentCultureIgnoreCase))
								return true;
						} else {
							if (mimeTypeId != rule.MimeTypeId)
								return true;
						}
					}
				}

				rules = contextFactory.Component.FileHeaderRules.Where(fh => fh.FileHeader == buffer && fh.IsAllowed == true);

				foreach(var rule in rules) {
					if (rule.MustMatchExtension) {
						if (!fileExtension.Equals(rule.FileExtension, StringComparison.CurrentCultureIgnoreCase))
							return false;
					}

					if (rule.MustMatchMimeType) {
						if (mimeTypeId == 0) {
							if (!fileExtension.Equals(rule.MimeType.FileExtension, StringComparison.CurrentCultureIgnoreCase))
								return false;
						} else {
							if (mimeTypeId != rule.MimeTypeId)
								return false;
						}
					}
				}

				var defaultRule = contextFactory.Component.FileHeaderRules.First(fh => fh.FileExtension == "*");

				if (defaultRule == null)
					return false;

				return defaultRule.IsAllowed;
			}
		}
	}
}