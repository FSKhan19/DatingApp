using System.Net;

namespace DatingApp.Backend.Models
{
    /// <summary>
    /// ResponseWrapperModel class with Data, Error, Status, httpStatusCode etc.
    /// </summary>
    [Serializable]
    public class ResponseWrapperModel
    {

        /// <summary>
        /// The Response Data
        /// </summary>
        public object? Data { get; set; }

        /// <summary>
        /// The Response Error
        /// </summary>
        public object? Errors { get; set; }

        /// <summary>
        /// The Response Success either pass or fail
        /// </summary>
        public bool Success { get; set; } 

        /// <summary>
        /// The Response Http Status Code
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
    }
}
