namespace SQLSimpleInterpreter.Helpers
{
    using Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal static class TablePropertiesValidator
    {
        private const string ErrorInvalidTableName = "SQL Table invalid name. You cant generate query, when SQL Table Name is whitespace or null.";

        public static string ReturnValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new SQLInvalidTableException(ErrorInvalidTableName);
            }

            if (CheckIfNameIsAlreadyValid(name))
            {
                return name;
            }

            if (name.Split(new[] { ' ' },
                StringSplitOptions.RemoveEmptyEntries).Count() > 0)
            {
                var nameParts = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                name = $"[{string.Join(' ', nameParts)}]";
            }
            else
            {
                name = $"[{name}]";
            }

            return name;
        }

        private static bool CheckIfNameIsAlreadyValid(string name)
        {
            return name.Contains('[') && name.Contains("]");
        }

        public static string ValidateOrCreateAlias(string alias, string name)
        {
            if (string.IsNullOrWhiteSpace(alias))
            {
                if (name.StartsWith('['))
                {
                    return alias = name.Substring(1, 1).ToLower();
                }
                else
                {
                    return alias = name.Substring(0, 1).ToLower();
                }
            }

            return alias.ToLower();
        }

        public static ColumnCollection ValidateColumns(ColumnCollection columns, string alias)
        {
            if (columns == null)
            {
                return new ColumnCollection();
            }

            List<string> cols = GetInstanceField(typeof(ColumnCollection), columns, "columns") as List<string>;
            List<string> validCols = new List<string>();

            if (cols.All(c => c.Contains($".[") && c.EndsWith(']')))
            {
                if (cols.All(c => c.StartsWith($"{alias}.[")))
                {
                    return new ColumnCollection(cols.ToArray());
                }

                int indexOfPointAfterAlias = cols.First().IndexOf(".[");
                cols = cols.Select(c => $"{alias}{c.Substring(indexOfPointAfterAlias)}").ToList();

                return new ColumnCollection(cols.ToArray());
            }

            foreach (var col in cols)
            {
                if (col.Split(' ', StringSplitOptions.RemoveEmptyEntries).Count() > 0)
                {
                    var colTokens = col.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
                    validCols.Add($"{alias}.[{string.Join(' ', colTokens)}]");
                }
                else
                {
                    validCols.Add($"{alias}.[{col}]");
                }
            }

            ColumnCollection result = new ColumnCollection(validCols.ToArray());

            return result;
        }

        private static object GetInstanceField(Type type, object instance, string fieldName)
        {
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.Static;
            FieldInfo field = type.GetField(fieldName, bindFlags);
            return field.GetValue(instance);
        }
    }
}
