﻿using AutoMapper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.IncomeTypes;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Helper;
using KokazGoodsTransfer.Services.Interfaces;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Concret
{
    public class IncomeTypeSerivce : IndexCURDService<IncomeType, IncomeTypeDto, CreateIncomeTypeDto, UpdateIncomeTypeDto>, IIncomeTypeSerive
    {
        IRepository<Income> _incomeReposiory;
        public IncomeTypeSerivce(IRepository<IncomeType> repository, IRepository<Income> incomeReposiory ,IMapper mapper) : base(repository, mapper)
        {
            _incomeReposiory = incomeReposiory;
        }
        public override async Task<ErrorRepsonse<IncomeTypeDto>> Delete(int id)
        {
            var enrity = await _repository.GetById(id);
            var response = new ErrorRepsonse<IncomeTypeDto>();
            if (enrity == null)
            {
                response.NotFound = true;
                response.Errors.Add("Not.Found");
            }
            else
            {
                if (await _incomeReposiory.Any(c => c.IncomeTypeId == id))
                {
                    response.CantDelete = true;
                    response.Errors.Add("Cant.Delete");
                }
                else
                {
                    await _repository.Delete(enrity);
                    response.Data = _mapper.Map<IncomeTypeDto>(enrity);
                }
            }
            return response;
        }
    }
}