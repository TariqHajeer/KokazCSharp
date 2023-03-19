using System.Collections.Generic;

namespace Quqaz.Web.Dtos.Common
{
    public class ErrorResponse<DictionaryKey, DictionaryValue>
    {
        public ErrorResponse()
        {

        }
        public ErrorResponse(string error, bool conflict = false, bool badRequest = true)
        {
            Error = error;
            Conflict = conflict;
            BadRequest = badRequest;
        }
        public ErrorResponse(IEnumerable<string> errors, bool conflict = false, bool badRequest = true)
        {
            Errors = errors;
            Conflict = conflict;
            BadRequest = badRequest;
        }
        public ErrorResponse(Dictionary<DictionaryKey, DictionaryValue> dictionaryErrors, bool conflict = false, bool badRequest = true)
        {
            DictionaryErrors = dictionaryErrors;
            Conflict = conflict;
            BadRequest = badRequest;
        }
        public bool Conflict { get; set; }
        public bool BadRequest { get; set; }
        public string Error { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public Dictionary<DictionaryKey, DictionaryValue> DictionaryErrors { get; set; }
    }
}
