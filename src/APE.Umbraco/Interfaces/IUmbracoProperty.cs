using APE.Interfaces;
using System;
using Umbraco.Core.Models;
namespace APE.Umbraco.Interfaces
{
	public interface IUmbracoProperty<TType> : IUmbracoProperty
	{
		TType Map(IPublishedContent content, bool recursive = false);
	}

	public interface IUmbracoProperty : IProperty
	{

	}
}
