﻿using Ciagoar.Core.Helper;
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
        

        public async Task<bool> Login(AuthenticationType authentication, string Email, string AuthenticationKey = null)
        {
            try
            {
                switch (authentication)
                {
                    case AuthenticationType.EM: return await TryLogin(authentication, Email, AuthenticationKey); 
                    case AuthenticationType.GG: return await TryGoogleLogin(Email);
                    default: return false;
                }
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }

            return false;
        }

        private async Task<bool> TryLogin(AuthenticationType AuthType, string Email, string Password = null )
        {

            try
            {
                // Builds the Token request
                REQ_USER_LOGIN _USER_LOGIN = new ()
                {
                    langCode = Properties.Settings.Default.LangCode,
                    authenticationType = (int)AuthType,
                    authenticationKey = Password,
                    email=Email
                };

                string URL = Properties.Settings.Default.ServerBaseAddress + "User/userLogin";
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
                    return await ConnectGoogle();
                }
                else
                {
                    return await TryLogin(AuthenticationType.GG, Email);
                }

            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
            return false;
        }

        private async Task<bool> ConnectGoogle()
        {
            try
            {
                BaseResponse<Ci_OAuth> OAuthKey = await GetOAuthInfo(AuthenticationType.GG);

                if (OAuthKey.Result)
                {
                    BaseResponse<GoogleUserInfo> response = await Google.TryLogin(OAuthKey.Data);

                    if (response.Result)
                    {

                        return await OAuthSet(AuthenticationType.GG, response.Data.email, response.Data.refresh_token, response.Data.name);
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
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
            return false;
        }

        private async Task<bool> OAuthSet(AuthenticationType authentication ,string Email, string refresh_token, string nickname)
        {
            try
            {
                // Builds the Token request
                REQ_USER_JOIN _USER_JOIN = new()
                {
                    langCode = Properties.Settings.Default.LangCode,
                    authenticationType = (short)authentication,
                    email = Email,
                    authenticationKey = refresh_token,
                    nickname = nickname
                };

                string URL = Properties.Settings.Default.ServerBaseAddress + "User/SetJoinUser";
                string Stringcontent = JsonSerializer.Serialize(_USER_JOIN);

                BaseResponse<Ci_User> JoinResult = await HttpHelper.SendRequest<Ci_User>(URL, Stringcontent, HttpMethod.Post, MediaType.APPLICATION_JSON, MediaType.APPLICATION_JSON, null);

                if (JoinResult.Result)
                {

                    Localproperties.LoginUser = JoinResult.Data;
                    //가입성공 진입
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
                return false;
            }
        }

        
        private async Task<BaseResponse<Ci_OAuth>> GetOAuthInfo(AuthenticationType authenticationType)
        {
            BaseResponse<Ci_OAuth> result = new BaseResponse<Ci_OAuth>();
            try
            {
                Dictionary<string, string> pQueryParm = new()
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
