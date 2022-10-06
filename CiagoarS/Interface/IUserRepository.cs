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

        Task<UserInfo> GetUserByEmailAsync(string Email);

        Task<UserAuth> GetUserAuthsAsync(int UserInfoID, short TypeCode);

        Task<UserAuth> GetUserAuthsByEmailAsync(string Email, short TypeCode);

        Task<int> UpdateAuthKeyAsync(int UserAuthID, string AuthKey);

        Task<Tuple<Ci_User, string>> GetUserAuthkeyByEmailAsync(string Email);

        Task<List<AuthInfo>> GetAuthInfoByTypeAsync(AuthType Type);

        Task<bool> CheckUserByEmailAsync(string Email);

        Task<Ci_User> InsertUserInfoAsync(UserInfo userInfo);

        Task<int> InsertUserAuthAsync(UserAuth UserAuthData);

    }
}
