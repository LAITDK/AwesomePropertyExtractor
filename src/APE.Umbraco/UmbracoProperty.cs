using APE.Umbraco.Interfaces;
using System;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace APE.Umbraco
{
	public class UmbracoProperty<TType> : Property, IUmbracoProperty<TType>
	{
		public virtual TType Map(IPublishedContent content, bool recursive = false)
		{
			return content.GetPropertyValue<TType>(this.Alias, recursive);
		}

		//public static implicit operator string(DocTypeProperty<TType> type)
		//{
		//	return type.Alias;
		//}

	}
}