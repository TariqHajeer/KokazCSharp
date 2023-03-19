using Quqaz.Web.Models.Infrastrcuter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Dtos.IncomeTypes
{
    public class CreateIncomeTypeDto: INameEntity
    {
        public string Name { get; set; }
    }
}
