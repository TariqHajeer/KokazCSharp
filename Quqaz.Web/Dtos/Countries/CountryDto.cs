using Quqaz.Web.Dtos.Common;
using System;
using System.Collections.Generic;

namespace Quqaz.Web.Dtos.Countries
{
    public class CountryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal DeliveryCost { get; set; }
        public bool CanDelete { get; set; }
        public bool CanDeleteWithRegion { get; set; }
        public Int16 Points { get; set; }
        public List<NameAndIdDto> Regions { get; set; }
        public List<NameAndIdDto> Agents { get; set; }
        public bool RequiredAgent { get; set; }

    }
}
