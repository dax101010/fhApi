using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace fhApi.Models
{
	public class DocumentModel { 
	[Required]
		public string FileName { get; set; }
	[Required]
		public int[] Content { get; set; }
	[Required]
		public string MimeType { get; set; }
}
}
