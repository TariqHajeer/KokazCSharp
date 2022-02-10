using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.Users;
using KokazGoodsTransfer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IUserCashedService : ICashService<User, UserDto, CreateUserDto, UpdateUserDto>
    {
        Task<UserDto> GetById(int id);
        Task DeletePhone(int id);
        Task DeleteGroup(int userId, int groupId);
        Task<PhoneDto> AddPhone(AddPhoneDto addPhoneDto);
        Task AddToGroup(int userId, int groupId);
    }
}
