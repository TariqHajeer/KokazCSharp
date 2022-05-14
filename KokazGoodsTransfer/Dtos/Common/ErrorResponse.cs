using System.Collections.Generic;

namespace KokazGoodsTransfer.Dtos.Common
{
    public class ErrorResponse<DictionaryKey, DictionaryValue>
    {
        public ErrorResponse()
        {
            
        }
        public ErrorResponse(string error)
        {
            Error = error;
        }
        public ErrorResponse(IEnumerable<string> errors)
        {
            Errors = errors;
        }
        public ErrorResponse(Dictionary<DictionaryKey, DictionaryValue> dictionaryErrors)
        {
            DictionaryErrors = dictionaryErrors;
        }
        public string Error { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public Dictionary<DictionaryKey, DictionaryValue> DictionaryErrors { get; set; }
    }
}
