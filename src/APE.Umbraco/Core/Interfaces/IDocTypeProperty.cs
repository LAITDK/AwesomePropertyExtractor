using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace APE.Umbraco.Core.Interfaces
{
    public interface IDocTypeProperty
    {
        string Alias { get; set; }

        TProp As<TProp>()
            where TProp : DocTypeProperty, new();
    }

    public interface IDocTypeProperty<TType>: IDocTypeProperty
    {
        TType Map(IPublishedContent content, bool recursive = false);
    }
}
