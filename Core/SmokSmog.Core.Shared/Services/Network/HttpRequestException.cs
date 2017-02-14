namespace SmokSmog.Services.Network
{
    public class HttpRequestException : System.Exception
    {
        public HttpRequestException()
        {
        }

        public HttpRequestException(string message) : base(message)
        {
        }

        public HttpRequestException(string message, System.Exception inner) : base(message, inner)
        {
        }
    }
}