using HotChocolate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.API.GraphQL.Exceptions
{
    public class AppException : IErrorFilter
    {
        public IError OnError(IError error)
        {
            var message = error.Exception?.InnerException != null ? error.Exception?.InnerException.Message : error.Exception.Message;
            return error.WithMessage(message);
        }
    }
}
