using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APE.Umbraco.Core.DTO
{
	public class DocTypeDTO
	{
		public string PropertyAlias { get; set; }
		public string PropertyAliasText { get; set; }
		public string DocType { get; set; }
		public string DocTypeAlias { get; set; }
		public string ParentDocType { get; set; }
		public string PropertyDescription { get; set; }
		public string PropertyType { get; set; }
		public string PropertyTypeAlias { get; set; }
	}
}
