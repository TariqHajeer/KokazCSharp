using System;
using System.Collections.Generic;
namespace KokazGoodsTransfer.CustomException
{
    public class ConfilectException:Exception
    {
        IEnumerable<string> errors;
        public IEnumerable<string> Errors
        {
            get { return errors; }
        }
        public ConfilectException( string error)
        {
            var errors = new List<string>();
            errors.Add(error);
            this.errors= errors;
        }
        public ConfilectException(IEnumerable<string> errors)
        {
            this.errors = errors;
        }
    }
}
