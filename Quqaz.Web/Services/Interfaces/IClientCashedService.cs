using Quqaz.Web.Dtos.Clients;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Models;
using Quqaz.Web.Services.Helper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quqaz.Web.Services.Interfaces
{
    public interface IClientCashedService : ICashService<Client, ClientDto, CreateClientDto, UpdateClientDto>
    {
        Task<ErrorRepsonse<PhoneDto>> AddPhone(AddPhoneDto addPhoneDto);
        Task DeletePhone(int id);
        Task<ErrorRepsonse<ClientDto>> GivePoints(GiveOrDiscountPointsDto giveOrDiscountPointsDto);
        Task<int>Account(AccountDto accountDto);
        Task<AuthClient> GetAuthClient();
        Task Update(CUpdateClientDto cUpdateClientDto);
        Task<List<ClientDto>> GetClientsByBranchId(int branchId);   
        Task UpdatePassword(UpdatePasswordDto updatePasswordDto);
        Task SetToken(SetFCMTokenDto setFCMToken);
    }
}
