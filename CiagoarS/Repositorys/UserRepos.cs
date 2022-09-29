using Ciagoar.Data.Response.Users;
using CiagoarS.DataBase;
using Microsoft.Data.SqlClient;
using System;
using System.Linq;

namespace CiagoarS.Repositorys
{
    public class UserRepos
    {
        protected CiagoarContext _context;

        public UserRepos(CiagoarContext context)
        { 
            _context = context;
        }

        public Ci_User GetUser(string Email, string Password)
        {
            try
            {
                return (from UInfo in _context.UserInfos.Where(p => p.Email.Equals(Email) && (bool)p.Isuse && !p.Isdelete)
                        join UAuthentications in _context.UserAuths.Where(p => p.AuthKey.Equals(Password) && p.TypeCode == (int)AuthType.EM && (bool)p.Isuse && !p.Isdelete)
                        on UInfo.Id equals UAuthentications.UserInfoId
                        select new Ci_User()
                        {
                            TypeCode = UInfo.TypeCode,
                            Email = UInfo.Email,
                            Nickname = UInfo.Nickname,
                            AuthStep = UAuthentications.AuthStep
                        }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
