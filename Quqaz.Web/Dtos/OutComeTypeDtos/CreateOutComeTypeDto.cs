using Quqaz.Web.Models.Infrastrcuter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Dtos.OutComeTypeDtos
{
    public class CreateOutComeTypeDto: INameEntity
    {
        public string Name { get; set; }
    }
}
