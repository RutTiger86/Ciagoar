using Ciagoar.Data.Request.User;
using Ciagoar.Data.Response;
using CiagoarS.Common;
using CiagoarS.DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

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
        /// </response>
        [Route("userJoin")]
        [HttpPost]
        [Produces("application/json")]
        public BaseResponse<bool> SetUserJoin(REQ_USER_JOIN parameters)
        {
            LogingREQ(parameters);

            BaseResponse<bool> response = new BaseResponse<bool>();

            try
            {
            }
            catch (Exception Exp)
            {
                response = ExceptionError<bool>(Exp.Message);
            }

            LogingRES(response);

            return response;
        }
    }
}
