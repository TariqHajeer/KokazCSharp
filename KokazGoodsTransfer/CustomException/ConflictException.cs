using System;
using System.Collections.Generic;
namespace KokazGoodsTransfer.CustomException
{
    public class ConflictException:Exception
    {
        IEnumerable<string> errors;
        public ConfliectErrorMessage Errors
        {
            get { return new ConfliectErrorMessage( errors); }
        }
        public ConflictException( string error)
        {
            var errors = new List<string>
            {
                error
            };
            this.errors= errors;
        }
        public ConflictException(IEnumerable<string> errors)
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
