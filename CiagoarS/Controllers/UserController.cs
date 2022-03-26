using Ciagoar.Core.Helper;
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
                switch ((AuthenticationType)parameters.authenticationType)
                {
                    case AuthenticationType.EM: response = SetUserLogin_Email(parameters.email, parameters.authenticationKey); break;
                    case AuthenticationType.GG: response = SetUserLogin_Google(parameters.email, parameters.authenticationKey); break;
                    default: break;
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
                Password = CryptographyHelper.GetHash(Password);

                Ci_User userInfo = (from UInfo in _context.UserInfos.Where(p => p.Email.Equals(Email) && p.IsUse && !p.IsDelete)
                                    join UAuthentications in _context.UserAuthentications.Where(p => p.AuthenticationKey.Equals(Password) && p.AuthenticationType == (int)AuthenticationType.EM && p.IsUse && !p.IsDelete)
                                    on UInfo.Id equals UAuthentications.UserInfoId
                                    select new Ci_User()
                                    {
                                        AuthType = UInfo.AuthType,
                                        Email = UInfo.Email,
                                        Nickname = UInfo.Nickname
                                    }).FirstOrDefault();

                if (userInfo != null)
                {
                    return new BaseResponse<Ci_User>
                    {
                        Result = true,
                        Data = userInfo
                    };
                }
                else
                {
                    return ProcessError<Ci_User>(ErrorCode.RE_NEXIST_USER);
                }

            }
            catch (SqlException SExp)
            {
                return DataBaseError<Ci_User>(SExp.Message);
            }
            catch (Exception Exp)
            {
                return ExceptionError<Ci_User>(Exp.Message);
            }
        }

        private BaseResponse<Ci_User> SetUserLogin_Google(string Email, string RefreshTokken)
        {
            try
            {
                Tuple<Ci_User, string> userInfo = (from UInfo in _context.UserInfos.Where(p => p.Email.Equals(Email) && p.IsUse && !p.IsDelete)
                                                   join UAuthentications in _context.UserAuthentications.Where(p => p.AuthenticationType == (int)AuthenticationType.GG && p.IsUse && !p.IsDelete)
                                                   on UInfo.Id equals UAuthentications.UserInfoId
                                                   select new Tuple<Ci_User, string>(new Ci_User()
                                                   {
                                                       AuthType = UInfo.AuthType,
                                                       Email = UInfo.Email,
                                                       Nickname = UInfo.Nickname
                                                   }, UAuthentications.AuthenticationKey)
                                                   ).FirstOrDefault();

                if (userInfo != default)
                {
                    string AuthRefrashTokken = userInfo.Item2;
                    if (string.IsNullOrWhiteSpace(RefreshTokken) && !userInfo.Item2.Equals(RefreshTokken))
                    {
                        UserAuthentication userAuthentication = _context.UserAuthentications.Where(p => p.AuthenticationType == (int)AuthenticationType.GG && p.IsUse && !p.IsDelete).First();
                        userAuthentication.AuthenticationKey = RefreshTokken;
                        _context.SaveChanges();

                        AuthRefrashTokken = RefreshTokken;
                    }

                    if (GetAccessKey(AuthRefrashTokken))
                    {
                        //엑세스 토큰 성공 호출 로그인 진행 
                        return new BaseResponse<Ci_User>
                        {
                            Result = true,
                            Data = userInfo.Item1
                        };
                    }
                    else
                    {
                        // 리프레쉬 토큰 만료 재 로그인 필요
                        return ProcessError<Ci_User>(ErrorCode.RE_OAUTH_REFRASH_TOKKEN_EXPIRED);
                    }
                }
                else
                {
                    // 구글 로그인 사용자 없음 
                    if (_context.UserInfos.Any(p => p.Email.Equals(Email) && p.IsUse && !p.IsDelete))
                    {
                        //해당 계정 존재 ( 연결 확인 필요 ) 
                        return ProcessError<Ci_User>(ErrorCode.RE_EXIST_USER_NEXIST_OAUTH);
                    }
                    else
                    {
                        //해당 계정 자체가 존재 하지 않음
                        return ProcessError<Ci_User>(ErrorCode.RE_NEXIST_USER);
                    }                    
                }
            }
            catch (SqlException SExp)
            {
                return DataBaseError<Ci_User>(SExp.Message);
            }
            catch (Exception Exp)
            {
                return ExceptionError<Ci_User>(Exp.Message);
            }
        }

        private bool GetAccessKey(string RefreshTokken)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                _mLogger.LogError($"[{ModuleName}]  Detail- {ex.Message}{Environment.NewLine}");
                return false;
            }
        }

    }
}
