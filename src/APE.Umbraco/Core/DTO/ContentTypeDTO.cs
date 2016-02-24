using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APE.Umbraco.Core.DTO
{
	public class ContentTypeDTO
	{
		public int Id { get; set; }
		public string ContentType { get; set; }
		public string ContentTypeAlias { get; set; }
		public string ContentTypeDescription { get; set; }
		public string ParentContentType { get; set; }
		public ICollection<ContentTypePropertyDTO> Properties { get; set; } = new List<ContentTypePropertyDTO>();
	}

	public class ContentTypePropertyDTO
	{
		public string PropertyAlias { get; set; }
		public string PropertyAliasText { get; set; }
		public string PropertyDescription { get; set; }
		public string PropertyType { get; set; }
		public string PropertyTypeAlias { get; set; }
	}
}