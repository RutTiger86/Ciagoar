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
    public class UsersModel : BaseModel
    {


        public async Task<bool> Login(AuthType authentication, string Email, string AuthenticationKey = null)
        {
            try
            {
                switch (authentication)
                {
                    case AuthType.EM: return await TryLogin(authentication, Email, AuthenticationKey);
                    case AuthType.GG: return await TryGoogleLogin(Email);
                    default: return false;
                }
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }

            return false;
        }

        private async Task<bool> TryLogin(AuthType AuthType, string Email, string Password = null)
        {

            try
            {
                // Builds the Token request
                REQ_USER_LOGIN _USER_LOGIN = new()
                {
                    langCode = Properties.Settings.Default.LangCode,
                    authType = (int)AuthType,
                    authKey = Password,
                    email = Email
                };

                string URL = Properties.Settings.Default.ServerBaseAddress + "Users/userLogin";
                string Stringcontent = JsonSerializer.Serialize(_USER_LOGIN);

                BaseResponse<Ci_User> response = await HttpHelper.SendRequest<Ci_User>(URL, Stringcontent, HttpMethod.Post);

                if (response.Result)
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
                if (String.IsNullOrEmpty(Email))
                {
                    BaseResponse<GoogleUserInfo> googleInfo = await GetGoogleOAuth();// google 정보 가져오기 

                    if (!googleInfo.Result)
                    {
                        LogError($"{googleInfo.ErrorCode} : {googleInfo.ErrorMessage}");
                        return false;
                    }

                    if (!await TryLogin(AuthType.GG, googleInfo.Data.email))// 로그인시도 있는지 
                    {
                        return await ConnectGoogle(googleInfo.Data);// 연결시도 
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return await TryLogin(AuthType.GG, Email);
                }

            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
            return false;
        }

        private async Task<BaseResponse<GoogleUserInfo>> GetGoogleOAuth()
        {
            try
            {
                BaseResponse<GoogleUserInfo> response = new();
                BaseResponse<Ci_User> JoinResult = new();

                BaseResponse<Ci_OAuth> OAuthKey = await GetOAuthInfo(AuthType.GG);
                if (!OAuthKey.Result)
                {
                    LogError($"{OAuthKey.ErrorCode} : {OAuthKey.ErrorMessage}");
                }
                return await Google.TryLogin(OAuthKey.Data);
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
            return new BaseResponse<GoogleUserInfo>() {Result = false};
        }

        private async Task<bool> ConnectGoogle(GoogleUserInfo UserInfo)
        {
            try
            {
                BaseResponse<Ci_User> JoinResult = await JoinUser(AuthType.GG, UserInfo.email, UserInfo.refresh_token, UserInfo.name);

                if (JoinResult.Result)
                {
                    Localproperties.LoginUser = JoinResult.Data;
                    return true;
                }
                else
                {
                    LogError($"{JoinResult.ErrorCode} : {JoinResult.ErrorMessage}");
                    return false;
                }

            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
            return false;
        }

        public async Task<BaseResponse<Ci_User>> JoinUser(AuthType authentication, string Email, string AuthenticationKey, string nickname)
        {
            BaseResponse<Ci_User> JoinResult = new();
            try
            {
                // Builds the Token request
                REQ_USER_JOIN _USER_JOIN = new()
                {
                    langCode = Properties.Settings.Default.LangCode,
                    authType = (short)authentication,
                    email = Email,
                    authKey = AuthenticationKey,
                    nickname = nickname
                };

                string URL = Properties.Settings.Default.ServerBaseAddress + "Users/SetJoinUser";
                string Stringcontent = JsonSerializer.Serialize(_USER_JOIN);

                JoinResult = await HttpHelper.SendRequest<Ci_User>(URL, Stringcontent, HttpMethod.Post);

            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }

            return JoinResult;
        }

        private async Task<BaseResponse<Ci_OAuth>> GetOAuthInfo(AuthType authType)
        {
            BaseResponse<Ci_OAuth> result = new BaseResponse<Ci_OAuth>();
            try
            {
                Dictionary<string, string> pQueryParm = new()
                {
                    ["langCode"] = Properties.Settings.Default.LangCode,
                    ["authType"] = ((int)authType).ToString()
                };

                string URL = Properties.Settings.Default.ServerBaseAddress + "Users/oAuthInfo";

                result = await HttpHelper.SendRequest<Ci_OAuth>(URL, null, HttpMethod.Get, pQueryParm);
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }

            return result;
        }

        public async Task<BaseResponse<bool>> AuthenticationStepCheck(string Email, string AuthenticationKey)
        {
            BaseResponse<bool> JoinResult = new();
            try
            {
                // Builds the Token request
                REQ_AUTH_STEP _USER_JOIN = new()
                {
                    langCode = Properties.Settings.Default.LangCode,
                    email = Email,
                    authStepKey = AuthenticationKey
                };

                string URL = Properties.Settings.Default.ServerBaseAddress + "Users/AuthenticationStepCheck";
                string Stringcontent = JsonSerializer.Serialize(_USER_JOIN);

                JoinResult = await HttpHelper.SendRequest<bool>(URL, Stringcontent, HttpMethod.Put);

            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }

            return JoinResult;
        }
    }
}
