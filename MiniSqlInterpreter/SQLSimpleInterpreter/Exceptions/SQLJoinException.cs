namespace SQLSimpleInterpreter.Exceptions
{
    using System;

    internal class SQLJoinException : Exception
    {
        private const string DefaultMessage = "Join failed.";

        public SQLJoinException() : base(DefaultMessage)
        {
        }

        public SQLJoinException(string message) : base(message)
        {
        }
    }
}
