namespace SQLSimpleInterpreter
{
    using Contracts;
    using System.Text;
    using System;
    using Helpers;

    public class SQLTable : IGeneratable
    {
        /// <summary>
        /// Creates an instance of SQLTable with only table name.
        /// <para>Please note: You've entered no columns, meaning that, if you want to generate a query, it will select everything and alias will be auto generated</para>
        /// </summary>
        /// <param name="tableName">Name of table</param>
        public SQLTable(string tableName)
        {
            this.Name = tableName;
        }

        /// <summary>
        /// Creates an instance of SQLTable with table name and alias.
        /// Please note: You've entered no columns, meaning that, if you want to generate a query, it will select everything.
        /// </summary>
        /// <param name="tableName">Name of the table</param>
        /// <param name="alias">Alias of the table</param>
        public SQLTable(string tableName, string alias)
            : this(tableName)
        {
            this.Alias = alias;
        }

        /// <summary>
        /// Creates an instance of SQLTable. All of the elements in the ColumnCollection will be selected, if a future query is generated.
        /// </summary>
        /// <param name="tableName">Name of the table</param>
        /// <param name="columns">ColumnCollection class contains the column names to be selected.</param>
        public SQLTable(string tableName, ColumnCollection columns)
            : this(tableName)
        {
            this.Columns = columns;
        }

        /// <summary>
        /// Creates an instance of SQLTable. All of the elements in the ColumnCollection will be selected, if a future query is generated.
        /// </summary>
        /// <param name="tableName">Name of the table</param>
        /// <param name="alias">Alias of the table</param>
        /// <param name="columns">ColumnCollection class contains the column names to be selected.</param>
        public SQLTable(string tableName, string alias, ColumnCollection columns)
            : this(tableName, alias)
        {
            this.Columns = columns;
        }

        /// <summary>
        /// The name of the SQLTable
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The alias of the table. If none is given, it takes the first letter of the table name.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// The columns to be selected, if future query is generated. You can add or remove columns at any time.
        /// </summary>
        public ColumnCollection Columns { get; set; }

        /// <summary>
        /// Generates a SELECT SQL Query. This will give you all the data, for the given columns.
        /// </summary>
        /// <returns></returns>
        public string GenerateQuery()
        {
            StringBuilder builder = new StringBuilder();

            this.Name = TablePropertiesValidator.ReturnValidName(this.Name);
            this.Alias = TablePropertiesValidator.ValidateOrCreateAlias(this.Alias, this.Name);
            this.Columns = TablePropertiesValidator.ValidateColumns(this.Columns, this.Alias);

            if (this.Columns.Count() == 0)
            {
                builder.AppendLine($"SELECT *");
            }
            else
            {
                builder.AppendLine($"SELECT {string.Join($"{Environment.NewLine}      ,", this.Columns)}");
            }

            builder.Append($"  FROM [dbo].{this.Name} AS {this.Alias}");

            return builder.ToString();
        }

        /// <summary>
        /// Generates a SELECT SQL Query. This will give you all the data, for the given columns.
        /// </summary>
        /// <param name="selectTopX">Take the first X results. </param>
        /// <returns></returns>
        public string GenerateQuery(int selectTopX)
        {
            var result = this.GenerateQuery();

            return result.Replace("SELECT", $"SELECT TOP ({selectTopX})");
        }
    }
}
