namespace SQLSimpleInterpreter.Exceptions
{
    using System;

    internal class SQLInvalidTableException : Exception
    {
        private const string DefaultMessage = "Invalid SQL table operation.";

        public SQLInvalidTableException() : base(DefaultMessage)
        {
        }

        public SQLInvalidTableException(string message) : base(message)
        {
        }
    }
}
