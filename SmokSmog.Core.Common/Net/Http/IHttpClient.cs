using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace SmokSmog.Net.Http
{
    public interface IHttpClient
    {
        // Summary: Gets or sets the base address of Uniform Resource Identifier (URI) of the
        // Internet resource used when sending requests.
        //
        // Returns: Returns System.Uri.The base address of Uniform Resource Identifier (URI) of the
        // Internet resource used when sending requests.
        Uri BaseAddress { get; set; }

        // Summary: Gets the headers which should be sent with each request.
        //
        // Returns: Returns System.Net.Http.Headers.HttpRequestHeaders.The headers which should be
        // sent with each request.
        HttpRequestHeaders DefaultRequestHeaders { get; }

        // Summary: Gets or sets the maximum number of bytes to buffer when reading the response content.
        //
        // Returns: Returns System.Int32.The maximum number of bytes to buffer when reading the
        // response content. The default value for this property is 2 gigabytes.
        //
        // Exceptions: T:System.ArgumentOutOfRangeException: The size specified is less than or equal
        // to zero.
        //
        // T:System.InvalidOperationException: An operation has already been started on the current instance.
        //
        // T:System.ObjectDisposedException: The current instance has been disposed.
        long MaxResponseContentBufferSize { get; set; }

        // Summary: Gets or sets the number of milliseconds to wait before the request times out.
        //
        // Returns: Returns System.TimeSpan.The number of milliseconds to wait before the request
        // times out.
        //
        // Exceptions: T:System.ArgumentOutOfRangeException: The timeout specified is less than or
        // equal to zero and is not System.Threading.Timeout.Infinite.
        //
        // T:System.InvalidOperationException: An operation has already been started on the current instance.
        //
        // T:System.ObjectDisposedException: The current instance has been disposed.
        TimeSpan Timeout { get; set; }

        // Summary: Cancel all pending requests on this instance.
        void CancelPendingRequests();

        // Summary: Send a DELETE request to the specified Uri as an asynchronous operation.
        //
        // Parameters: requestUri: The Uri the request is sent to.
        //
        // Returns: Returns System.Threading.Tasks.Task`1.The task object representing the
        // asynchronous operation.
        //
        // Exceptions: T:System.ArgumentNullException: The requestUri was null.
        //
        // T:System.InvalidOperationException: The request message was already sent by the
        // System.Net.Http.HttpClient instance.
        Task<HttpResponseMessage> DeleteAsync(string requestUri);

        // Summary: Send a DELETE request to the specified Uri as an asynchronous operation.
        //
        // Parameters: requestUri: The Uri the request is sent to.
        //
        // Returns: Returns System.Threading.Tasks.Task`1.The task object representing the
        // asynchronous operation.
        //
        // Exceptions: T:System.ArgumentNullException: The requestUri was null.
        //
        // T:System.InvalidOperationException: The request message was already sent by the
        // System.Net.Http.HttpClient instance.
        Task<HttpResponseMessage> DeleteAsync(Uri requestUri);

        // Summary: Send a DELETE request to the specified Uri with a cancellation token as an
        // asynchronous operation.
        //
        // Parameters: requestUri: The Uri the request is sent to.
        //
        // cancellationToken: A cancellation token that can be used by other objects or threads to
        // receive notice of cancellation.
        //
        // Returns: Returns System.Threading.Tasks.Task`1.The task object representing the
        // asynchronous operation.
        //
        // Exceptions: T:System.ArgumentNullException: The requestUri was null.
        //
        // T:System.InvalidOperationException: The request message was already sent by the
        // System.Net.Http.HttpClient instance.
        Task<HttpResponseMessage> DeleteAsync(Uri requestUri, CancellationToken cancellationToken);

        // Summary: Send a DELETE request to the specified Uri with a cancellation token as an
        // asynchronous operation.
        //
        // Parameters: requestUri: The Uri the request is sent to.
        //
        // cancellationToken: A cancellation token that can be used by other objects or threads to
        // receive notice of cancellation.
        //
        // Returns: Returns System.Threading.Tasks.Task`1.The task object representing the
        // asynchronous operation.
        //
        // Exceptions: T:System.ArgumentNullException: The requestUri was null.
        //
        // T:System.InvalidOperationException: The request message was already sent by the
        // System.Net.Http.HttpClient instance.
        Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken);

        // Summary: Send a GET request to the specified Uri as an asynchronous operation.
        //
        // Parameters: requestUri: The Uri the request is sent to.
        //
        // Returns: Returns System.Threading.Tasks.Task`1.The task object representing the
        // asynchronous operation.
        //
        // Exceptions: T:System.ArgumentNullException: The requestUri was null.
        Task<HttpResponseMessage> GetAsync(string requestUri);

        // Summary: Send a GET request to the specified Uri as an asynchronous operation.
        //
        // Parameters: requestUri: The Uri the request is sent to.
        //
        // Returns: Returns System.Threading.Tasks.Task`1.The task object representing the
        // asynchronous operation.
        //
        // Exceptions: T:System.ArgumentNullException: The requestUri was null.
        Task<HttpResponseMessage> GetAsync(Uri requestUri);

        // Summary: Send a GET request to the specified Uri with an HTTP completion option as an
        // asynchronous operation.
        //
        // Parameters: requestUri: The Uri the request is sent to.
        //
        // completionOption: An HTTP completion option value that indicates when the operation should
        // be considered completed.
        //
        // Returns: Returns System.Threading.Tasks.Task`1.
        //
        // Exceptions: T:System.ArgumentNullException: The requestUri was null.
        Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption);

        // Summary: Send a GET request to the specified Uri with a cancellation token as an
        // asynchronous operation.
        //
        // Parameters: requestUri: The Uri the request is sent to.
        //
        // cancellationToken: A cancellation token that can be used by other objects or threads to
        // receive notice of cancellation.
        //
        // Returns: Returns System.Threading.Tasks.Task`1.
        //
        // Exceptions: T:System.ArgumentNullException: The requestUri was null.
        Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken);

        // Summary: Send a GET request to the specified Uri with an HTTP completion option as an
        // asynchronous operation.
        //
        // Parameters: requestUri: The Uri the request is sent to.
        //
        // completionOption: An HTTP completion option value that indicates when the operation should
        // be considered completed.
        //
        // Returns: Returns System.Threading.Tasks.Task`1.The task object representing the
        // asynchronous operation.
        //
        // Exceptions: T:System.ArgumentNullException: The requestUri was null.
        Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption);

        // Summary: Send a GET request to the specified Uri with a cancellation token as an
        // asynchronous operation.
        //
        // Parameters: requestUri: The Uri the request is sent to.
        //
        // cancellationToken: A cancellation token that can be used by other objects or threads to
        // receive notice of cancellation.
        //
        // Returns: Returns System.Threading.Tasks.Task`1.The task object representing the
        // asynchronous operation.
        //
        // Exceptions: T:System.ArgumentNullException: The requestUri was null.
        Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken);

        // Summary: Send a GET request to the specified Uri with an HTTP completion option and a
        // cancellation token as an asynchronous operation.
        //
        // Parameters: requestUri: The Uri the request is sent to.
        //
        // completionOption: An HTTP completion option value that indicates when the operation should
        // be considered completed.
        //
        // cancellationToken: A cancellation token that can be used by other objects or threads to
        // receive notice of cancellation.
        //
        // Returns: Returns System.Threading.Tasks.Task`1.
        //
        // Exceptions: T:System.ArgumentNullException: The requestUri was null.
        Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken);

        // Summary: Send a GET request to the specified Uri with an HTTP completion option and a
        // cancellation token as an asynchronous operation.
        //
        // Parameters: requestUri: The Uri the request is sent to.
        //
        // completionOption: An HTTP completion option value that indicates when the operation should
        // be considered completed.
        //
        // cancellationToken: A cancellation token that can be used by other objects or threads to
        // receive notice of cancellation.
        //
        // Returns: Returns System.Threading.Tasks.Task`1.The task object representing the
        // asynchronous operation.
        //
        // Exceptions: T:System.ArgumentNullException: The requestUri was null.
        Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken);

        // Summary: Send a GET request to the specified Uri and return the response body as a byte
        // array in an asynchronous operation.
        //
        // Parameters: requestUri: The Uri the request is sent to.
        //
        // Returns: Returns System.Threading.Tasks.Task`1.The task object representing the
        // asynchronous operation.
        //
        // Exceptions: T:System.ArgumentNullException: The requestUri was null.
        Task<byte[]> GetByteArrayAsync(string requestUri);

        // Summary: Send a GET request to the specified Uri and return the response body as a byte
        // array in an asynchronous operation.
        //
        // Parameters: requestUri: The Uri the request is sent to.
        //
        // Returns: Returns System.Threading.Tasks.Task`1.The task object representing the
        // asynchronous operation.
        //
        // Exceptions: T:System.ArgumentNullException: The requestUri was null.
        Task<byte[]> GetByteArrayAsync(Uri requestUri);

        // Summary: Send a GET request to the specified Uri and return the response body as a stream
        // in an asynchronous operation.
        //
        // Parameters: requestUri: The Uri the request is sent to.
        //
        // Returns: Returns System.Threading.Tasks.Task`1.The task object representing the
        // asynchronous operation.
        //
        // Exceptions: T:System.ArgumentNullException: The requestUri was null.
        Task<Stream> GetStreamAsync(string requestUri);

        // Summary: Send a GET request to the specified Uri and return the response body as a stream
        // in an asynchronous operation.
        //
        // Parameters: requestUri: The Uri the request is sent to.
        //
        // Returns: Returns System.Threading.Tasks.Task`1.The task object representing the
        // asynchronous operation.
        //
        // Exceptions: T:System.ArgumentNullException: The requestUri was null.
        Task<Stream> GetStreamAsync(Uri requestUri);

        // Summary: Send a GET request to the specified Uri and return the response body as a string
        // in an asynchronous operation.
        //
        // Parameters: requestUri: The Uri the request is sent to.
        //
        // Returns: Returns System.Threading.Tasks.Task`1.The task object representing the
        // asynchronous operation.
        //
        // Exceptions: T:System.ArgumentNullException: The requestUri was null.
        Task<string> GetStringAsync(string requestUri);

        // Summary: Send a GET request to the specified Uri and return the response body as a string
        // in an asynchronous operation.
        //
        // Parameters: requestUri: The Uri the request is sent to.
        //
        // Returns: Returns System.Threading.Tasks.Task`1.The task object representing the
        // asynchronous operation.
        //
        // Exceptions: T:System.ArgumentNullException: The requestUri was null.
        Task<string> GetStringAsync(Uri requestUri);

        // Summary: Send a POST request to the specified Uri as an asynchronous operation.
        //
        // Parameters: requestUri: The Uri the request is sent to.
        //
        // content: The HTTP request content sent to the server.
        //
        // Returns: Returns System.Threading.Tasks.Task`1.The task object representing the
        // asynchronous operation.
        //
        // Exceptions: T:System.ArgumentNullException: The requestUri was null.
        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);

        // Summary: Send a POST request to the specified Uri as an asynchronous operation.
        //
        // Parameters: requestUri: The Uri the request is sent to.
        //
        // content: The HTTP request content sent to the server.
        //
        // Returns: Returns System.Threading.Tasks.Task`1.The task object representing the
        // asynchronous operation.
        //
        // Exceptions: T:System.ArgumentNullException: The requestUri was null.
        Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content);

        // Summary: Send a POST request with a cancellation token as an asynchronous operation.
        //
        // Parameters: requestUri: The Uri the request is sent to.
        //
        // content: The HTTP request content sent to the server.
        //
        // cancellationToken: A cancellation token that can be used by other objects or threads to
        // receive notice of cancellation.
        //
        // Returns: Returns System.Threading.Tasks.Task`1.The task object representing the
        // asynchronous operation.
        //
        // Exceptions: T:System.ArgumentNullException: The requestUri was null.
        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, CancellationToken cancellationToken);

        // Summary: Send a POST request with a cancellation token as an asynchronous operation.
        //
        // Parameters: requestUri: The Uri the request is sent to.
        //
        // content: The HTTP request content sent to the server.
        //
        // cancellationToken: A cancellation token that can be used by other objects or threads to
        // receive notice of cancellation.
        //
        // Returns: Returns System.Threading.Tasks.Task`1.The task object representing the
        // asynchronous operation.
        //
        // Exceptions: T:System.ArgumentNullException: The requestUri was null.
        Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken);

        // Summary: Send a PUT request to the specified Uri as an asynchronous operation.
        //
        // Parameters: requestUri: The Uri the request is sent to.
        //
        // content: The HTTP request content sent to the server.
        //
        // Returns: Returns System.Threading.Tasks.Task`1.The task object representing the
        // asynchronous operation.
        //
        // Exceptions: T:System.ArgumentNullException: The requestUri was null.
        Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content);

        // Summary: Send a PUT request to the specified Uri as an asynchronous operation.
        //
        // Parameters: requestUri: The Uri the request is sent to.
        //
        // content: The HTTP request content sent to the server.
        //
        // Returns: Returns System.Threading.Tasks.Task`1.The task object representing the
        // asynchronous operation.
        //
        // Exceptions: T:System.ArgumentNullException: The requestUri was null.
        Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content);

        // Summary: Send a PUT request with a cancellation token as an asynchronous operation.
        //
        // Parameters: requestUri: The Uri the request is sent to.
        //
        // content: The HTTP request content sent to the server.
        //
        // cancellationToken: A cancellation token that can be used by other objects or threads to
        // receive notice of cancellation.
        //
        // Returns: Returns System.Threading.Tasks.Task`1.The task object representing the
        // asynchronous operation.
        //
        // Exceptions: T:System.ArgumentNullException: The requestUri was null.
        Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content, CancellationToken cancellationToken);

        // Summary: Send a PUT request with a cancellation token as an asynchronous operation.
        //
        // Parameters: requestUri: The Uri the request is sent to.
        //
        // content: The HTTP request content sent to the server.
        //
        // cancellationToken: A cancellation token that can be used by other objects or threads to
        // receive notice of cancellation.
        //
        // Returns: Returns System.Threading.Tasks.Task`1.The task object representing the
        // asynchronous operation.
        //
        // Exceptions: T:System.ArgumentNullException: The requestUri was null.
        Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken);

        // Summary: Send an HTTP request as an asynchronous operation.
        //
        // Parameters: request: The HTTP request message to send.
        //
        // Returns: Returns System.Threading.Tasks.Task`1.The task object representing the
        // asynchronous operation.
        //
        // Exceptions: T:System.ArgumentNullException: The request was null.
        //
        // T:System.InvalidOperationException: The request message was already sent by the
        // System.Net.Http.HttpClient instance.
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);

        // Summary: Send an HTTP request as an asynchronous operation.
        //
        // Parameters: request: The HTTP request message to send.
        //
        // completionOption: When the operation should complete (as soon as a response is available
        // or after reading the whole response content).
        //
        // Returns: Returns System.Threading.Tasks.Task`1.The task object representing the
        // asynchronous operation.
        //
        // Exceptions: T:System.ArgumentNullException: The request was null.
        //
        // T:System.InvalidOperationException: The request message was already sent by the
        // System.Net.Http.HttpClient instance.
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption);

        // Summary: Send an HTTP request as an asynchronous operation.
        //
        // Parameters: request: The HTTP request message to send.
        //
        // cancellationToken: The cancellation token to cancel operation.
        //
        // Returns: Returns System.Threading.Tasks.Task`1.The task object representing the
        // asynchronous operation.
        //
        // Exceptions: T:System.ArgumentNullException: The request was null.
        //
        // T:System.InvalidOperationException: The request message was already sent by the
        // System.Net.Http.HttpClient instance.
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);

        // Summary: Send an HTTP request as an asynchronous operation.
        //
        // Parameters: request: The HTTP request message to send.
        //
        // completionOption: When the operation should complete (as soon as a response is available
        // or after reading the whole response content).
        //
        // cancellationToken: The cancellation token to cancel operation.
        //
        // Returns: Returns System.Threading.Tasks.Task`1.The task object representing the
        // asynchronous operation.
        //
        // Exceptions: T:System.ArgumentNullException: The request was null.
        //
        // T:System.InvalidOperationException: The request message was already sent by the
        // System.Net.Http.HttpClient instance.
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken);
    }
}