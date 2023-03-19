using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.Users;
using Quqaz.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Services.Interfaces
{
    public interface IUserCashedService : ICashService<User, UserDto, CreateUserDto, UpdateUserDto>
    {
        Task DeletePhone(int id);
        Task DeleteGroup(int userId, int groupId);
        Task<PhoneDto> AddPhone(AddPhoneDto addPhoneDto);
        Task AddToGroup(int userId, int groupId);
        Task<UserDto> AddAsync2(CreateUserDto createDto);
    }
}
