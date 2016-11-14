using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;
using APE.Umbraco.Core.Interfaces;
using APE.Umbraco.Core.Models;

namespace APE.Umbraco
{
	/// <summary>
	/// Represents a property of type <see cref="bool"/>.
	/// </summary>
	[UmbracoPropertyId("38B352C1-E9F8-4FD8-9324-9A2EAB06D97A")]
	[UmbracoPropertyId("UMBRACO.TRUEFALSE")]
	public class BooleanProperty : DocTypeProperty<bool>
	{
	}

    /// <summary>
    /// Represents a property of type <see cref="int"/>.
    /// </summary>
    [UmbracoPropertyId("1413AFCB-D19A-4173-8E9A-68288D2A73B8")]
    [UmbracoPropertyId("UMBRACO.INTEGER")]
    [UmbracoPropertyId("A52C7C1C-C330-476E-8605-D63D3B84B6A6")]
    [UmbracoPropertyId("UMBRACO.RADIOBUTTONLIST")]
    [UmbracoPropertyId("29E790E6-26B3-438A-B21F-908663A0B19E")]
    [UmbracoPropertyId("UMBRACO.SLIDER")]
    [UmbracoPropertyId("UMBRACO.USERPICKER")]
    public class IntProperty : DocTypeProperty<int>
    {
    }

    /// <summary>
    /// Represents a property of type <see cref="string"/>.
    /// </summary>
    [UmbracoPropertyId("67DB8357-EF57-493E-91AC-936D305E0F2A")]
	[UmbracoPropertyId("UMBRACO.TEXTBOXMULTIPLE")]
	[UmbracoPropertyId("A74EA9C9-8E18-4D2A-8CF6-73C6206C5DA6")]
	[UmbracoPropertyId("UMBRACO.DROPDOWN")]
	public class StringProperty : DocTypeProperty<string>
	{
	}

	/// <summary>
	/// Represents a property of type <see cref="IHtmlString"/>.
	/// </summary>
	[UmbracoPropertyId("5E9B75AE-FACE-41C8-B47E-5F4B0FD82F83")] // Richtext editor
	[UmbracoPropertyId("60B7DABF-99CD-41EB-B8E9-4D2E669BBDE9")] // Simple editor
	[UmbracoPropertyId("UMBRACO.TINYMCEV3")]
	[UmbracoPropertyId("UMBRACO.MARKDOWNEDITOR")]
	public class HtmlStringProperty : DocTypeProperty<IHtmlString>
	{
		public override IHtmlString Map(IPublishedContent content, bool recursive = false)
		{
			return new HtmlString(content.GetPropertyValue<string>(this.Alias, recursive, string.Empty));
		}
	}

	/// <summary>
	/// Represents a property of type <see cref="DateTime"/>.
	/// </summary>
	[UmbracoPropertyId("23E93522-3200-44E2-9F29-E61A6FCBB79A")]
	[UmbracoPropertyId("UMBRACO.DATE")]
	[UmbracoPropertyId("B6FB1622-AFA5-4BBF-A3CC-D9672A442222")]
	[UmbracoPropertyId("UMBRACO.DATETIME")]
	public class DateTimeProperty : DocTypeProperty<DateTime>
	{
	}

	/// <summary>
	/// Represents a property of type <see cref="IEnumerable{}"/> of <see cref="string"/>.
	/// </summary>
	[UmbracoPropertyId("5359AD0B-06CC-4182-92BD-0A9117448D3F")]
	[UmbracoPropertyId("UMBRACO.MULTIPLETEXTSTRING")]
	public class EnumerableStringProperty : DocTypeProperty<IEnumerable<string>>
	{
	}

	/// <summary>
	/// Represents a property of type <see cref="IEnumerable{string}"/> generated from a comma separated <seealso cref="string"/>.
	/// </summary>
	[UmbracoPropertyId("B4471851-82B6-4C75-AFA4-39FA9C6A75E9")]
	[UmbracoPropertyId("UMBRACO.CHECKBOXLIST")]
	[UmbracoPropertyId("4023E540-92F5-11DD-AD8B-0800200C9A66")]
	[UmbracoPropertyId("UMBRACO.TAGS")]
	[UmbracoPropertyId("928639ED-9C73-4028-920C-1E55DBB68783")]
	[UmbracoPropertyId("UMBRACO.DROPDOWNMULTIPLE")]
	[UmbracoPropertyId("UMBRACO.MEMBERGROUPPICKER")]
	public class EnumerableCsvStringProperty : EnumerableStringProperty
	{
		public override IEnumerable<string> Map(IPublishedContent content, bool recursive = false)
		{
			var value = content.GetPropertyValue<string>(Alias, recursive, string.Empty);
			if (string.IsNullOrEmpty(value))
				return Enumerable.Empty<string>();

			return value.Split(',');
		}
	}

	/// <summary>
	/// Represents a property of type <see cref="IEnumerable{int}"/> generated from a comma separated <seealso cref="string"/>.
	/// </summary>
	[UmbracoPropertyId("928639AA-9C73-4028-920C-1E55DBB68783")]
	[UmbracoPropertyId("UMBRACO.DROPDOWNLISTMULTIPLEPUBLISHKEYS")]
	public class EnumerableCsvIntegerProperty : DocTypeProperty<IEnumerable<int>>
	{
		public override IEnumerable<int> Map(IPublishedContent content, bool recursive = false)
		{
			var value = content.GetPropertyValue<string>(Alias, recursive, string.Empty);
			if (string.IsNullOrEmpty(value))
				yield break;

			var vals = value.Split(',');
			int res;
			foreach (var val in vals)
			{
				if (int.TryParse(val, out res))
					yield return res;
			}
		}
	}

	/// <summary>
	/// Represents a property of type <see cref="IPublishedContent"/>, containing content.
	/// </summary>
	[UmbracoPropertyId("158AA029-24ED-4948-939E-C3DA209E5FBA")]
	[UmbracoPropertyId("UMBRACO.CONTENTPICKERALIAS")]
	public class ContentProperty : DocTypeProperty<IPublishedContent>
	{
		public override IPublishedContent Map(IPublishedContent content, bool recursive = false)
		{
			var value = content.GetPropertyValue<int>(Alias, recursive, 0);
			if (value == 0)
				return null;

			return UH.UmbracoHelper.TypedContent(value);
		}
	}

	/// <summary>
	/// Represents a property of type <see cref="IPublishedContent"/>, containing media.
	/// </summary>
	[UmbracoPropertyId("EAD69342-F06D-4253-83AC-28000225583B")]
	[UmbracoPropertyId("UMBRACO.MEDIAPICKER")]
	public class MediaProperty : DocTypeProperty<IPublishedContent>
	{
		public override IPublishedContent Map(IPublishedContent content, bool recursive = false)
		{
			var value = content.GetPropertyValue<int>(Alias, recursive, 0);
			if (value == 0)
				return null;

			return UH.UmbracoHelper.TypedMedia(value);
		}

    }

	/// <summary>
	/// Represents a property of type <see cref="IPublishedContent"/>, containing a member.
	/// </summary>
	[UmbracoPropertyId("UMBRACO.MEMBERPICKER")]
	public class MemberProperty : DocTypeProperty<IPublishedContent>
	{
		public override IPublishedContent Map(IPublishedContent content, bool recursive = false)
		{
			var value = content.GetPropertyValue<int>(this);

			return UH.UmbracoHelper.TypedMember(value);
		}
	}

	/// <summary>
	/// Represents a property of type <see cref="IEnumerable{}"/> of <see cref="IPublishedContent"/>, containing content.
	/// For a bit faster performance, use <see cref="MultiContentProperty"/>, <see cref="MultiMediaProperty"/> or <see cref="MultiMemberProperty"/>.
	/// </summary>
	[UmbracoPropertyId("7E062C13-7C41-4AD9-B389-41D88AEEF87C")]
	[UmbracoPropertyId("UMBRACO.MULTINODETREEPICKER")]
	public class MultiNodeProperty : DocTypeProperty<IEnumerable<IPublishedContent>>, IDocTypeProperty<IPublishedContent>
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
					returnStuff = UH.UmbracoHelper.TypedMedia(ids);

				if (!returnStuff.Any())
				{
					// There is currently no overload which takes a collection of ids for members.
					returnStuff = ids.Select(c => UH.UmbracoHelper.TypedMember(c));
				}
			}

			return returnStuff;
		}

        public static Type GetValueType(IEnumerable<PropertyPreValue> preValues)
        {                                        
            if (preValues.Any(pv => pv.Alias == "maxNumber" && pv.Value == "1"))
            {
            return typeof(IDocTypeProperty<IPublishedContent>);
            }
                return typeof(MultiNodeProperty);
        }

        IPublishedContent IDocTypeProperty<IPublishedContent>.Map(IPublishedContent content, bool recursive)
        {
            return this.Map(content, recursive).FirstOrDefault();
        }

    }

	/// <summary>
	/// Represents a property of type <see cref="IEnumerable{}"/> of <see cref="IPublishedContent"/>, containing content.
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
	/// Represents a property of type <see cref="IEnumerable{}"/> of <see cref="IPublishedContent"/>, containing media.
	/// </summary>
	[UmbracoPropertyId("UMBRACO.MULTIPLEMEDIAPICKER")]
	public class MultiMediaProperty : DocTypeProperty<IEnumerable<IPublishedContent>>, IDocTypeProperty<IPublishedContent>
    {
		public override IEnumerable<IPublishedContent> Map(IPublishedContent content, bool recursive = false)
		{
			var prop = content.GetPropertyValue<string>(Alias, recursive, string.Empty);
			return string.IsNullOrWhiteSpace(prop)
				? Enumerable.Empty<IPublishedContent>()
				: UH.UmbracoHelper.TypedMedia(prop.Split(','));
		}


        public static Type GetValueType(IEnumerable<PropertyPreValue> preValues)
        {
            if (preValues.Any(pv=>pv.Alias == "multiPicker" && pv.Value == "1"))
            {
                return typeof(MultiMediaProperty);
            }
            return typeof(IDocTypeProperty<IPublishedContent>);
        }

        IPublishedContent IDocTypeProperty<IPublishedContent>.Map(IPublishedContent content, bool recursive)
        {
           return this.Map(content, recursive).FirstOrDefault();
        }
    }

	/// <summary>
	/// Represents a property of type <see cref="IEnumerable{}"/> of <see cref="IPublishedContent"/>, containing members.
	/// </summary>
	public class MultiMemberProperty : DocTypeProperty<IEnumerable<IPublishedContent>>
	{
		public override IEnumerable<IPublishedContent> Map(IPublishedContent content, bool recursive = false)
		{
			var prop = content.GetPropertyValue<string>(Alias);
			return string.IsNullOrWhiteSpace(prop)
				? Enumerable.Empty<IPublishedContent>()
				: prop.Split(',').Select(p => UH.UmbracoHelper.TypedMember(p));
		}
	}

	/// <summary>
	/// Represents a property of type <see cref="JObject"/>. The object contains json representing the data. Retrieve the raw json by calling ToString() on the object.
	/// </summary>
	[UmbracoPropertyId("UMBRACO.GRID")]
	public class JObjectProperty : DocTypeProperty<JObject>
	{
	}

	/// <summary>
	/// Represents a property of type <see cref="string"/>. This can be converted to the Umbraco model Umbraco.Web.Models.ImageCropDataSet by calling
	/// Newtonsoft.Json.JsonConvert.DeserializeObject&lt;Umbraco.Web.Models.ImageCropDataSet&gt;(Model.Content.GetPropertyValue(DocTypes.&lt;DocType&gt;.&lt;Imagecropper&gt;))
	/// </summary>
	// TODO?: Should inherit from DocTypeProperty<Umbraco.Web.Models.ImageCropDataSet>, but that assembly isn't currently referenced.
	[UmbracoPropertyId("7A2D436C-34C2-410F-898F-4A23B3D79F54")]
	[UmbracoPropertyId("UMBRACO.IMAGECROPPER")]
	//public class ImageCropperProperty : DocTypeProperty<JObject>
	public class ImageCropperProperty : StringProperty
	{
	}

	/// <summary>
	/// Represents a property of type <see cref="JArray"/>. The object contains json representing the data, assuming the outermost json data is an array. Retrieve the raw json by calling ToString() on the object.
	/// </summary>	[UmbracoPropertyId("71B8AD1A-8DC2-425C-B6B8-FAA158075E63")]
	[UmbracoPropertyId("UMBRACO.RELATEDLINKS")]
	public class JArrayProperty : DocTypeProperty<JArray>
	{
	}
}