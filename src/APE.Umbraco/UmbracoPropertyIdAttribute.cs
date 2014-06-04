using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APE.Umbraco
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class UmbracoPropertyIdAttribute : Attribute
    {
        public string Id { get; private set; }
        public UmbracoPropertyIdAttribute(string id)
        {
            this.Id = id;
        }
    }
}
