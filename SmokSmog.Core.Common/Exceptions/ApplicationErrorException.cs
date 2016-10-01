namespace SmokSmog.Exceptions
{
    public class ApplicationException : System.Exception
    {
        public ApplicationException()
        {
        }

        public ApplicationException(string message) : base(message)
        {
        }

        public ApplicationException(string message, System.Exception inner) : base(message, inner)
        {
        }
    }
}