using APE.Umbraco.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public int DataTypeId { get; internal set; }
        public string PropertyAlias { get; set; }
		public string PropertyAliasText { get; set; }
		public string PropertyDescription { get; set; }
		public string PropertyType { get; set; }
		public string PropertyTypeAlias { get; set; }


        internal Type Type { get; set; }

        public string GetValueTypeName(IEnumerable<PreValue> preValues)
        {
            var type = Type;
            var getTypeMethod = this.Type.GetMethod("GetValueType");
            if (getTypeMethod != null)
            {
                type = (Type)getTypeMethod.Invoke(null, new object[] { preValues });
            }

            return Helpers.CSharpName(type);
        }
    }
}