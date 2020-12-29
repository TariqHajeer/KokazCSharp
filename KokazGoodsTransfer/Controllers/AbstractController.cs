using AutoMapper;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers
{
    [EnableCors("EnableCORS")]

    public abstract class AbstractController : ControllerBase
    {
        protected KokazContext Context;
        protected IMapper mapper;
        public AbstractController(KokazContext context, IMapper mapper)
        {
            this.Context = context;
            this.mapper = mapper;
        }

    }
}