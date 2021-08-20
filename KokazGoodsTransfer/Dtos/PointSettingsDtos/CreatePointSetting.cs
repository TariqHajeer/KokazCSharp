using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.PointSettingsDtos
{
    public class CreatePointSetting
    {
        public int Points { get; set; }
        public decimal Money { get; set; }
    }
}
