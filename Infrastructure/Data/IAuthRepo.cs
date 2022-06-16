using Core.Dtos;
using Core.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public interface IAuthRepo
    {
        Task<ActionResponse<ReturnUserDto>> Login(LoginDto user);
        Task<ReturnUserDto> Logout();
        Task<ActionResponse<ReturnUserDto>> CreateUser(CreateUserDto user);
    }
}
