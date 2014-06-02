using System.Web;
using Umbraco.Web;

namespace APE.Umbraco
{
    public static class UH
    {
        public static UmbracoHelper UmbracoHelper
        {
            get
            {
                var helper = HttpContext.Current.Items["LaitUmbracoHelper"] as UmbracoHelper;
                if (helper == null)
                {
                    HttpContext.Current.Items["LaitUmbracoHelper"] = helper = new UmbracoHelper(UmbracoContext.Current);
                }
                return helper;
            }
        }
    }
}
