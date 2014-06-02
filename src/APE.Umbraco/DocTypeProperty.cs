using System;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace APE.Umbraco
{
	public class DocTypeProperty<TType> : DocTypeProperty
	{
		public virtual TType Map(IPublishedContent content, bool recursive = false)
		{
			return content.GetPropertyValue<TType>(this.Alias, recursive, default(TType));
		}

		public static implicit operator string(DocTypeProperty<TType> type)
		{
			return type.Alias;
		}

	}

	public abstract class DocTypeProperty
	{
		private string _alias;

		/// <summary>
		/// Gets the alias of the property.
		/// </summary>
		public string Alias
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_alias))
				{
					throw new Exception("Alias has not been set! Unable to get property value with no alias!");
				}
				return _alias;
			}
			set
			{
				if (string.IsNullOrWhiteSpace(_alias))
				{
					_alias = value;
				}
				else
				{
					throw new Exception("Alias has already been set!");
				}
			}
		}

		protected DocTypeProperty() { }

		public TProp As<TProp>()
			where TProp : DocTypeProperty, new()
		{
			return new TProp() { Alias = this.Alias };
		}

		/// <summary>
		/// Implicitly cast an object of this type to a string.
		/// </summary>
		/// <param name="prop">DocTypeProperty to cast.</param>
		/// <returns>The alias of the property</returns>
		public static implicit operator string(DocTypeProperty prop)
		{
			return prop.Alias;
		}

	}
}