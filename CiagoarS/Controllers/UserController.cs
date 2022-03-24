using Ciagoar.Core.OAuth.Common;
using Ciagoar.Data.Enums;
using Ciagoar.Data.Request.Users;
using Ciagoar.Data.Response;
using Ciagoar.Data.Response.Users;
using CiagoarS.CodeMessage;
using CiagoarS.Common;
using CiagoarS.Common.Enums;
using CiagoarS.DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;

namespace CiagoarS.Controllers
{
    /// <summary>
    /// 사용자 관련 API
    /// </summary>
    [Route("User")]
    [ApiController]
    public class UserController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        public UserController(ILogger<UserController> logger, CiagoarContext context)
        {
            _mLogger = logger;
            _context = context;
        }

        /// <summary>
        /// 사용자 가입
        /// </summary>
        /// <remarks>
        /// 신규 사용자 가입
        /// </remarks>
        /// <param name="parameters">
        /// <para>요청형식 설명</para>
        /// <para>- langCode                    : 언어코드 (en-US:영어, ko-KR:한국어)</para>
        /// <para>- authenticationType          : 인증방법 (EM:Emain인증, GG: Google)</para>
        /// <para>- email                       : 이메일주소</para>
        /// <para>- authenticationKey           : 가입 Key(EM 일경우 패스워드/ 3rd Party일경우 Tokken) </para>
        /// </param> 
        /// <returns>리턴값 설명</returns>
        /// <response code="200">
        /// <para>응답형식 설명</para>
        /// <para>Result - true/false</para>
        /// <para>ErrorCode - 실패시 오류코드</para>
        /// <para>ErrorMessage - 실패시 오류메세지</para>
        /// <para>Data - 성공시 결과</para>
        /// <para>     --Email       사용자 Email</para>
        /// <para>     --Nickname    사용자 별칭</para>
        /// <para>     --AuthType    계정 권한</para>
        /// </response>
        [Route("userLogin")]
        [HttpPost]
        [Produces("application/json")]
        public BaseResponse<Ci_User> SetUserLogin(REQ_USER_LOGIN parameters)
        {
            LogingREQ(parameters);

            BaseResponse<Ci_User> response = new BaseResponse<Ci_User>();

            try
            {
                switch((AuthenticationType)parameters.authenticationType)
                {
                    case AuthenticationType.EM: response = SetUserLogin_Email(parameters.email, parameters.authenticationKey); break;
                    case AuthenticationType.GG:break;
                        default:break;
                }

            }
            catch (Exception Exp)
            {
                response = ExceptionError<Ci_User>(Exp.Message);
            }

            LogingRES(response);

            return response;
        }

        private BaseResponse<Ci_User> SetUserLogin_Email(string Email, string Password)
        {
            try
            {
                Password= GetHash(Password);

                UserInfo userInfo =  _context.UserInfos.FirstOrDefault(p => p.Email.Equals(Email) && p.AuthenticationKey.Equals(Password));

                if(userInfo != null)
                {
                    return new BaseResponse<Ci_User> { Result = true, Data = new Ci_User()
                    {
                        AuthType = userInfo.AuthType,
                        Email = userInfo.Email,
                        Nickname = userInfo.Nickname
                    }
                    };
                }
                else
                {
                    return ProcessError<Ci_User>(ErrorCode.RE_NEXIST_USER_001);
                }

            }
            catch(SqlException SExp)
            {
                return DataBaseError<Ci_User>(SExp.Message);
            }
            catch (Exception Exp)
            {
                return ExceptionError<Ci_User>(Exp.Message);
            }

        }

        private static string GetHash(string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = OAuthCommon.sha256(input); ;

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}
