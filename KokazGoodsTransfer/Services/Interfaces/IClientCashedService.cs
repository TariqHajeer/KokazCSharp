using KokazGoodsTransfer.Dtos.Clients;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IClientCashedService : ICashService<Client, ClientDto, CreateClientDto, UpdateClientDto>
    {
        Task<ErrorRepsonse<PhoneDto>> AddPhone(AddPhoneDto addPhoneDto);
        Task DeletePhone(int id);
        Task<ErrorRepsonse<ClientDto>> GivePoints(GiveOrDiscountPointsDto giveOrDiscountPointsDto);
        Task<int>Account(AccountDto accountDto);
    }
}
