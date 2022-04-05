using Ciagoar.Core.Helper;
using Ciagoar.Data.HTTPS;
using Ciagoar.Data.OAuth;
using Ciagoar.Data.Response;
using Ciagoar.Data.Response.Users;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace Ciagoar.Core.OAuth
{
    public class Google : BaseOAuth
    {
        const string userInfoEndpoint = "https://www.googleapis.com/oauth2/v3/userinfo";
        const string SCOPE = "openid https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile";

        public static async Task<BaseResponse<GoogleUserInfo>> TryLogin(Ci_OAuth GoogleO)
        {
            try
            {
                // Generates state and PKCE values.
                string state = CryptographyHelper.randomDataBase64url(32);
                string code_verifier = CryptographyHelper.randomDataBase64url(32);
                string code_challenge = CryptographyHelper.base64urlencodeNoPadding(CryptographyHelper.sha256(code_verifier));
                const string code_challenge_method = "S256";

                // Creates a redirect URI using an available port on the loopback address.
                string redirectURI = $"http://{IPAddress.Loopback}:{GetRandomUnusedPort()}/";

                // Creates an HttpListener to listen for requests on that redirect URI.           
                HttpListener http = new();
                http.Prefixes.Add(redirectURI);
                http.Start();

                // Creates the OAuth 2.0 authorization request.
                string authorizationRequest = $"{GoogleO.AuthUri}?response_type=code&scope={SCOPE}&redirect_uri={Uri.EscapeDataString(redirectURI)}&client_id={GoogleO.ClientId}&state={state}&code_challenge={code_challenge}&code_challenge_method={code_challenge_method}";

                // Opens request in the browser.
                Process.Start(new ProcessStartInfo { FileName = authorizationRequest, UseShellExecute = true });

                // Waits for the OAuth authorization response.
                HttpListenerContext context = await http.GetContextAsync();


                // Sends an HTTP response to the browser.
                HttpListenerResponse response = context.Response;
                string responseString = "<html><head><meta http-equiv='refresh' content='10;url=https://google.com'></head><body>Please return to the app.</body></html>";
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                Stream responseOutput = response.OutputStream;
                Task responseTask = responseOutput.WriteAsync(buffer, 0, buffer.Length).ContinueWith((task) =>
                {
                    responseOutput.Close();
                    http.Stop();
                });

                // Checks for errors.
                if (context.Request.QueryString.Get("error") != null)
                {
                    return new BaseResponse<GoogleUserInfo>() { Result = false, ErrorCode = "authorization", ErrorMessage = context.Request.QueryString.Get("error") };
                }
                if (context.Request.QueryString.Get("code") == null || context.Request.QueryString.Get("state") == null)
                {
                    return new BaseResponse<GoogleUserInfo>() { Result = false, ErrorCode = "authorization", ErrorMessage = context.Request.QueryString.ToString() };
                }

                return await InvalidCode_GetUSer(context, state, GoogleO, redirectURI, code_verifier);
            }
            catch (Exception)
            {

                throw;
            }
        }

        async static Task<BaseResponse<GoogleUserInfo>> InvalidCode_GetUSer(HttpListenerContext context, string state, Ci_OAuth GoogleO, string redirectURI, string code_verifier)
        {
            try
            {
                // extracts the code
                string code = context.Request.QueryString.Get("code");
                string incoming_state = context.Request.QueryString.Get("state");

                // Compares the receieved state to the expected value, to ensure that
                // this app made the request which resulted in authorization.
                if (incoming_state != state)
                {
                    return new BaseResponse<GoogleUserInfo>() { Result = false, ErrorCode = "invalid state", ErrorMessage = incoming_state };
                }

                // Starts the code exchange at the Token Endpoint.
                BaseResponse<GoogleOAuth> CodeExchange = await performCodeExchange(GoogleO, code, code_verifier, redirectURI);


                if (CodeExchange.Result)
                {
                    BaseResponse<GoogleUserInfo> userInfo = await GetUserInfo(CodeExchange.Data.access_token, CodeExchange.Data.token_type);

                    if (userInfo.Result)
                    {
                        userInfo.Data.refresh_token = CodeExchange.Data.refresh_token;
                    }

                    return userInfo;
                }
                else
                {
                    return new BaseResponse<GoogleUserInfo>()
                    {
                        Result = false,
                        ErrorCode = CodeExchange.ErrorCode,
                        ErrorMessage = CodeExchange.ErrorMessage,
                        Data = default
                    };
                }
            }
            catch (Exception )
            {
                throw;
            }
        }

        async static Task<BaseResponse<GoogleOAuth>> performCodeExchange(Ci_OAuth GoogleO, string code, string code_verifier, string redirectURI)
        {
            try
            {
                // Builds the Token request
                string tokenRequestBody = $"code={code}&redirect_uri={System.Uri.EscapeDataString(redirectURI)}&client_id={GoogleO.ClientId}&code_verifier={code_verifier}&client_secret={GoogleO.ClientSecret}&scope={SCOPE}&grant_type=authorization_code";

                StringContent content = new(tokenRequestBody, Encoding.UTF8, "application/x-www-form-urlencoded");

                // Performs the authorization code exchange.
                HttpClientHandler handler = new();
                handler.AllowAutoRedirect = true;
                HttpClient client = new(handler);

                HttpResponseMessage response = await client.PostAsync(GoogleO.TokenUri, content);
                string responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new BaseResponse<GoogleOAuth>() { Result = false, ErrorCode = response.StatusCode.ToString(), ErrorMessage = responseString };
                }
                else
                {
                    // Sets the Authentication header of our HTTP client using the acquired access token.              
                    BaseResponse<GoogleOAuth> baseResponse = new()
                    {
                        Result = true,
                        Data = JsonSerializer.Deserialize<GoogleOAuth>(responseString)
                    };

                    return baseResponse;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        async static Task<BaseResponse<GoogleUserInfo>> GetUserInfo(string access_token, string token_type = "Bearer")
        {
            try
            {
                // Performs the authorization code exchange.
                HttpClientHandler handler = new();
                handler.AllowAutoRedirect = true;
                HttpClient client = new(handler);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token_type, access_token);

                // Makes a call to the Userinfo endpoint, and prints the results.
                HttpResponseMessage response = client.GetAsync(userInfoEndpoint).Result;
                string responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new BaseResponse<GoogleUserInfo>() { Result = false, ErrorCode = response.StatusCode.ToString(), ErrorMessage = responseString };
                }
                else
                {
                    // Sets the Authentication header of our HTTP client using the acquired access token.              
                    BaseResponse<GoogleUserInfo> baseResponse = new()
                    {
                        Result = true,
                        Data = JsonSerializer.Deserialize<GoogleUserInfo>(responseString)
                    };
                    return baseResponse;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async static Task<BaseResponse<GoogleOAuth>> RefrashAccessToken(Ci_OAuth GoogleO, string refresh_token)
        {
            try
            {
                // Builds the Token request
                string tokenRequestBody = $"client_id={GoogleO.ClientId}&client_secret={GoogleO.ClientSecret}&refresh_token={refresh_token}&grant_type=refresh_token";

                StringContent content = new(tokenRequestBody, Encoding.UTF8, "application/x-www-form-urlencoded");

                // Performs the authorization code exchange.
                HttpClientHandler handler = new();
                handler.AllowAutoRedirect = true;
                HttpClient client = new(handler);

                HttpResponseMessage response = await client.PostAsync(GoogleO.TokenUri, content);
                string responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new BaseResponse<GoogleOAuth>() { Result = false, ErrorCode = response.StatusCode.ToString(), ErrorMessage = responseString };
                }
                else
                {
                    // Sets the Authentication header of our HTTP client using the acquired access token.              
                    BaseResponse<GoogleOAuth> baseResponse = new()
                    {
                        Result = true,
                        Data = JsonSerializer.Deserialize<GoogleOAuth>(responseString)
                    };

                    return baseResponse;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

}
