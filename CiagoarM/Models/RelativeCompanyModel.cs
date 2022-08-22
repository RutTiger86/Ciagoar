using Ciagoar.Core.Helper;
using Ciagoar.Core.OAuth;
using Ciagoar.Data.Enums;
using Ciagoar.Data.HTTPS;
using Ciagoar.Data.OAuth;
using Ciagoar.Data.Request.RelativeCompanies;
using Ciagoar.Data.Request.Users;
using Ciagoar.Data.Response;
using Ciagoar.Data.Response.RelativecCompanies;
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
    public class RelativeCompanyModel : BaseModel
    {


        public async Task<List<Ci_RELATVE_CO>> GetRelativeCompany(string SortOrder, string SearchString, int pageCount, int pageIndex)
        {
            try
            {
                Dictionary<string, string> pQueryParm = new()
                {
                    {"langCode",Properties.Settings.Default.LangCode},
                    {"sortOrder",SortOrder},
                    {"searchString",SearchString},
                    {"pageCount",pageCount.ToString()},
                    {"pageIndex",pageIndex.ToString()}
                };


                string URL = Properties.Settings.Default.ServerBaseAddress + "RelativeCompanies";

                BaseResponse<List<Ci_RELATVE_CO>> response = await HttpHelper.SendRequest<List<Ci_RELATVE_CO>>(URL, null, HttpMethod.Get, pQueryParm);

                if (response.Result)
                {

                    return response.Data;
                }
                else
                {
                    LogError($"{response.ErrorCode} : {response.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
            return new List<Ci_RELATVE_CO>();
        }
    }
}
