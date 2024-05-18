using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GTBack.Core.Results
{
    public class SuccessResult : Result
    {

        public SuccessResult() : base(true)
        {

        }

        public SuccessResult(IEnumerable<string> messages) : base(true, messages)
        {

        }

        public SuccessResult(string message) : base(false, message, HttpStatusCode.OK)
        {
        }

        public SuccessResult(HttpStatusCode statusCode) : base(true, statusCode)
        {

        }
        public SuccessResult(string message, HttpStatusCode statusCode) : base(true, message, statusCode)
        {
        }

    }
}
