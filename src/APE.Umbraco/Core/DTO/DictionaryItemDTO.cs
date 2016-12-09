using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APE.Umbraco.Core.DTO
{
    [Serializable]
    public class DictionaryItemDTO
	{
		public string Alias { get; set; }
		public string Name { get; set; }
	}
}