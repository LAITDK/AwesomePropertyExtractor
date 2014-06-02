using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using APE.Umbraco.Core.DTO;
using System.Data.SqlServerCe;
using System.Text.RegularExpressions;
using System.Configuration;
using APE.Umbraco.Properties;

namespace APE.Umbraco.Core
{
	public static class DBConnection
	{
		public static IEnumerable<DocTypeDTO> GetDocTypes(string dataDir, ConnectionStringSettings connectionSetting)
		{
			if (connectionSetting != null)
			{
				using (IDbConnection conn = GetDbConnection(dataDir, connectionSetting))
				{
					conn.Open();

					var cmd = conn.CreateCommand();
					cmd.CommandText = Resources.DocTypeSql;
					var reader = cmd.ExecuteReader();
					while (reader.Read())
					{
						var record = (IDataRecord)reader;

						var doctypeAlias = record["DocType"].ToString();
						string propertyTypeAlias = Helpers.GetPropertyType(record["propertyTypeId"].ToString());

						var propertyAlias = record["PropertyAlias"].ToString();
						var propertyAliasText = Helpers.NameReplacement(propertyAlias);
						
						if (!string.IsNullOrWhiteSpace(propertyAliasText))
						{
							propertyAliasText = propertyAliasText[0].ToString().ToUpper() + propertyAliasText.Substring(1);
						}

						yield return new DocTypeDTO
						{
							PropertyAlias = record["PropertyAlias"].ToString(),
							PropertyAliasText = propertyAliasText,
							DocType = doctypeAlias[0].ToString().ToUpper() + doctypeAlias.Substring(1),
							DocTypeAlias = doctypeAlias,
							ParentDocType = record["ParentDocType"].ToString(),
							PropertyDescription = record["PropertyDescription"].ToString().Replace(Environment.NewLine, " "),
							PropertyType = record["PropertyType"].ToString(),
							PropertyTypeAlias = propertyTypeAlias
						};
					}
				}
			}
		}

		public static IEnumerable<DictionaryItemDTO> GetDictionary(string dataDir, ConnectionStringSettings connectionSetting)
		{
			if (connectionSetting == null) yield break;

			using (IDbConnection conn = GetDbConnection(dataDir, connectionSetting))
			{
				conn.Open();

				var cmd = conn.CreateCommand();
				cmd.CommandText = Resources.DictionarySql;
				IDataReader reader = cmd.ExecuteReader();

				while (reader.Read())
				{
					var record = reader;
					var dictionaryAlias = record[0].ToString();

					yield return new DictionaryItemDTO()
					{
						Name = Helpers.NameReplacement(dictionaryAlias),
						Alias = dictionaryAlias
					};
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
