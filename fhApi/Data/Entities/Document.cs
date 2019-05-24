using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fhApi.Data.Entities
{
	public class Document : DocumentWithoutContent
	{
		public int[] Content { get; set; }
	}
}
