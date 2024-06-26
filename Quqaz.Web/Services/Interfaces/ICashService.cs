﻿using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Models.Infrastrcuter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Services.Interfaces
{
    public interface ICashService<TEntity, TDTO, CreateDto, UpdateDto> : ICRUDService<TEntity, TDTO, CreateDto, UpdateDto> where TEntity : class, IIdEntity where TDTO : class where CreateDto : class where UpdateDto : class
    {
        Task<IEnumerable<TDTO>> GetCashed();
        void RemoveCash();
    }
}
