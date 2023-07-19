using Quqaz.Web.DAL.Helper;
using System;
using System.Collections.Generic;

namespace Quqaz.Web.Dtos.TreasuryDtos
{
    public class TreasuryDto
    {
        public int Id { get; set; }
        public decimal Total { get; set; }
        public DateTime CreateOnUtc { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
        public PagingResualt<IEnumerable<TreasuryHistoryDto>> History { get; set; }
    }

}
