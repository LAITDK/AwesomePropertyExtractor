using APE.Umbraco.Core.DTO;
using APE.Umbraco.Core.Interfaces;
using APE.Umbraco.Core.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace APE.Umbraco.Core
{
    public class AssemblyProxy : MarshalByRefObject
    {
        private readonly Dictionary<string, Type> TypeDictionary = new Dictionary<string, Type>();
        private ProxyHelpers _ProxyHelpers;

        public AssemblyProxy()
        {
        }

        public void Init(ConnectionStringContainer connectionString)
        {
            _ProxyHelpers = new ProxyHelpers(connectionString);
        }

        public IDictionary<string, Type> GetTypeDictionary(string assemblyLocation)
        {
            if (TypeDictionary.Any())
            {
                return TypeDictionary;
            }
            var classes = this.GetTypes(assemblyLocation);

            foreach (var type in classes)
            {
                foreach (var attribute in type.CustomAttributes.Where(x => x.AttributeType.AssemblyQualifiedName == typeof(UmbracoPropertyIdAttribute).AssemblyQualifiedName))
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

            return TypeDictionary;
        }

        public Type GetPropertyType(string idOrAlias, string assemblyLocation)
        {
            Type propType;
            if (!GetTypeDictionary(assemblyLocation).TryGetValue(idOrAlias.ToUpperInvariant(), out propType))
            {
                propType = typeof(StringProperty);
            }
            return propType;
        }

        public string GetValueTypeName(string editorAlias, string assemblyLocation, int dataTypeId)
        {
            var type = GetPropertyType(editorAlias, assemblyLocation);
            var preValues = GetPreValues(dataTypeId);
            var instance = (IDocTypeProperty)type.Assembly.CreateInstance(type.FullName);

            type = instance.GetValueType(preValues);


            return _ProxyHelpers.CSharpName(type);
        }

        public string GetTypeName(string editorAlias, string assemblyLocation)
        {
            var type = GetPropertyType(editorAlias, assemblyLocation);

            return _ProxyHelpers.CSharpName(type);
        }
        public ContentTypeDTO[] GetDocTypes()
        {
            return _ProxyHelpers._DbContext.GetContentTypes<ContentTypeDTO>(new Guid(global::Umbraco.Core.Constants.ObjectTypes.DocumentType)).ToArray();
        }

        public MediaTypeDTO[] GetMediaTypes()
        {
            return _ProxyHelpers._DbContext.GetContentTypes<MediaTypeDTO>(new Guid(global::Umbraco.Core.Constants.ObjectTypes.MediaType)).ToArray();
        }

        public MemberTypeDTO[] GetMemberTypes()
        {
            return _ProxyHelpers._DbContext.GetContentTypes<MemberTypeDTO>(new Guid(global::Umbraco.Core.Constants.ObjectTypes.MemberType)).ToArray();
        }

        public DictionaryItemDTO[] GetDictionary()
        {
            return _ProxyHelpers._DbContext.GetDictionary().ToArray();
        }

        private IEnumerable<PropertyPreValue> GetPreValues(int dataTypeId)
        {
            var items = _ProxyHelpers._DbContext.GetPreValues()[dataTypeId];
            return items;
        }


        private IEnumerable<Type> GetClasses(Assembly assembly)
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

        private IEnumerable<Type> GetTypes(string assemblyLocation)
        {
            var classes = new List<Type>();
            var dlls = Directory.GetFiles(assemblyLocation, "*.dll", SearchOption.AllDirectories);
            foreach (var dll in dlls)
            {
                try
                {
                    var assembly = this.GetAssembly(dll);
                    if (assembly != null)
                        classes.AddRange(GetClasses(assembly));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

            return classes;
        }


        private Assembly GetAssembly(string assemblyPath)
        {
            try
            {
                var tmp = Assembly.LoadFile(assemblyPath);
                return Assembly.Load(tmp.FullName);
            }
            catch (Exception)
            {
                return null;
                // throw new InvalidOperationException(ex);
            }
        }
    }
}
