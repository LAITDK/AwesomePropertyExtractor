using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using APE.Umbraco.Core.DTO;
using System.Data.SqlServerCe;
using System.Configuration;

namespace APE.Umbraco.Core
{
	public static class DBConnection
	{
		public static IEnumerable<ContentTypeDTO> GetDocTypes(string dataDir, ConnectionStringSettings connectionSetting)
		{
			return GetContentTypes<ContentTypeDTO>(dataDir, connectionSetting, new Guid(global::Umbraco.Core.Constants.ObjectTypes.DocumentType));
		}

		public static IEnumerable<MediaTypeDTO> GetMediaTypes(string dataDir, ConnectionStringSettings connectionSetting)
		{
			return GetContentTypes<MediaTypeDTO>(dataDir, connectionSetting, new Guid(global::Umbraco.Core.Constants.ObjectTypes.MediaType));
		}

		public static IEnumerable<MemberTypeDTO> GetMemberTypes(string dataDir, ConnectionStringSettings connectionSetting)
		{
			return GetContentTypes<MemberTypeDTO>(dataDir, connectionSetting, new Guid(global::Umbraco.Core.Constants.ObjectTypes.MemberType));
		}

		private static IEnumerable<T> GetContentTypes<T>(string dataDir, ConnectionStringSettings connectionSetting, Guid contentTypeGuid)
			where T : ContentTypeDTO, new()
		{
			List<T> tList = new List<T>();
			string getAllSql = Properties.Resources.ContentTypeSql;

			if (connectionSetting != null)
			{
				using (IDbConnection conn = GetDbConnection(dataDir, connectionSetting))
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
								var contentTypeAliasText = Helpers.GetValidCSharpIdentifier(contentTypeAlias);
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
			}

			return tList;
		}

		// Recursively get all inherited content types.
		private static void GetAllInheritedContentTypeIds(ICollection<int> ids, IDbConnection conn, int id, Guid contentTypeGuid)
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

		private static void GetContentTypeProperties<T>(T t, IEnumerable<int> ids, IDbConnection conn, int parentId, Guid contentTypeGuid)
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
					contentTypePropertyDTO.PropertyAlias = record["PropertyAlias"].ToString();
					contentTypePropertyDTO.PropertyAliasText = Helpers.GetValidCSharpIdentifier(contentTypePropertyDTO.PropertyAlias);
					if (!string.IsNullOrWhiteSpace(contentTypePropertyDTO.PropertyAliasText))
						contentTypePropertyDTO.PropertyAliasText = contentTypePropertyDTO.PropertyAliasText[0].ToString().ToUpper() + contentTypePropertyDTO.PropertyAliasText.Substring(1);
					contentTypePropertyDTO.PropertyDescription = record["PropertyDescription"].ToString();
					contentTypePropertyDTO.PropertyType = record["PropertyType"].ToString();
					contentTypePropertyDTO.PropertyTypeAlias = Helpers.GetPropertyType(record["PropertyEditorAlias"].ToString());

					t.Properties.Add(contentTypePropertyDTO);
				}
			}
		}

		public static IEnumerable<DictionaryItemDTO> GetDictionary(string dataDir, ConnectionStringSettings connectionSetting)
		{
			if (connectionSetting != null)
			{
				using (IDbConnection conn = GetDbConnection(dataDir, connectionSetting))
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
									Name = Helpers.GetValidCSharpIdentifier(dictionaryAlias),
									Alias = dictionaryAlias
								};
							}
						}
					}
				}
			}
		}

		private static IDbConnection GetDbConnection(string dataDir, ConnectionStringSettings connectionSetting)
		{
			var connString = connectionSetting.ConnectionString.Replace("|DataDirectory|", dataDir);

			if (connectionSetting.ProviderName.Contains("SqlServerCe")) // Is SQL CE
			{
				return new SqlCeConnection(connString);
			}

			return new SqlConnection(connString);
		}
	}
}