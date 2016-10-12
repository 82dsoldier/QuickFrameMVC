using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Data.Validation
{
    public class IsValidUrlAttribute : ValidationAttribute
    {
		public override bool IsValid(object value) {
			Uri uri = null;
			var val = (value as string);
			if(val == null)
				return false;
			if(!Uri.TryCreate(val, UriKind.Absolute, out uri))
				return false;
			return true;
		}

		public override string FormatErrorMessage(string name) {
			return String.Format(CultureInfo.CurrentCulture, "Value is not a valid URL", name);
		}
	}
}
