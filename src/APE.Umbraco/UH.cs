using System.Web;
using Umbraco.Web;

namespace APE.Umbraco
{
	public static class UH
	{
		public const string UmbracoHelperKey = "__LaitUmbracoHelper__";

		public static UmbracoHelper UmbracoHelper
		{
			get
			{
				var helper = HttpContext.Current.Items[UmbracoHelperKey] as UmbracoHelper;
				if (helper == null)
					HttpContext.Current.Items[UmbracoHelperKey] = helper = new UmbracoHelper(UmbracoContext.Current);
				return helper;
			}
		}
	}
}