namespace SQLSimpleInterpreter
{
    using Contracts;
    using Helpers;
    using System.Text;
    using System;

    public class SQLJoin : IJoinGeneratable
    {
        /// <summary>
        /// Creates an instance of SQLJoin. You can use it to Join the given tables via join columns.
        /// </summary>
        /// <param name="tableOne">Main table</param>
        /// <param name="tableTwo">The table which will be joined to the first one</param>
        /// <param name="tableOneJoinColumnName">The column name of the key. E.g. Id.</param>
        /// <param name="tableTwoJoinColumnName">The column name of the key. E.g. Id.</param>
        public SQLJoin(SQLTable tableOne,
            SQLTable tableTwo,
            string tableOneJoinColumnName,
            string tableTwoJoinColumnName)
        {
            this.TableOne = tableOne;
            this.TableTwo = tableTwo;

            this.TableOneJoinColumn = tableOneJoinColumnName;
            this.TableTwoJoinColumn = tableTwoJoinColumnName;
        }

        public SQLTable TableOne { get; set; }
        public string TableOneJoinColumn { get; set; }

        public SQLTable TableTwo { get; set; }
        public string TableTwoJoinColumn { get; set; }

        /// <summary>
        /// Joins the given tables. Inner join will be used.
        /// </summary>
        /// <param name="selectTopX">Take the first X results.</param>
        /// <returns>SQL Query</returns>
        public string InnerJoin(int selectTopX = 0)
        {
            var result = this.BaseConstruction();

            result = result.Replace("{{{joinType}}}", " INNER");

            return selectTopX == 0 ? result : result.Replace("SELECT", $"SELECT TOP ({selectTopX})");
        }

        /// <summary>
        /// Joins the given tables. Inner join will be used.
        /// </summary>
        /// <param name="selectTopX">Take the first X results.</param>
        /// <returns>SQL Query</returns>
        public string Intersection(int selectTopX = 0)
        {
            var result = this.BaseConstruction();

            result = result.Replace("{{{joinType}}}", " INNER");

            return selectTopX == 0 ? result : result.Replace("SELECT", $"SELECT TOP ({selectTopX})");
        }

        /// <summary>
        /// Joins the given tables. Left join will be used.
        /// </summary>
        /// <param name="selectTopX">Take the first X results.</param>
        /// <returns>SQL Query</returns>
        public string LeftJoin(int selectTopX = 0)
        {
            var result = this.BaseConstruction();

            result = result.Replace("{{{joinType}}}", "  LEFT");

            return selectTopX == 0 ? result : result.Replace("SELECT", $"SELECT TOP ({selectTopX})");
        }

        /// <summary>
        /// Joins the given tables. Right join will be used.
        /// </summary>
        /// <param name="selectTopX">Take the first X results.</param>
        /// <returns>SQL Query</returns>
        public string RightJoin(int selectTopX = 0)
        {
            var result = this.BaseConstruction();

            result = result.Replace("{{{joinType}}}", " RIGHT");

            return selectTopX == 0 ? result : result.Replace("SELECT", $"SELECT TOP ({selectTopX})");
        }

        /// <summary>
        /// Joins the given tables.
        /// </summary>
        /// <param name="selectTopX">Take the first X results.</param>
        /// <returns>SQL Query</returns>
        public string LeftSetDifference(int selectTopX = 0)
        {
            var result = this.LeftJoin();
            result += Environment.NewLine + $" WHERE {this.TableTwo.Alias}.[{this.TableTwoJoinColumn}] IS NULL";

            return selectTopX == 0 ? result : result.Replace("SELECT", $"SELECT TOP ({selectTopX})");
        }

        /// <summary>
        /// Joins the given tables. Inner join will be used.
        /// </summary>
        /// <param name="selectTopX">Take the first X results.</param>
        /// <returns>SQL Query</returns>
        public string RightSetDifference(int selectTopX = 0)
        {
            var result = this.RightJoin();
            result += Environment.NewLine + $" WHERE {this.TableOne.Alias}.[{this.TableOneJoinColumn}] IS NULL";

            return selectTopX == 0 ? result : result.Replace("SELECT", $"SELECT TOP ({selectTopX})");
        }

        /// <summary>
        /// Joins the given tables. Full outer join will be used.
        /// </summary>
        /// <param name="selectTopX">Take the first X results.</param>
        /// <returns>SQL Query</returns>
        public string FullOuterJoin(int selectTopX = 0)
        {
            var result = this.BaseConstruction();

            result = result.Replace("{{{joinType}}}", "  FULL OUTER");

            return selectTopX == 0 ? result : result.Replace("SELECT", $"SELECT TOP ({selectTopX})");
        }

        /// <summary>
        /// Joins the given tables.
        /// </summary>
        /// <param name="selectTopX">Take the first X results.</param>
        /// <returns>SQL Query</returns>
        public string SymmetricDifference(int selectTopX = 0)
        {
            var result = this.FullOuterJoin();
            result += Environment.NewLine + $" WHERE {this.TableOne.Alias}.[{this.TableOneJoinColumn}] IS NULL" +
                Environment.NewLine +
                $"    OR {this.TableTwo.Alias}.[{this.TableTwoJoinColumn}] IS NULL";

            return selectTopX == 0 ? result : result.Replace("SELECT", $"SELECT TOP ({selectTopX})");
        }

        private string BaseConstruction()
        {
            var builder = new StringBuilder();

            JoinQueriesValidator.ThrowIfInvalidTableNames(this.TableOne.Name, this.TableTwo.Name);


            if (this.TableOne.Alias == null)
            {
                this.TableOne.Alias = TablePropertiesValidator.ValidateOrCreateAlias(this.TableOne.Alias, this.TableOne.Name);
            }
            if (this.TableTwo.Alias == null)
            {
                this.TableTwo.Alias = TablePropertiesValidator.ValidateOrCreateAlias(this.TableTwo.Alias, this.TableTwo.Name);
            }

            bool equalAlias = JoinQueriesValidator.AreAliasEqual(this.TableOne.Alias, this.TableTwo.Alias);
            if (equalAlias)
            {
                this.TableOne.Alias += "1";
                this.TableTwo.Alias += "2";
                this.BaseConstruction();
            }

            this.ValidateTable(this.TableOne);
            this.ValidateTable(this.TableTwo);

            builder.AppendLine(GetSelectPart());
            builder.AppendLine($"  FROM [dbo].{this.TableOne.Name} AS {this.TableOne.Alias}");
            builder.AppendLine("{{{joinType}}} JOIN" + $" {this.TableTwo.Name} AS {this.TableTwo.Alias}");
            builder.AppendLine($"    ON {this.TableOne.Alias}.[{this.TableOneJoinColumn}] = {this.TableTwo.Alias}.[{this.TableTwoJoinColumn}]");


            return builder.ToString().TrimEnd();
        }

        private string GetSelectPart()
        {
            var selectPart = string.Empty;

            if (this.TableOne.Columns.Count() == 0 &&
    this.TableTwo.Columns.Count() == 0)
            {
                selectPart = "SELECT *";
            }
            else if (this.TableOne.Columns.Count() == 0)
            {
                selectPart = $"SELECT {string.Join($"{Environment.NewLine}      ,", this.TableTwo.Columns)}";
            }
            else if (this.TableTwo.Columns.Count() == 0)
            {
                selectPart = $"SELECT {string.Join($"{Environment.NewLine}      ,", this.TableOne.Columns)}";
            }
            else
            {
                selectPart = $"SELECT {string.Join($"{Environment.NewLine}      ,", this.TableOne.Columns)}";
                selectPart = selectPart + $"{Environment.NewLine}      ,";
                selectPart = selectPart + $"{string.Join($"{Environment.NewLine}      ,", this.TableTwo.Columns)}";
            }

            return selectPart;
        }

        private void ValidateTable(SQLTable table)
        {
            table.Name = TablePropertiesValidator.ReturnValidName(table.Name);
            table.Alias = TablePropertiesValidator.ValidateOrCreateAlias(table.Alias, table.Name);
            table.Columns = TablePropertiesValidator.ValidateColumns(table.Columns, table.Alias);
        }
    }
}
