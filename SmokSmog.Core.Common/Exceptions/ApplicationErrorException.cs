namespace SmokSmog.Exceptions
{
    //[System.Serializable]
    public class ApplicationErrorException : System.Exception
    {
        public ApplicationErrorException()
        {
        }

        public ApplicationErrorException(string message) : base(message)
        {
        }

        public ApplicationErrorException(string message, System.Exception inner) : base(message, inner)
        {
        }

        //protected ErrorException(
        //  System.Runtime.Serialization.SerializationInfo info,
        //  System.Runtime.Serialization.StreamingContext context) : base(info, context)
        //{ }
    }
}