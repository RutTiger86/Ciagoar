using Ciagoar.Data.Response;
using CiagoarS.CodeMessage;
using CiagoarS.DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace CiagoarS.Common
{
    public class BaseController : Controller
    {
        protected ILogger<BaseController> _mLogger;
        protected CiagoarContext _context;

        protected string ModuleName
        {
            get => ControllerContext.ActionDescriptor.AttributeRouteInfo.Template;
        }

        protected void LogingREQ(object parameters, [CallerMemberName] string propertyName = "")
        {          
            _mLogger.LogInformation($"[{ModuleName}]  ==========  START {propertyName}  ==========");
            _mLogger.LogInformation($"[{ModuleName}]  RECEIVE REQUEST DATA  [{JsonSerializer.Serialize(parameters)}]{Environment.NewLine}");
        }

        protected void LogingRES(object response)
        {

            _mLogger.LogInformation($"[{ModuleName}]  RESPONSE DATA  [{JsonSerializer.Serialize(response)}]{Environment.NewLine}");
        }

        protected BaseResponse<T> ExceptionError<T>(string sDetail)
        {            
            BaseResponse<T> response = new()
            {
                Result = false,
                ErrorCode = nameof(Resource.EC_DB_001),
                ErrorMessage = Resource.EC_DB_001,
                Data = default(T)
            };

            _mLogger.LogError($"{ModuleName}  RESPONSE DATA  [{JsonSerializer.Serialize(response)}]{Environment.NewLine} Detail- {sDetail}{Environment.NewLine}");
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

            _mLogger.LogError($"{ModuleName}  RESPONSE DATA  [{JsonSerializer.Serialize(response)}]{Environment.NewLine} Detail- {sDetail}{Environment.NewLine}");
            return response;
        }

        protected BaseResponse<T> ProcessError<T>(string ErrorCode, string sDetail = null)
        {
            BaseResponse<T> response = new()
            {
                Result = false,
                ErrorCode = ErrorCode,
                ErrorMessage = Resource.ResourceManager.GetString(ErrorCode),
                Data = default(T)
            };

            _mLogger.LogError($"{ModuleName}  RESPONSE DATA  [{JsonSerializer.Serialize(response)}]{Environment.NewLine} Detail- {sDetail}{Environment.NewLine}");

            if (sDetail != null)
            {
                _mLogger.LogInformation($"{ModuleName} Detail- {sDetail}{Environment.NewLine}");
            }
            return response;
        }

    }
}
