using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace APE.Umbraco
{
	/// <summary>
	/// Represents a property of type Boolean.
	/// </summary>
	public class BooleanProperty : DocTypeProperty<bool>
	{

	}

	/// <summary>
	/// Represents a property of type IPublishedContent, containing content.
	/// </summary>
	public class ContentProperty : DocTypeProperty<IPublishedContent>
	{
		public override IPublishedContent Map(IPublishedContent content, bool recursive = false)
		{
			var value = content.GetPropertyValue<int>(Alias, recursive, 0);
			return UH.UmbracoHelper.TypedContent(value);
		}
	}

	/// <summary>
	/// Represents a property of type DateTime.
	/// </summary>
	public class DateTimeProperty : DocTypeProperty<DateTime>
	{

	}

	/// <summary>
	/// Represents a property of type IHtmlString.
	/// </summary>
	public class HtmlStringProperty : DocTypeProperty<IHtmlString>
	{
		public override IHtmlString Map(IPublishedContent content, bool recursive = false)
		{
			return new HtmlString(content.GetPropertyValue<string>(this.Alias, recursive, string.Empty));
		}
	}

	/// <summary>
	/// Represents a property of type Int32.
	/// </summary>
	public class IntProperty : DocTypeProperty<int>
	{

	}

	/// <summary>
	/// Represents a property of type IPublishedContent, containing media.
	/// </summary>
	public class MediaProperty : DocTypeProperty<IPublishedContent>
	{
		public override IPublishedContent Map(IPublishedContent content, bool recursive = false)
		{
			var value = content.GetPropertyValue<int>(Alias, recursive, 0);
			return UH.UmbracoHelper.TypedMedia(value);
		}
	}

	/// <summary>
	/// Represents a property of type IEnumerable of IPublishedContent, containing content.
	/// For a bit faster performance, use MultiContentProperty or MultiMediaProperty
	/// </summary>
	public class MultiNodeProperty : DocTypeProperty<IEnumerable<IPublishedContent>>
	{
		public override IEnumerable<IPublishedContent> Map(IPublishedContent content, bool recursive = false)
		{
			var prop = content.GetPropertyValue<string>(Alias, recursive, string.Empty);

			var returnStuff = Enumerable.Empty<IPublishedContent>();

			if (!string.IsNullOrWhiteSpace(prop))
			{
				var ids = prop.Split(',');
				returnStuff = UH.UmbracoHelper.TypedContent(ids);

				if (!returnStuff.Any())
				{
					returnStuff = UH.UmbracoHelper.TypedMedia(ids);
				}
			}

			return returnStuff;
		}
	}


	/// <summary>
	/// Represents a property of type IEnumerable of IPublishedContent, containing content.
	/// </summary>
	public class MultiContentProperty : DocTypeProperty<IEnumerable<IPublishedContent>>
	{
		public override IEnumerable<IPublishedContent> Map(IPublishedContent content, bool recursive = false)
		{
			var prop = content.GetPropertyValue<string>(Alias, recursive, string.Empty);
			return string.IsNullOrWhiteSpace(prop)
				? Enumerable.Empty<IPublishedContent>()
				: UH.UmbracoHelper.TypedContent(prop.Split(','));
		}
	}

	/// <summary>
	/// Represents a property of type IEnumerable of IPublishedContent, containing media.
	/// </summary>
	public class MultiMediaProperty : DocTypeProperty<IEnumerable<IPublishedContent>>
	{
		public override IEnumerable<IPublishedContent> Map(IPublishedContent content, bool recursive = false)
		{
			var prop = content.GetPropertyValue<string>(Alias, recursive, string.Empty);
			return string.IsNullOrWhiteSpace(prop)
				? Enumerable.Empty<IPublishedContent>()
				: UH.UmbracoHelper.TypedMedia(prop.Split(','));
		}
	}

	/// <summary>
	/// Represents a property of type String.
	/// </summary>
	public class StringProperty : DocTypeProperty<string>
	{

	}
}
