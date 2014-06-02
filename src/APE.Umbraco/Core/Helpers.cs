using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APE.Umbraco.Core
{
	public static class Helpers
	{
		public static string GetPropertyType(string idOrAlias)
		{
			Type propType = null;
			switch (idOrAlias.ToUpper())
			{
				// Content Picker
				case "158AA029-24ED-4948-939E-C3DA209E5FBA":
				case "UMBRACO.CONTENTPICKERALIAS":
					propType = typeof(ContentProperty);
					break;
				// Media Picker
				case "EAD69342-F06D-4253-83AC-28000225583B":
				case "UMBRACO.MEDIAPICKER":
					propType = typeof(MediaProperty);
					break;
				// Richtext editor
				case "5E9B75AE-FACE-41C8-B47E-5F4B0FD82F8":
				case "UMBRACO.TINYMCEV3":
				// Numeric
				case "1413AFCB-D19A-4173-8E9A-68288D2A73B8":
				case "UMBRACO.INTEGER":
				// Dropdown
				case "A74EA9C9-8E18-4D2A-8CF6-73C6206C5DA6":
				case "UMBRACO.DROPDOWN":
				// Radiobox
				case "A52C7C1C-C330-476E-8605-D63D3B84B6A6":
				case "UMBRACO.RADIOBUTTONLIST":
					propType = typeof(IntProperty);
					break;
				// True/false
				case "38B352C1-E9F8-4FD8-9324-9A2EAB06D97A":
				case "UMBRACO.TRUEFALSE":
					propType = typeof(BooleanProperty);
					break;
				// Date Picker
				case "23E93522-3200-44E2-9F29-E61A6FCBB79A":
				case "UMBRACO.DATE":
				// Date Picker with time
				case "928639ED-9C73-4028-920C-1E55DBB68783":
				case "UMBRACO.DATETIME":
					propType = typeof(DateTimeProperty);
					break;
				// Multi-Node Tree Picker
				case "7E062C13-7C41-4AD9-B389-41D88AEEF87C":
				case "UMBRACO.MULTINODETREEPICKER":
					propType = typeof(MultiNodeProperty);
					break;
				// Multiple Media Picker
				case "UMBRACO.MULTIPLEMEDIAPICKER":
					propType = typeof(MultiMediaProperty);
					break;
				// Textbox multiple
				case "67DB8357-EF57-493E-91AC-936D305E0F2A":
				case "UMBRACO.TEXTBOXMULTIPLE":
				default:
					propType = typeof(StringProperty);
					break;
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
