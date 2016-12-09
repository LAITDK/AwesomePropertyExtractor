using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using APE.Umbraco.Core.DTO;
using System.Data.SqlServerCe;
using System.Configuration;
using APE.Umbraco.Core.Models;

namespace APE.Umbraco.Core
{
    public class DBConnection
    {
        private readonly ProxyHelpers _Helpers;
        private readonly ConnectionStringContainer _ConnectionString;
        private ILookup<int, PropertyPreValue> _PreValues;

        public DBConnection(ProxyHelpers helpers, ConnectionStringContainer connectionString)
        {
            _Helpers = helpers;
            _ConnectionString = connectionString;
        }


        internal IEnumerable<T> GetContentTypes<T>(Guid contentTypeGuid)
            where T : ContentTypeDTO, new()
        {
            List<T> tList = new List<T>();
            string getAllSql = Properties.Resources.ContentTypeSql;

            using (IDbConnection conn = GetDbConnection())
            {
                conn.Open();

                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = getAllSql;
                    var parm = cmd.CreateParameter();
                    parm.ParameterName = "contentTypeGuid";
                    parm.Value = contentTypeGuid;
                    cmd.Parameters.Add(parm);
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            T t = new T();

                            var contentTypeAlias = reader["ContentTypeAlias"].ToString();
                            var contentTypeAliasText = _Helpers.GetValidCSharpIdentifier(contentTypeAlias);
                            var contentTypeDescription = reader["ContentTypeDescription"].ToString();

                            t.ContentTypeAlias = contentTypeAlias;
                            t.ContentType = contentTypeAliasText[0].ToString().ToUpper() + contentTypeAliasText.Substring(1);
                            t.ContentTypeDescription = contentTypeDescription;
                            t.Id = (int)reader["Id"];
                            tList.Add(t);
                        }
                    }
                }

                foreach (T t in tList)
                {
                    var ids = new List<int>();
                    GetAllInheritedContentTypeIds(ids, conn, t.Id, contentTypeGuid);
                    GetContentTypeProperties(t, ids, conn, t.Id, contentTypeGuid);
                }
            }


            return tList;
        }

        internal ILookup<int, PropertyPreValue> GetPreValues()
        {
            if (_PreValues!= null && _PreValues.Any())
            {
                return _PreValues;
            }
            List<DataTypePreValue> tList = new List<DataTypePreValue>();
            string getAllSql = Properties.Resources.CMSDataTypePreValuesSql;

            using (IDbConnection conn = GetDbConnection())
            {
                conn.Open();

                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = getAllSql;

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DataTypePreValue t = new DataTypePreValue();

                            t.Alias = reader["Alias"].ToString();
                            t.DataTypeId = (int)reader["DatatypeNodeId"];
                            t.Value = reader["Value"].ToString();
                            tList.Add(t);
                        }
                    }
                }
            }

            return _PreValues = tList.ToLookup(k => k.DataTypeId, v => (PropertyPreValue)v);
        }

        // Recursively get all inherited content types.
        private void GetAllInheritedContentTypeIds(ICollection<int> ids, IDbConnection conn, int id, Guid contentTypeGuid)
        {
            if (id == -1)
                return;
            ids.Add(id);

            string getPropSql = Properties.Resources.InheritedContentTypeSql;

            List<int> parentIds = new List<int>();
            using (IDbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = getPropSql;
                var parm = cmd.CreateParameter();
                parm.ParameterName = "contentTypeGuid";
                parm.Value = contentTypeGuid;
                cmd.Parameters.Add(parm);
                parm = cmd.CreateParameter();
                parm.ParameterName = "id";
                parm.Value = id;
                cmd.Parameters.Add(parm);
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var record = reader;

                        var contentTypePropertyDTO = new ContentTypePropertyDTO();
                        int parentId = -1;
                        if (record["ParentId"] is int)
                            parentId = (int)record["ParentId"];
                        parentIds.Add(parentId);
                    }
                }
            }

            foreach (int parentId in parentIds)
            {
                GetAllInheritedContentTypeIds(ids, conn, parentId, contentTypeGuid);
            }
        }

        private void GetContentTypeProperties<T>(T t, IEnumerable<int> ids, IDbConnection conn, int parentId, Guid contentTypeGuid)
    where T : ContentTypeDTO
        {
            if (parentId == -1)
                return;
            //			string getPropSql = @"
            //select un.Id, un.[Text] as ContentType,
            //cpt.Alias as PropertyAlias,
            //cpt.Description as PropertyDescription,
            //un3.Text as PropertyType,
            //cdt.PropertyEditorAlias as PropertyEditorAlias
            //from umbracoNode un
            // inner join cmspropertytype cpt on un.id = cpt.contentTypeId
            // left join cmsdatatype cdt on cpt.dataTypeId = cdt.nodeId
            // left join umbracoNode un3 on cpt.dataTypeId = un3.id
            //where un.NodeObjectType = '{0}'
            //and un.Id in ({1})";

            string getPropSql = Properties.Resources.ContentTypePropertiesSql;

            IDbCommand cmd = conn.CreateCommand();
            StringBuilder sb = new StringBuilder();
            var parm = cmd.CreateParameter();
            parm.ParameterName = "contentTypeGuid";
            parm.Value = contentTypeGuid;
            cmd.Parameters.Add(parm);
            var parameters = new string[ids.Count()];
            for (int i = 0; i < ids.Count(); i++)
            {
                parm = cmd.CreateParameter();
                parm.ParameterName = string.Format("@ids{0}", i);
                parm.Value = ids.ElementAt(i);
                cmd.Parameters.Add(parm);
                if (sb.Length > 0)
                    sb.Append(',');
                sb.Append(parm.ParameterName);
            }
            cmd.CommandText = string.Format(getPropSql, sb.ToString());
            using (IDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var record = reader;

                    var contentTypePropertyDTO = new ContentTypePropertyDTO();
                    contentTypePropertyDTO.DataTypeId = (int)record["DataTypeId"];
                    contentTypePropertyDTO.PropertyAlias = record["PropertyAlias"].ToString();
                    contentTypePropertyDTO.PropertyAliasText = _Helpers.GetValidCSharpIdentifier(contentTypePropertyDTO.PropertyAlias);
                    if (!string.IsNullOrWhiteSpace(contentTypePropertyDTO.PropertyAliasText))
                        contentTypePropertyDTO.PropertyAliasText = contentTypePropertyDTO.PropertyAliasText[0].ToString().ToUpper() + contentTypePropertyDTO.PropertyAliasText.Substring(1);
                    contentTypePropertyDTO.PropertyDescription = record["PropertyDescription"].ToString();
                    contentTypePropertyDTO.PropertyType = record["PropertyType"].ToString();

                    contentTypePropertyDTO.EditorAlias = record["PropertyEditorAlias"].ToString();

                    t.Properties.Add(contentTypePropertyDTO);
                }
            }
        }

        public IEnumerable<DictionaryItemDTO> GetDictionary()
        {
            using (IDbConnection conn = GetDbConnection())
            {
                conn.Open();

                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = Properties.Resources.DictionarySql;
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var record = reader;

                            var dictionaryAlias = record[0].ToString();

                            yield return new DictionaryItemDTO
                            {
                                Name = _Helpers.GetValidCSharpIdentifier(dictionaryAlias),
                                Alias = dictionaryAlias
                            };
                        }
                    }
                }
            }

        }

        private IDbConnection GetDbConnection()
        {
            var connString = _ConnectionString.ConnectionString.Replace("|DataDirectory|", _ConnectionString.DataDir);

            if (_ConnectionString.ProviderName.Contains("SqlServerCe")) // Is SQL CE
            {
                return new SqlCeConnection(connString);
            }

            return new SqlConnection(connString);
        }
    }
}