using System;
using System.Collections.Generic;
namespace KokazGoodsTransfer.CustomException
{
    public class ConfilectException:Exception
    {
        IEnumerable<string> errors;
        public ConfliectErrorMessage Errors
        {
            get { return new ConfliectErrorMessage( errors); }
        }
        public ConfilectException( string error)
        {
            var errors = new List<string>
            {
                error
            };
            this.errors= errors;
        }
        public ConfilectException(IEnumerable<string> errors)
        {
            this.errors = errors;
        }
        public class ConfliectErrorMessage
        {
            public ConfliectErrorMessage(IEnumerable<string> errors)
            {
                Errors = errors;
            }
            public IEnumerable<string> Errors { get; set; }
        }
    }
    
}
