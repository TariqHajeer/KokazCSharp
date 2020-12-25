using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Controllers
{

    [EnableCors("EnableCORS")]
    public abstract class AbstractController: ControllerBase
    {
        protected KokazContext Context;
        public AbstractController(KokazContext context)
        {
            this.Context = context;
        }
    }
}
