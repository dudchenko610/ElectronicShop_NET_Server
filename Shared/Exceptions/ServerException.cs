using System;
using System.Collections.Generic;
using System.Net;

namespace Shared.Exceptions
{
    public class ServerException : Exception
    {
        public HttpStatusCode Code { get; set; }
        public List<string> ErrorMessages { get; set; }
        public ServerException(string error, HttpStatusCode errorCode = HttpStatusCode.BadRequest)
        {
            Code = errorCode;
            ErrorMessages = new List<string>();
            ErrorMessages.Add(error);
        }

        public ServerException(List<string> errorMessages, HttpStatusCode errorCode = HttpStatusCode.BadRequest)
        {
            ErrorMessages = new List<string>(errorMessages);
            Code = errorCode;
        }

    }
}
