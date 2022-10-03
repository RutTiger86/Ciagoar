using Ciagoar.Data.Enums;
using Ciagoar.Data.Response.Users;
using CiagoarS.DataBase;
using CiagoarS.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CiagoarS.Repositorys
{
    public class UserRepository: IUserRepository
    {
        protected CiagoarContext _mContext;

        public UserRepository(CiagoarContext context)
        { 
            _mContext = context;
        }

        public async Task<bool> CheckUserByEmailAsync(string Email)
        {
            try
            {
                return await _mContext.UserInfos.AnyAsync(p => p.Email.Equals(Email) && (bool)p.Isuse && !p.Isdelete);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<AuthInfo>> GetAuthInfoByTypeAsync(AuthType Type)
        {
            try
            {
                return await _mContext.AuthInfos.Where(p => p.TypeCode == (short)AuthType.GG).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Tuple<Ci_User, string>> GetUserAuthkeyByEmailAsync(string Email)
        {
            try
            {
                return await (from UInfo in _mContext.UserInfos.Where(p => p.Email.Equals(Email) && (bool)p.Isuse && !p.Isdelete)
                             join UAuthentications in _mContext.UserAuths.Where(p => p.TypeCode == (int)AuthType.GG && (bool)p.Isuse && !p.Isdelete)
                             on UInfo.Id equals UAuthentications.UserInfoId
                             select new Tuple<Ci_User, string>(new Ci_User()
                             {
                                 TypeCode = UInfo.TypeCode,
                                 Email = UInfo.Email,
                                 Nickname = UInfo.Nickname,
                                 AuthStep = 0
                             }, UAuthentications.AuthKey)
                             ).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Ci_User> GetUserByEmailPWAsync(string Email, string Password)
        {
            try
            {
                return await (from UInfo in _mContext.UserInfos.Where(p => p.Email.Equals(Email) && (bool)p.Isuse && !p.Isdelete)
                        join UAuthentications in _mContext.UserAuths.Where(p => p.AuthKey.Equals(Password) && p.TypeCode == (int)AuthType.EM && (bool)p.Isuse && !p.Isdelete)
                        on UInfo.Id equals UAuthentications.UserInfoId
                        select new Ci_User()
                        {
                            TypeCode = UInfo.TypeCode,
                            Email = UInfo.Email,
                            Nickname = UInfo.Nickname,
                            AuthStep = UAuthentications.AuthStep
                        }).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
