using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Helpers
{
    public class ErrorMessage
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public int Line { get; set; }
        public List<string> Messges { get; set; }
        public ExceptionDto Exception { get; set; }
        public ErrorMessage()
        {
            Messges = new List<string>();
            Exception = new ExceptionDto();
        }

    }
    public class ExceptionDto
    {
        public string Message { get; set; }
        public string InnerExceptionMessage { get; set; }
    }
}
