using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace fhApi.Data
{
	public static class MimeTypeFileExtension
	{
		public static string GetFileExtensionFromMimeType(string mimeType)
		{
			return MimeTypeMap.List.MimeTypeMap.GetExtension(mimeType).First<string>();


		}
	}
}
