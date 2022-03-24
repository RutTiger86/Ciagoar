using Ciagoar.Core.OAuth;
using Ciagoar.Data.Enums;
using Ciagoar.Data.HTTPS;
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

                BaseResponse<Ci_User> response =  await CallPostCiagoarAPI<Ci_User>("User/userLogin", _USER_LOGIN);

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


        private async Task<BaseResponse<T>> CallPostCiagoarAPI<T>(string RequestURI ,object Content)
        {
            BaseResponse<T> baseResponse = null;

            try
            {
                string URL = Properties.Settings.Default.ServerBaseAddress + RequestURI;
                string Stringcontent = JsonSerializer.Serialize(Content);
                
                StringContent content = new(Stringcontent, Encoding.Default, "application/json");

                HttpClient client = new();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                HttpResponseMessage response = await client.PostAsync(URL, content).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    baseResponse = await response.Content.ReadFromJsonAsync<BaseResponse<T>>();
                }
                else
                {                   
                    var customerJsonString = response.Content.ReadAsStringAsync().Result;

                    Error data = JsonSerializer.Deserialize<Error>(customerJsonString);

                    baseResponse = new BaseResponse<T>()
                    {
                        ErrorCode = data.status.ToString(),
                        ErrorMessage = data.errors.ToString(),
                        Result = false
                    };
                }
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }

            return baseResponse;
        }


        private async Task<bool> TryGoogleLogin(string RefrashTokken)
        {
            try
            {
                if(String.IsNullOrEmpty(RefrashTokken))
                {
                   string Result = await Google.TryLogin();

                   if(!String.IsNullOrEmpty(Result))
                    {
                        Console.WriteLine(Result);
                        return true;
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
