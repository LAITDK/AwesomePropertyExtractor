using APE.Umbraco.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace APE.Umbraco.Core
{
	public static class Helpers
	{
        private static readonly Dictionary<string, Type> TypeDictionary = new Dictionary<string, Type>();

        private static IEnumerable<Type> GetClasses(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.GetInterfaces().Any(i => i == typeof(IUmbracoProperty)))
                .Where(x => x.CustomAttributes.Any(ca => ca.AttributeType == typeof(UmbracoPropertyIdAttribute)));
        }

        static Helpers()
        {
            var classes = new List<Type>();

            //var test2 = Assembly.LoadFrom(currentAssemblyPath);

            //classes.AddRange(GetClasses(test2));
			classes.AddRange(GetClasses(typeof(IUmbracoProperty).Assembly));

            foreach (var type in classes)
            {
                foreach (var attribute in type.CustomAttributes.Where(x => x.AttributeType == typeof(UmbracoPropertyIdAttribute)))
                {
                    var id = attribute.ConstructorArguments.FirstOrDefault().Value.ToString().ToUpper();
                    TypeDictionary.Add(id, type);
                }
            }
        }
        public static string GetPropertyType(string idOrAlias)
        {
            Type propType;
            if (!TypeDictionary.TryGetValue(idOrAlias.ToUpper(), out propType))
            {
                propType = typeof(StringProperty);
            }
            return propType.Name;
        }

		public static string NameReplacement(string input)
		{
			var propReplacement = new Dictionary<string, string>
	        {
	            {"-", "_"},
	            {" ", "_"},
	            {".", ""},
	            {",", ""},
	            {";", ""},
	            {@"""", ""},
	            {"'*", ""}
	        };


			foreach (var replacement in propReplacement)
			{
				input = input.Replace(replacement.Key, replacement.Value);
			}

			return input;
		}
	}
}
