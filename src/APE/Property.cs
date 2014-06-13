using APE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APE
{
	public abstract class Property : IProperty
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

		protected Property() { }

		public TProp As<TProp>()
			where TProp : IProperty, new()
		{
			return new TProp() { Alias = this.Alias };
		}

		/// <summary>
		/// Implicitly cast an object of this type to a string.
		/// </summary>
		/// <param name="prop">DocTypeProperty to cast.</param>
		/// <returns>The alias of the property</returns>
		public static implicit operator string(Property prop)
		{
			return prop.Alias;
		}
	}
}
