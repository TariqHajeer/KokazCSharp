using System.Collections.Generic;
using System.Linq;
namespace KokazGoodsTransfer.Dtos.Common
{
    public class GenaricErrorResponse<T, DictionaryKey, DictionaryValue> : ErrorResponse<DictionaryKey, DictionaryValue>
    {
        public GenaricErrorResponse(T data)
        {
            Data = data;
        }
        public GenaricErrorResponse(string error, bool conflict = false, bool badRequest = true) : base(error, conflict, badRequest)
        {
        }
        public GenaricErrorResponse(IEnumerable<string> errors, bool conflict = false, bool badRequest = true) : base(errors, conflict, badRequest)
        {
        }
        public GenaricErrorResponse(Dictionary<DictionaryKey, DictionaryValue> dictionaryErrors, bool conflict = false, bool badRequest = true) : base(dictionaryErrors, conflict, badRequest)
        {
        }
        public GenaricErrorResponse()
        {

        }
        public bool Success
        {
            get
            {
                return string.IsNullOrEmpty(Error) && !(Errors?.Any()==true) && !(DictionaryErrors?.Any()==true);
            }
        }
        public T Data { get; set; }

    }
}
