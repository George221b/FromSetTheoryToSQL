namespace SQLSimpleInterpreter
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class ColumnCollection : IEnumerable<string>
    {
        private const string ErrorRemoveMessage = "You cannot remove a column, which is not present in the ColumnCollection.";

        private List<string> columns;

        /// <summary>
        /// Creates an intance of ColumnCollection. This can be passed to SQLTable or SQLJoin.
        /// </summary>
        /// <param name="columnNames">The names of the columns.</param>
        public ColumnCollection(params string[] columnNames)
        {
            this.columns = new List<string>(columnNames);
        }

        /// <summary>
        /// Adds a column to the already existing ones.
        /// </summary>
        /// <param name="column">Name of the column</param>
        public void Add(string column)
        {
            this.columns.Add(column);
        }

        /// <summary>
        /// Deletes a column from the already existing ones.
        /// </summary>
        /// <param name="column">Column name to remove.</param>
        public void Remove(string column)
        {
            if (!this.columns.Contains(column))
            {
                throw new InvalidOperationException(ErrorRemoveMessage);
            }

            this.columns.Remove(column);
        }

        /// <summary>
        /// Returns the count of the columns in the collection.
        /// </summary>
        /// <returns>int</returns>
        public int Count()
        {
            return this.columns.Count;
        }

        public IEnumerator<string> GetEnumerator()
            => this.columns.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();
    }
}
