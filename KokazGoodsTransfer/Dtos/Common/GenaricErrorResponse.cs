using System.Collections.Generic;
namespace KokazGoodsTransfer.Dtos.Common
{
    public class GenaricErrorResponse<T, DictionaryKey, DictionaryValue> : ErrorResponse<DictionaryKey, DictionaryValue>
    {
        public GenaricErrorResponse(T data)
        {
            Data = data;
        }
        public GenaricErrorResponse(string error):base(error)
        {
        }
        public GenaricErrorResponse(IEnumerable<string> errors):base(errors)
        {
        }
        public GenaricErrorResponse(Dictionary<DictionaryKey, DictionaryValue> dictionaryErrors):base(dictionaryErrors)
        {
        }
        public GenaricErrorResponse()
        {

        }
        public T Data { get; set; }
        
    }
}
