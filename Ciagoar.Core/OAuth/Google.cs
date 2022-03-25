using Ciagoar.Core.Helper;
using Ciagoar.Data.HTTPS;
using Ciagoar.Data.OAuth;
using Ciagoar.Data.Response;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace Ciagoar.Core.OAuth
{
    public class Google : BaseOAuth
    {
        // client configuration
        const string clientID = "";
        const string clientSecret = "";
        const string authorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
        const string tokenEndpoint = "https://www.googleapis.com/oauth2/v4/token";
        const string userInfoEndpoint = "https://www.googleapis.com/oauth2/v3/userinfo";
        const string SCOPE = "https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile";

        public static async Task<BaseResponse<GoogleUserInfo>> TryLogin()
        {
            // Generates state and PKCE values.
            string state = CryptographyHelper.randomDataBase64url(32);
            string code_verifier = CryptographyHelper.randomDataBase64url(32);
            string code_challenge = CryptographyHelper.base64urlencodeNoPadding(CryptographyHelper.sha256(code_verifier));
            const string code_challenge_method = "S256";

            // Creates a redirect URI using an available port on the loopback address.
            string redirectURI = string.Format("http://{0}:{1}/", IPAddress.Loopback, GetRandomUnusedPort());

            // Creates an HttpListener to listen for requests on that redirect URI.           
            var http = new HttpListener();
            http.Prefixes.Add(redirectURI);
            http.Start();

            // Creates the OAuth 2.0 authorization request.
            string authorizationRequest = string.Format("{0}?response_type=code&scope=openid%20profile&redirect_uri={1}&client_id={2}&state={3}&code_challenge={4}&code_challenge_method={5}",
                authorizationEndpoint,
                System.Uri.EscapeDataString(redirectURI),
                clientID,
                state,
                code_challenge,
                code_challenge_method);

            // Opens request in the browser.
            Process.Start(new ProcessStartInfo { FileName = authorizationRequest, UseShellExecute = true });

            // Waits for the OAuth authorization response.
            var context = await http.GetContextAsync();


            // Sends an HTTP response to the browser.
            var response = context.Response;
            string responseString = string.Format("<html><head><meta http-equiv='refresh' content='10;url=https://google.com'></head><body>Please return to the app.</body></html>");
            var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            var responseOutput = response.OutputStream;
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

            // extracts the code
            var code = context.Request.QueryString.Get("code");
            var incoming_state = context.Request.QueryString.Get("state");

            // Compares the receieved state to the expected value, to ensure that
            // this app made the request which resulted in authorization.
            if (incoming_state != state)
            {
                return new BaseResponse<GoogleUserInfo>() { Result = false, ErrorCode = "invalid state", ErrorMessage = incoming_state };
            }

            // Starts the code exchange at the Token Endpoint.
            return await performCodeExchange(code, code_verifier, redirectURI);
        }


        async static Task<BaseResponse<GoogleUserInfo>> performCodeExchange(string code, string code_verifier, string redirectURI)
        {
            // Builds the Token request
            string tokenRequestBody = string.Format("code={0}&redirect_uri={1}&client_id={2}&code_verifier={3}&client_secret={4}&scope={5}&grant_type=authorization_code",
                code,
                System.Uri.EscapeDataString(redirectURI),
                clientID,
                code_verifier,
                clientSecret,
                SCOPE
                );

            StringContent content = new StringContent(tokenRequestBody, Encoding.UTF8, "application/x-www-form-urlencoded");

            // Performs the authorization code exchange.
            HttpClientHandler handler = new HttpClientHandler();
            handler.AllowAutoRedirect = true;
            HttpClient client = new HttpClient(handler);

            HttpResponseMessage response = await client.PostAsync(tokenEndpoint, content);
            string responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new BaseResponse<GoogleUserInfo>() { Result = false, ErrorCode = response.StatusCode.ToString(), ErrorMessage = responseString };
            }
            else
            {

                // Sets the Authentication header of our HTTP client using the acquired access token.
                GoogleOAuth tokenEndpointDecoded = JsonSerializer.Deserialize<GoogleOAuth>(responseString);

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(tokenEndpointDecoded.token_type, tokenEndpointDecoded.access_token);

                // Makes a call to the Userinfo endpoint, and prints the results.
                HttpResponseMessage userinfoResponse = client.GetAsync(userInfoEndpoint).Result;
                string userinfoResponseContent = await userinfoResponse.Content.ReadAsStringAsync();

                BaseResponse<GoogleUserInfo> baseResponse = new BaseResponse<GoogleUserInfo>()
                {
                    Result = true,
                    Data = JsonSerializer.Deserialize<GoogleUserInfo>(userinfoResponseContent)
                };

                return baseResponse;
            }
        }
    }

}
