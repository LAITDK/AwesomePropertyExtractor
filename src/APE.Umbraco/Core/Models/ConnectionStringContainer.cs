using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APE.Umbraco.Core.Models
{
    [Serializable]
    public class ConnectionStringContainer
    {
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }
        public string DataDir { get; set; }
    }
}
