using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.CodeAnalysis.CSharp;

namespace APE.Umbraco.Core
{
    public static class Helpers
    {
        private static readonly Dictionary<string, Type> TypeDictionary = new Dictionary<string, Type>();

        private static IEnumerable<Type> GetClasses(Assembly assembly)
        {
            var types = Enumerable.Empty<Type>();
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                types = ex.Types.Where(t => t != null);
            }
            return types
                .Where(type => type.IsClass && type.IsAbstract == false)
                //.Where(type => type.IsSubclassOf(typeof(DocTypeProperty)))
                .Where(x => x.CustomAttributes.Any(ca => ca.AttributeType == typeof(UmbracoPropertyIdAttribute)));
        }

        static Helpers()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var location = new FileInfo(currentAssembly.Location).Directory.FullName;
            AppDomainSetup domaininfo = new AppDomainSetup();
            domaininfo.ApplicationBase = location;
            var adevidence = AppDomain.CurrentDomain.Evidence;

            var dom = AppDomain.CreateDomain("APE-assembly-loader", adevidence, domaininfo);
            try
            {
                AssemblyProxy value = GetProxy(dom);

                var classes = new List<Type>();
                var dlls = Directory.GetFiles(location, "*.dll", SearchOption.AllDirectories);
                foreach (var dll in dlls)
                {
                    try
                    {
                        var assembly = value.GetAssembly(dll);
						if (assembly != null)
	                        classes.AddRange(GetClasses(assembly));
                    }
                    catch (Exception ex)
                    {
                    }
                }

                foreach (var type in classes)
                {
                    foreach (var attribute in type.CustomAttributes.Where(x => x.AttributeType == typeof(UmbracoPropertyIdAttribute)))
                    {
                        var id = attribute.ConstructorArguments.FirstOrDefault().Value.ToString().ToUpperInvariant();
                        if (TypeDictionary.ContainsKey(id))
                        {
                            var existing = TypeDictionary[id];
                            throw new InvalidOperationException($"The property type '{id}' on '{type.FullName}' has already been registered on '{existing.FullName}'!");
                        }

                        TypeDictionary.Add(id, type);
                    }
                }
            }
            finally
            {
                AppDomain.Unload(dom);
            }
        }

        private static AssemblyProxy GetProxy(AppDomain dom)
        {
            Type proxyType = typeof(AssemblyProxy);
            var value = (AssemblyProxy)dom.CreateInstanceAndUnwrap(
                proxyType.Assembly.FullName,
                proxyType.FullName);
            return value;
        }

        public static Type GetPropertyType(string idOrAlias)
        {
            Type propType;
            if (!TypeDictionary.TryGetValue(idOrAlias.ToUpperInvariant(), out propType))
            {
                propType = typeof(StringProperty);
            }
            return propType;
        }

        public static string CSharpName(Type type)
        {
            var sb = new StringBuilder();
            var name = type.FullName;
            if (type.IsGenericType)
            {
                sb.Append(name.Substring(0, name.IndexOf('`')));
                sb.Append("<");
                sb.Append(string.Join(", ", type.GetGenericArguments()
                                                .Select(t => CSharpName(t))));
                sb.Append(">");
            }else
            {
                sb.Append(name);
            }

            var csName = sb.ToString();
            if (csName.StartsWith("Umbraco"))
            {
                csName = "global::" + csName;
            }

            return csName;
        }

        // Get a valid C# identifier from a string. This method will also try to CamelCase the identifier.
        // This will in no way generate the perfect identifier nor can it check if the identifier already exists.
        public static string GetValidCSharpIdentifier(string identifier)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier) + " must have a value");
            identifier = identifier.Trim();
            if (identifier.Length == 0)
                throw new ArgumentException(nameof(identifier) + " is an empty or whitespace string, which is invalid");

            // If the first char isn't a valid first char then prepend the identifier name with a "_".
            var sb = new StringBuilder();
            if (!CSharpHelper.IsIdentifierStartCharacter(identifier[0]))
                sb.Append("_");

            // Go through all the chars and try to create the "perfect" identifier.
            // All '-' will be converted to '_', all other invalid chars are ignored.
            // Will also try to CamelCase the identifier where appropriate.
            bool upperCaseNextChar = true;
            foreach (var c in identifier)
            {
                char checkChar = c;
                if (checkChar == '-')
                    checkChar = '_';
                else if (upperCaseNextChar)
                {
                    checkChar = char.ToUpperInvariant(checkChar);
                    upperCaseNextChar = false;
                }

                if (CSharpHelper.IsIdentifierPartCharacter(checkChar))
                    sb.Append(checkChar);
                else
                    upperCaseNextChar = true;
            }

            var newIdentifierName = sb.ToString();

            // Check if the identifier is a known C# keyword. Won't check for existing object names like Int32.
            // Note: All C# identifiers start with a lowercase letter (or underscores followed by a lowercase letter), but the generated identifier will start
            // with an uppercase letter, so this check probably isn't needed.
            if (CSharpHelper.IsKeyword(newIdentifierName))
                newIdentifierName = "@" + newIdentifierName;

            return newIdentifierName;
        }
    }
}