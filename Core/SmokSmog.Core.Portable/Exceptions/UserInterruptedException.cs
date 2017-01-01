namespace SmokSmog.Exceptions
{
    //[System.Serializable]
    public class UserInterruptedException : System.Exception
    {
        public UserInterruptedException()
        {
        }

        public UserInterruptedException(string message) : base(message)
        {
        }

        public UserInterruptedException(string message, System.Exception inner) : base(message, inner)
        {
        }

        //protected UserInterruptedException(
        //  System.Runtime.Serialization.SerializationInfo info,
        //  System.Runtime.Serialization.StreamingContext context) : base(info, context)
        //{ }
    }
}