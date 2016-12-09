using APE.Umbraco.Core.Interfaces;
using APE.Umbraco.Core.Models;
using System;
using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace APE.Umbraco
{
	public class DocTypeProperty<TType> : DocTypeProperty, IDocTypeProperty<TType>
	{
		public virtual TType Map(IPublishedContent content, bool recursive = false)
		{
			return content.GetPropertyValue<TType>(this.Alias, recursive, default(TType));
        }
        public override Type GetValueType(IEnumerable<PropertyPreValue> preValues)
        {
            return this.GetType();
        }

    }

	public abstract class DocTypeProperty: IDocTypeProperty
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

        public abstract Type GetValueType(IEnumerable<PropertyPreValue> preValues);

    }
}