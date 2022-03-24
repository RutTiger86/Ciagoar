using Ciagoar.Data;
using Ciagoar.Data.Response;
using CiagoarS.CodeMessage;
using CiagoarS.Common.Enums;
using CiagoarS.DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace CiagoarS.Common
{
    public class BaseController : ControllerBase
    {
        protected ILogger<BaseController> _mLogger;
        protected CiagoarContext _context;

        protected string ModuleName
        {
            get => ControllerContext.ActionDescriptor.AttributeRouteInfo.Template;
        }

        private string Serialize(object parameters)
        {
            return JsonSerializer.Serialize(parameters,Localproperties.JsonOption);
        }

        protected void LogingREQ(object parameters, [CallerMemberName] string propertyName = "")
        {          
            _mLogger.LogInformation($"[{ModuleName}]  ==========  START {propertyName}  ==========");
            _mLogger.LogInformation($"[{ModuleName}]  RECEIVE REQUEST DATA  [{Serialize(parameters)}]{Environment.NewLine}");
        }

        protected void LogingRES(object response)
        {

            _mLogger.LogInformation($"[{ModuleName}]  RESPONSE DATA  [{Serialize(response)}]{Environment.NewLine}");
        }

        protected BaseResponse<T> ExceptionError<T>(string sDetail)
        {            
            BaseResponse<T> response = new()
            {
                Result = false,
                ErrorCode = nameof(Resource.EC_EX_001),
                ErrorMessage = Resource.EC_EX_001,
                Data = default(T)
            };

            _mLogger.LogError($"[{ModuleName}]  RESPONSE DATA  [{Serialize(response)}]{Environment.NewLine} Detail- {sDetail}{Environment.NewLine}");
            return response;
        }


        protected BaseResponse<T> DataBaseError<T>(string sDetail)
        {
            BaseResponse<T> response = new()
            {
                Result = false,
                ErrorCode = nameof(Resource.EC_DB_001),
                ErrorMessage = Resource.EC_DB_001,
                Data = default(T)
            };

            _mLogger.LogError($"[{ModuleName}]  RESPONSE DATA  [{Serialize(response)}]{Environment.NewLine} Detail- {sDetail}{Environment.NewLine}");
            return response;
        }

        protected BaseResponse<T> ProcessError<T>(ErrorCode ECode, string sDetail = null)
        {
            BaseResponse<T> response = new()
            {
                Result = false,
                ErrorCode = ECode.ToString(),
                ErrorMessage = Resource.ResourceManager.GetString(ECode.ToString()),
                Data = default(T)
            };

            _mLogger.LogInformation($"[{ModuleName}]  RESPONSE DATA  [{Serialize(response)}]{Environment.NewLine}");

            if (sDetail != null)
            {
                _mLogger.LogInformation($"[{ModuleName}] Detail- {sDetail}{Environment.NewLine}");
            }
            return response;
        }

    }
}
