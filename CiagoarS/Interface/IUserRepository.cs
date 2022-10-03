using Ciagoar.Data.Enums;
using Ciagoar.Data.Response.Users;
using CiagoarS.DataBase;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CiagoarS.Interface
{
    public interface IUserRepository
    {
        Task<Ci_User> GetUserByEmailPWAsync(string Email, string Password);

        Task<Tuple<Ci_User, string>> GetUserAuthkeyByEmailAsync(string Email);

        Task<List<AuthInfo>> GetAuthInfoByTypeAsync(AuthType Type);

        Task<bool> CheckUserByEmailAsync(string Email);
    }
}
