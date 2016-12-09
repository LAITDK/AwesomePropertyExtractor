using APE.Umbraco.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APE.Umbraco.Core
{
    public class ProxyHelpers
    {
        public readonly DBConnection _DbContext;

        public ProxyHelpers(ConnectionStringContainer connectionString)
        {
            _DbContext = new DBConnection(this, connectionString);
        }

        internal string CSharpName(Type type)
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
            }
            else
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
        public string GetValidCSharpIdentifier(string identifier)
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
