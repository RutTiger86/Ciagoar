using Ciagoar.Data.Enums;
using CiagoarM.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiagoarM.Models
{
    public class UserModel : BaseModel
    {
        public bool Login(AuthenticationType authentication, string AuthenticationKey, string Email = null)
        {
            try
            {
                switch (authentication)
                {
                    case AuthenticationType.EM: return TryEmailLogin(Email, AuthenticationKey); 
                    case AuthenticationType.GG: return TryGoogleLogin(AuthenticationKey);
                    default: return false;
                }
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }

            return false;
        }

        private bool TryEmailLogin(string Email, string Password )
        {

            try
            {
                

            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
            return false;
        }

        private bool TryGoogleLogin(string RefrashTokken)
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
            return false;
        }
    }
}
