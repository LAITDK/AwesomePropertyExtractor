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
    [UmbracoPropertyId("38B352C1-E9F8-4FD8-9324-9A2EAB06D97A")]
    [UmbracoPropertyId("UMBRACO.TRUEFALSE")]
    public class BooleanProperty : DocTypeProperty<bool>
	{

	}

	/// <summary>
	/// Represents a property of type IPublishedContent, containing content.
	/// </summary>
    [UmbracoPropertyId("158AA029-24ED-4948-939E-C3DA209E5FBA")]
    [UmbracoPropertyId("UMBRACO.CONTENTPICKERALIAS")]
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
    [UmbracoPropertyId("23E93522-3200-44E2-9F29-E61A6FCBB79A")]
    [UmbracoPropertyId("UMBRACO.DATE")]
    [UmbracoPropertyId("928639ED-9C73-4028-920C-1E55DBB68783")]
    [UmbracoPropertyId("UMBRACO.DATETIME")]
    public class DateTimeProperty : DocTypeProperty<DateTime>
	{

	}

	/// <summary>
	/// Represents a property of type IHtmlString.
	/// </summary>
	[UmbracoPropertyId("5E9B75AE-FACE-41C8-B47E-5F4B0FD82F83")] // Richtext editor
	[UmbracoPropertyId("60B7DABF-99CD-41EB-B8E9-4D2E669BBDE9")] // Simple editor
    [UmbracoPropertyId("UMBRACO.TINYMCEV3")]
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
    [UmbracoPropertyId("1413AFCB-D19A-4173-8E9A-68288D2A73B8")]
    [UmbracoPropertyId("UMBRACO.INTEGER")]
    [UmbracoPropertyId("A74EA9C9-8E18-4D2A-8CF6-73C6206C5DA6")]
    [UmbracoPropertyId("UMBRACO.DROPDOWN")]
    [UmbracoPropertyId("A52C7C1C-C330-476E-8605-D63D3B84B6A6")]
    [UmbracoPropertyId("UMBRACO.RADIOBUTTONLIST")]
    public class IntProperty : DocTypeProperty<int>
	{

	}

	/// <summary>
	/// Represents a property of type IPublishedContent, containing media.
	/// </summary>
    [UmbracoPropertyId("EAD69342-F06D-4253-83AC-28000225583B")]
    [UmbracoPropertyId("UMBRACO.MEDIAPICKER")]
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
    [UmbracoPropertyId("7E062C13-7C41-4AD9-B389-41D88AEEF87C")]
    [UmbracoPropertyId("UMBRACO.MULTINODETREEPICKER")]
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
    [UmbracoPropertyId("UMBRACO.MULTIPLEMEDIAPICKER")]
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
    [UmbracoPropertyId("67DB8357-EF57-493E-91AC-936D305E0F2A")]
    [UmbracoPropertyId("UMBRACO.TEXTBOXMULTIPLE")]
    public class StringProperty : DocTypeProperty<string>
	{

	}
}
