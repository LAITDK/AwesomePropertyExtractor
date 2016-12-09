using APE.Umbraco.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace APE.Umbraco.Core.DTO
{
    [Serializable]
	public class ContentTypeDTO
	{
		public int Id { get; set; }
		public string ContentType { get; set; }
		public string ContentTypeAlias { get; set; }
		public string ContentTypeDescription { get; set; }
		public string ParentContentType { get; set; }
		public ICollection<ContentTypePropertyDTO> Properties { get; set; } = new List<ContentTypePropertyDTO>();
	}

    [Serializable]
    public class ContentTypePropertyDTO
	{
        public int DataTypeId { get; internal set; }
        public string EditorAlias { get; internal set; }
        public string PropertyAlias { get; set; }
		public string PropertyAliasText { get; set; }
		public string PropertyDescription { get; set; }
		public string PropertyType { get; set; }
    }
}