using Quqaz.Web.Dtos.Clients;
using Quqaz.Web.Dtos.Countries;
using Quqaz.Web.Migrations;
using System.Collections.Generic;

namespace Quqaz.Web.Dtos.Common
{
    public class IndexListDto
    {
        public IEnumerable<CountryDto> Countries { get; set; }
        public IEnumerable<ClientDto> Clients { get; set; }
        public IEnumerable<NameAndIdDto> Benaches { get; set; }
    }
}
