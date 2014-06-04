using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APE.Umbraco
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class UmbracoId : Attribute
    {
        public string Id { get; private set; }
        public UmbracoId(string id)
        {
            this.Id = id;
        }
    }
}
