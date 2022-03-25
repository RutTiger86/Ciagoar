using Ciagoar.Core.Helper;
using Ciagoar.Core.OAuth;
using Ciagoar.Data.Enums;
using Ciagoar.Data.HTTPS;
using Ciagoar.Data.OAuth;
using Ciagoar.Data.Request.Users;
using Ciagoar.Data.Response;
using Ciagoar.Data.Response.Users;
using CiagoarM.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CiagoarM.Models
{
    public class UserModel : BaseModel
    {
        

        public async Task<bool> Login(AuthenticationType authentication, string AuthenticationKey, string Email = null)
        {
            try
            {
                switch (authentication)
                {
                    case AuthenticationType.EM: return await TryEmailLogin(Email, AuthenticationKey); 
                    case AuthenticationType.GG: return await TryGoogleLogin(AuthenticationKey);
                    default: return false;
                }
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }

            return false;
        }

        private async Task<bool> TryEmailLogin(string Email, string Password )
        {

            try
            {
                // Builds the Token request
                REQ_USER_LOGIN _USER_LOGIN = new ()
                {
                    langCode = Properties.Settings.Default.LangCode,
                    authenticationType = (int)AuthenticationType.EM,
                    authenticationKey = Password,
                    email=Email
                };

                string URL = Properties.Settings.Default.ServerBaseAddress + "User/Login";
                string Stringcontent = JsonSerializer.Serialize(_USER_LOGIN);

                BaseResponse<Ci_User> response = await HttpHelper.SendRequest<Ci_User>(URL, Stringcontent, HttpMethod.Post, MediaType.APPLICATION_JSON, MediaType.APPLICATION_JSON, null);

                if(response.Result)
                {
                    Localproperties.LoginUser = response.Data;
                    return true;
                }
                else
                {
                    LogError($"{response.ErrorCode} : {response.ErrorMessage}");
                    return false;
                }

            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
            return false;
        }

        private async Task<bool> TryGoogleLogin(string RefrashTokken)
        {
            try
            {
                if(String.IsNullOrEmpty(RefrashTokken))
                {
                    BaseResponse<GoogleUserInfo> response = await Google.TryLogin();

                    if (response.Result)
                    {
                        return true;
                    }
                    else
                    {
                        LogError($"{response.ErrorCode} : {response.ErrorMessage}");
                        return false;
                    }

                }
                else
                {
                    
                }

            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
            return false;
        }
    }
}
