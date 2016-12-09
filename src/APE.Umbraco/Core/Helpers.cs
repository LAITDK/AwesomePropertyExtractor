using APE.Umbraco.Core.DTO;
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
//using Microsoft.CodeAnalysis.CSharp;

namespace APE.Umbraco.Core
{
    public class Helpers : IDisposable
    {
        private readonly string _assemblyLocation;
        private readonly AssemblyProxy _Proxy;
        private readonly AppDomain _dom;

        public Helpers(string assemblyLocation, ConnectionStringContainer connectionString)
        {
            _assemblyLocation = assemblyLocation;
            var currentAssembly = Assembly.GetExecutingAssembly();
            AppDomainSetup domaininfo = new AppDomainSetup();
            domaininfo.ApplicationBase = assemblyLocation;
            domaininfo.ShadowCopyFiles = "true";
            var adevidence = AppDomain.CurrentDomain.Evidence;

            _dom = AppDomain.CreateDomain("APE-assembly-loader", adevidence, domaininfo);

            _Proxy = GetProxy(_dom);
            _Proxy.Init(connectionString);

        }

        private AssemblyProxy GetProxy(AppDomain dom)
        {
            var tst = AppDomain.CurrentDomain.GetAssemblies();
            Debug.WriteLine(dom.BaseDirectory);
            Type proxyType = typeof(AssemblyProxy);
            var value = (AssemblyProxy)dom.CreateInstanceAndUnwrap(
                proxyType.Assembly.FullName,
                proxyType.FullName);
            return value;
        }

        public string GetValueTypeName(string editorAlias, int dataTypeId)
        {
            return _Proxy.GetValueTypeName(editorAlias, this._assemblyLocation, dataTypeId);
        }

        public string GetTypeName(string editorAlias)
        {
            return _Proxy.GetTypeName(editorAlias, this._assemblyLocation);
        }

        public ContentTypeDTO[] GetDocTypes()
        {
            return _Proxy.GetDocTypes();
        }

        public MediaTypeDTO[] GetMediaTypes()
        {
            return _Proxy.GetMediaTypes();
        }

        public MemberTypeDTO[] GetMemberTypes()
        {
            return _Proxy.GetMemberTypes();
        }
        public DictionaryItemDTO[] GetDictionary()
        {
            return _Proxy.GetDictionary();
        }

        public void Dispose()
        {
            AppDomain.Unload(_dom);
        }
    }
}