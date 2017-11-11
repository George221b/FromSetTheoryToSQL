namespace SQLSimpleInterpreter.Helpers
{
    using Exceptions;

    internal static class JoinQueriesValidator
    {
        private const string NoTableNameErrorException = "Table names cannot be null or whitespace/s.";
        private const string SameTableNamesErrorMessage = "You cannot join tables with the same name.";

        public static void ThrowIfInvalidTableNames(string firstTableName, string secondTableName)
        {
            if (string.IsNullOrWhiteSpace(firstTableName) ||
                string.IsNullOrWhiteSpace(secondTableName))
            {
                throw new SQLJoinException(NoTableNameErrorException);
            }

            if (firstTableName.Equals(secondTableName))
            {
                throw new SQLJoinException(SameTableNamesErrorMessage);
            }
        }

        public static bool AreAliasEqual(string aliasTableOne, string aliasTableTwo)
        {
            return aliasTableOne.Equals(aliasTableTwo);
        }
    }
}
