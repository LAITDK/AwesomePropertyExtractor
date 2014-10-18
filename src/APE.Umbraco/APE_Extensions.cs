using APE.Umbraco;
using Umbraco.Core.Models;

namespace Umbraco.Web
{
    /// <summary>
    /// Extends umbraco's IPublishedContent to enable the use of our DocTypeProperty.
    /// </summary>
	public static class APE_Extensions
    {
        /// <summary>
        /// Gets an umbraco property from the passed content, and returns it in the type specified by the DocTypeProperty.
        /// </summary>
        /// <typeparam name="TType">Type to deliver the property value as.</typeparam>
        /// <param name="content">Content to get the property from.</param>
        /// <param name="property">The DocTypeProperty that contains information on how to receive the property.</param>
        /// <param name="recursive">Defines if it should recursively go through ancestors for the property. Default is false.</param>
        /// <returns>The value of the property, in the type specified by the passed DocTypeProperty</returns>
        public static TType GetPropertyValue<TType>(this IPublishedContent content, DocTypeProperty<TType> property, bool recursive)
        {
            return property.Map(content, recursive);
        }

        /// <summary>
        /// Gets an umbraco property from the passed content, and returns it in the type specified by the DocTypeProperty.
        /// </summary>
        /// <typeparam name="TType">Type to deliver the property value as.</typeparam>
        /// <param name="content">Content to get the property from.</param>
        /// <param name="property">The DocTypeProperty that contains information on how to receive the property.</param>
        /// <returns>The value of the property, in the type specified by the passed DocTypeProperty</returns>
        public static TType GetPropertyValue<TType>(this IPublishedContent content, DocTypeProperty<TType> property)
        {
            return content.GetPropertyValue(property, false);
        }
    }
}