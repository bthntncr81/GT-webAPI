    using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GTBack.Core.Results
{
    public interface IResults
    {
        bool Success { get; }
        string Message { get; }
        HttpStatusCode StatusCode { get; }
        Dictionary<string, string> ModelStateErrors { get; }
        IEnumerable<string> Messages { get; set; }
        public Type ExceptionType { get; }
        public int? TotalCount { get; set; }
    }
}
