using Ciagoar.Core.Helper;
using Ciagoar.Data.Enums;
using Ciagoar.Data.HTTPS;
using Ciagoar.Data.Request.Users;
using Ciagoar.Data.Response;
using Ciagoar.Data.Response.Users;
using CiagoarS.Common;
using CiagoarS.Controllers;
using CiagoarS.DataBase;
using CiagoarS.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CiagoarS.Repositorys
{
    public class UserRepository : IUserRepository
    {
        protected CiagoarContext mContext;
        public UserRepository(CiagoarContext Context)
        {
            mContext = Context;
        }

        public async Task<bool> CheckUserByEmailAsync(string Email)
        {
            try
            {
                return await mContext.UserInfos.AnyAsync(p => p.Email.Equals(Email) && (bool)p.Isuse && !p.Isdelete);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UserInfo> GetUserByEmailAsync(string Email)
        {
            try
            {
                return await mContext.UserInfos.FirstAsync(p => p.Email.Equals(Email) && (bool)p.Isuse && !p.Isdelete);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UserAuth> GetUserAuthsAsync(int UserInfoID, short TypeCode)
        {
            try
            {
                return await mContext.UserAuths.FirstOrDefaultAsync(p => p.UserInfoId == UserInfoID && p.TypeCode == TypeCode);


            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UserAuth> GetUserAuthsByEmailAsync(string Email, short TypeCode)
        {
            try
            {
                return await (from UAuthentications in mContext.UserAuths.Where(p => p.TypeCode == (int)TypeCode && (bool)p.Isuse && !p.Isdelete)
                              join UInfo in mContext.UserInfos.Where(p => p.Email.Equals(Email) && (bool)p.Isuse && !p.Isdelete)
                              on UAuthentications.UserInfoId equals UInfo.Id
                              select UAuthentications
                              ).FirstOrDefaultAsync();


            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<AuthInfo>> GetSMTPInfoAsync()
        {
            try
            {
                return await mContext.AuthInfos.Where(p => p.TypeCode == (short)AuthType.EM).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }



        public async Task<int> UpdateAuthKeyAsync(int UserAuthID, string UpdateAuthKey)
        {
            try
            {
                if (await mContext.UserAuths.FindAsync(UserAuthID) is UserAuth Auth)
                {
                    Auth.AuthKey = UpdateAuthKey;
                    return await mContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return -1;
        }

        public async Task<int> UpdateAuthStepAsync(int UserAuthID, short UpdateAuthStep)
        {
            try
            {
                if (await mContext.UserAuths.FindAsync(UserAuthID) is UserAuth Auth)
                {
                    Auth.AuthStep = UpdateAuthStep;
                    return await mContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return -1;
        }


        public async Task<List<AuthInfo>> GetAuthInfoByTypeAsync(AuthType Type)
        {
            try
            {
                return await mContext.AuthInfos.Where(p => p.TypeCode == (short)AuthType.GG).ToListAsync();
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
                return await (from UInfo in mContext.UserInfos.Where(p => p.Email.Equals(Email) && (bool)p.Isuse && !p.Isdelete)
                              join UAuthentications in mContext.UserAuths.Where(p => p.TypeCode == (int)AuthType.GG && (bool)p.Isuse && !p.Isdelete)
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
                return await (from UInfo in mContext.UserInfos.Where(p => p.Email.Equals(Email) && (bool)p.Isuse && !p.Isdelete)
                              join UAuthentications in mContext.UserAuths.Where(p => p.AuthKey.Equals(Password) && p.TypeCode == (int)AuthType.EM && (bool)p.Isuse && !p.Isdelete)
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


        public async Task<Ci_User> InsertUserInfoAsync(UserInfo InsertUserInfo)
        {

            IDbContextTransaction Transaction = mContext.Database.BeginTransaction();
            Ci_User response = null;
            try
            {
                mContext.UserInfos.Add(InsertUserInfo);
                mContext.SaveChanges();

                response = await (from UInfo in mContext.UserInfos.Where(p => p.Email.Equals(InsertUserInfo.Email) && (bool)p.Isuse && !p.Isdelete)
                                  select new Ci_User()
                                  {
                                      TypeCode = UInfo.TypeCode,
                                      Email = UInfo.Email,
                                      Nickname = UInfo.Nickname,
                                      AuthStep = 0
                                  }).FirstOrDefaultAsync();

                Transaction.Commit();

            }
            catch (Exception)
            {
                Transaction.Rollback();
                response = null;
                throw;
            }

            return response;
        }


        public async Task<int> InsertUserAuthAsync(UserAuth InsertUserAuthData)
        {
            try
            {
                mContext.UserAuths.Add(InsertUserAuthData);
                return await mContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }





    }
}
