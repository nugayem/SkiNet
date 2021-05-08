using System;

namespace API.Error
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message?? GetDefaultMessageStatusCode(statusCode);
        }

        private string GetDefaultMessageStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400=> "A bad Request, you have made",
                401=> "Authorized, You are not",
                404=> "Resource found, it was not",
                500=> "Error are the path to the dark side. Error leands to Anger, Anger leads to Hate, Hate leads to Carrer Change found, it was not",
                _=>null
            };
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}