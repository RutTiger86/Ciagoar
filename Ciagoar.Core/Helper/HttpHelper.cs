using Ciagoar.Data.Enums;
using Ciagoar.Data.HTTPS;
using Ciagoar.Data.Response;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ciagoar.Core.Helper
{
    public class HttpHelper
    {
        public async static Task<BaseResponse<T>> SendRequest<T>(string pUrl, string pBody, HttpMethod pMethod, Dictionary<string, string> pQueryParm = null,MediaType pAccept = MediaType.APPLICATION_JSON, MediaType pContent = MediaType.APPLICATION_JSON)
        {
            BaseResponse<T> response = new BaseResponse<T>();
            try
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = pMethod,
                    RequestUri = pQueryParm == null ? new Uri(pUrl) : new Uri(QueryHelpers.AddQueryString(pUrl, pQueryParm)),
                    Content = string.IsNullOrWhiteSpace(pBody) ? null : new StringContent(pBody, Encoding.UTF8, EnumHelper.GetDescription(pContent)),
                };

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Accept", EnumHelper.GetDescription(pAccept));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", EnumHelper.GetDescription(pContent));

                HttpResponseMessage httpResponse = await client.SendAsync(request).ConfigureAwait(false);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response = await httpResponse.Content.ReadFromJsonAsync<BaseResponse<T>>();
                }
                else
                {
                    string customerJsonString = httpResponse.Content.ReadAsStringAsync().Result;

                    Error data = JsonSerializer.Deserialize<Error>(customerJsonString);

                    response = new BaseResponse<T>()
                    {
                        ErrorCode = data.status.ToString(),
                        ErrorMessage = data.errors.ToString(),
                        Result = false
                    };
                }

            }
            catch (Exception ex)
            {
                response = new BaseResponse<T>()
                {
                    ErrorCode = "Exception",
                    ErrorMessage = ex.Message,
                    Result = false
                };
            }

            return response;
        }
    }
}
