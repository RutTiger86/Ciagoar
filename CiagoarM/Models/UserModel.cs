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
using System.IO;
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

        private async Task<bool> TryGoogleLogin(string Email)
        {
            try
            {
                if(String.IsNullOrEmpty(Email))
                {
                    BaseResponse<Ci_OAuth> OAuthKey = await GetOAuthInfo(AuthenticationType.GG);
                    if(OAuthKey.Result)
                    { 
                        BaseResponse<GoogleUserInfo> response = await Google.TryLogin(OAuthKey.Data);

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
                        LogError($"{OAuthKey.ErrorCode} : {OAuthKey.ErrorMessage}");
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

        private async Task<BaseResponse<Ci_OAuth>> GetOAuthInfo(AuthenticationType authenticationType)
        {
            BaseResponse<Ci_OAuth> result = new BaseResponse<Ci_OAuth>();
            try
            {
                Dictionary<string, string> pQueryParm = new Dictionary<string, string>()
                {
                    ["langCode"] = Properties.Settings.Default.LangCode,
                    ["authenticationType"] = ((int)authenticationType).ToString()
                };

                string URL = Properties.Settings.Default.ServerBaseAddress + "User/oAuthInfo";

                result = await HttpHelper.SendRequest<Ci_OAuth>(URL, null, HttpMethod.Get, MediaType.APPLICATION_JSON, MediaType.APPLICATION_JSON, pQueryParm);
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }

            return result;
        }
    }
}
