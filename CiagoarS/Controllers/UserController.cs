using Ciagoar.Core.Helper;
using Ciagoar.Core.OAuth;
using Ciagoar.Data.Enums;
using Ciagoar.Data.HTTPS;
using Ciagoar.Data.OAuth;
using Ciagoar.Data.Request.Users;
using Ciagoar.Data.Response;
using Ciagoar.Data.Response.Users;
using CiagoarS.Common;
using CiagoarS.Common.Enums;
using CiagoarS.DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        /// 사용자 로그인
        /// </summary>
        /// <remarks>
        /// 신규 사용자 가입
        /// </remarks>
        /// <param name="parameters">
        /// <para>요청형식 설명</para>
        /// <para>- langCode                    : 언어코드 (en-US:영어, ko-KR:한국어)</para>
        /// <para>- authenticationType          : 인증방법 </para>
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
        public async Task<BaseResponse<Ci_User>> SetUserLoginAsync(REQ_USER_LOGIN parameters)
        {
            LogingREQ(parameters);

            BaseResponse<Ci_User> response = new BaseResponse<Ci_User>();

            try
            {
                switch ((AuthenticationType)parameters.authenticationType)
                {
                    case AuthenticationType.EM: response = SetUserLogin_Email(parameters.email, parameters.authenticationKey); break;
                    case AuthenticationType.GG: response = await SetUserLogin_Google(parameters.email); break;
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
                                        Nickname = UInfo.Nickname,
                                        AuthenticationStep = UAuthentications.AuthenticationStep
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

        private async Task<BaseResponse<Ci_User>> SetUserLogin_Google(string Email)
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
                                                       Nickname = UInfo.Nickname,
                                                       AuthenticationStep = 0
                                                   }, UAuthentications.AuthenticationKey)
                                                   ).FirstOrDefault();

                if (userInfo != default)
                {
                    string AuthRefrashTokken = userInfo.Item2;

                    Ci_OAuth ci_OAuth = _context.OauthInfos.Where(p => p.AuthenticationType == (int)AuthenticationType.GG).Select(p => new Ci_OAuth()
                    {
                        AuthenticationType = p.AuthenticationType,
                        AuthUri = p.AuthUri,
                        ClientId = p.ClientId,
                        ClientSecret = p.ClientSecret,
                        TokenUri = p.TokenUri
                    }).FirstOrDefault();

                    BaseResponse<GoogleOAuth> response = await Google.RefrashAccessToken(ci_OAuth, AuthRefrashTokken);
                    if (response.Result)
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


        

        /// <summary>
        /// OAuth지원 3rdParty 정보
        /// </summary>
        /// <remarks>
        ///  OAuth지원 3rdParty 정보
        /// </remarks>
        /// <param name="parameters">
        /// <para>요청형식 설명</para>
        /// <para>- langCode                    : 언어코드 (en-US:영어, ko-KR:한국어)</para>
        /// <para>- authenticationType          : 인증방법 </para>
        /// </param> 
        /// <returns>리턴값 설명</returns>
        /// <response code="200">
        /// <para>응답형식 설명</para>
        /// <para>Result - true/false</para>
        /// <para>ErrorCode - 실패시 오류코드</para>
        /// <para>ErrorMessage - 실패시 오류메세지</para>
        /// <para>Data - 성공시 결과</para>
        /// <para>     --AuthenticationType       인증방법</para>
        /// <para>     --ClientId                 OAuth ClientID</para>
        /// <para>     --ClientSecret             OAuth ClientSecret</para>
        /// <para>     --AuthUri                  Auth요청 URI</para>
        /// <para>     --TokenUri                 Token요청 URI</para>
        /// </response>
        [Route("oAuthInfo")]
        [HttpGet]
        [Produces("application/json")]
        public BaseResponse<Ci_OAuth> GetOAuthInfo([FromQuery] REQ_OAUTH_INFO parameters)
        {
            LogingREQ(parameters);

            BaseResponse<Ci_OAuth> response = new BaseResponse<Ci_OAuth>();

            try
            {
                Ci_OAuth ci_OAuth = _context.OauthInfos.Where(p => p.AuthenticationType == parameters.authenticationType).Select(p => new Ci_OAuth()
                {
                    AuthenticationType = p.AuthenticationType,
                    AuthUri = p.AuthUri,
                    ClientId = p.ClientId,
                    ClientSecret = p.ClientSecret,
                    TokenUri = p.TokenUri
                }).FirstOrDefault();

                if (ci_OAuth != null)
                {
                    response = new BaseResponse<Ci_OAuth>() { Result = true, Data = ci_OAuth };
                }
                else
                {
                    response = ProcessError<Ci_OAuth>(ErrorCode.RE_OAUTH_UNSUPPORTED_3RDPARTY_LOGIN);
                }

            }
            catch (Exception Exp)
            {
                response = ExceptionError<Ci_OAuth>(Exp.Message);
            }

            LogingRES(response);

            return response;
        }

        /// <summary>
        /// 사용자 가입 
        /// </summary>
        /// <remarks>
        ///  OAuth지원 3rdParty 정보
        /// </remarks>
        /// <param name="parameters">
        /// <para>요청형식 설명</para>
        /// <para>- langCode                    : 언어코드 (en-US:영어, ko-KR:한국어)</para>
        /// <para>- authenticationType          : 인증방법 </para>
        /// </param> 
        /// <returns>리턴값 설명</returns>
        /// <response code="200">
        /// <para>응답형식 설명</para>
        /// <para>Result - true/false</para>
        /// <para>ErrorCode - 실패시 오류코드</para>
        /// <para>ErrorMessage - 실패시 오류메세지</para>
        /// <para>Data - 성공시 결과</para>
        /// <para>     --AuthenticationType       인증방법</para>
        /// <para>     --ClientId                 OAuth ClientID</para>
        /// <para>     --ClientSecret             OAuth ClientSecret</para>
        /// <para>     --AuthUri                  Auth요청 URI</para>
        /// <para>     --TokenUri                 Token요청 URI</para>
        /// </response>
        [Route("SetJoinUser")]
        [HttpPost]
        [Produces("application/json")]
        public BaseResponse<Ci_User> SetJoinUser(REQ_USER_JOIN parameters)
        {
            LogingREQ(parameters);

            BaseResponse<Ci_User> response = new();

            try
            {
                UserInfo userInfo = _context.UserInfos.Where(p => p.Email.Equals(parameters.email)).FirstOrDefault();

                if (userInfo == null)
                {
                    response = InsertUserInfo(parameters);
                }
                else
                {
                    response = ConnectUserAuthentication(userInfo,parameters);
                }

            }
            catch (Exception Exp)
            {
                response = ExceptionError<Ci_User>(Exp.Message);
            }

            LogingRES(response);

            return response;
        }

        private BaseResponse<Ci_User> InsertUserInfo(REQ_USER_JOIN parameters)
        {
            BaseResponse<Ci_User> response = new();

            var Transaction =  _context.Database.BeginTransaction();
            try
            {
                string Password = CryptographyHelper.GetHash(parameters.authenticationKey);

                //신규가입
                UserInfo userInfo = new ()
                {
                    Email = parameters.email,
                    AuthType = (int)AuthType.User,
                    Nickname = parameters.nickname,
                    IsUse = true,
                    UserAuthentications = new List<UserAuthentication>()
                    {
                        new UserAuthentication()
                        {
                            AuthenticationType = parameters.authenticationType,
                            AuthenticationKey = Password,
                            AuthenticationStep = (short)(parameters.authenticationType == (short)AuthenticationType.EM? 2 : 0),
                            IsUse = true,
                        }
                    }
                };

                _context.UserInfos.Add(userInfo);
                _context.SaveChanges();

                if(parameters.authenticationType == (short)AuthenticationType.EM)
                {
                    SMTP_INFO sMTP_INFO = _context.OauthInfos.Where(p => p.AuthenticationType == (short)AuthenticationType.EM).Select(p => new SMTP_INFO()
                    {
                        nSMTPPort = int.Parse(p.TokenUri),
                        sSMTPPassword = p.AuthUri,
                        sSMTPServer = p.ClientId,
                        sSMTPUser = p.ClientSecret
                    }).FirstOrDefault();

                   


                    if (sMTP_INFO != null && EmailHelper.SendEMail(sMTP_INFO,userInfo,_mLogger))
                    {
                        UserAuthentication authentication =  userInfo.UserAuthentications.Where(p => p.AuthenticationType == (short)AuthenticationType.EM).FirstOrDefault();

                        if (authentication != null)
                        {
                            authentication.AuthenticationStep = 1;
                            _context.SaveChanges();

                            response.Result = true;
                            response.Data = (from UInfo in _context.UserInfos.Where(p => p.Email.Equals(parameters.email) && p.IsUse && !p.IsDelete)
                                             select new Ci_User()
                                             {
                                                 AuthType = UInfo.AuthType,
                                                 Email = UInfo.Email,
                                                 Nickname = UInfo.Nickname,
                                                 AuthenticationStep = authentication.AuthenticationStep,
                                             }).FirstOrDefault();

                            Transaction.Commit();
                        }
                        else
                        {
                            response = ProcessError<Ci_User>(ErrorCode.EC_EX);
                            Transaction.Rollback();
                        }                       
                    }
                    else
                    {
                        response = ProcessError<Ci_User>(ErrorCode.EC_EX);
                        Transaction.Rollback();
                    }
                }
                else
                {
                    response.Result = true;
                    response.Data = (from UInfo in _context.UserInfos.Where(p => p.Email.Equals(parameters.email) && p.IsUse && !p.IsDelete)
                                     select new Ci_User()
                                     {
                                         AuthType = UInfo.AuthType,
                                         Email = UInfo.Email,
                                         Nickname = UInfo.Nickname,
                                         AuthenticationStep = 0
                                     }).FirstOrDefault();
                    Transaction.Commit();
                }

            }
            catch (Exception Exp)
            {
                response = ExceptionError<Ci_User>(Exp.Message);
                Transaction.Rollback();
            }

            return response;
        }

        private BaseResponse<Ci_User> ConnectUserAuthentication(UserInfo userInfo, REQ_USER_JOIN parameters)
        {
            BaseResponse<Ci_User> response = new();

            try
            {
                UserAuthentication userAuthentication = _context.UserAuthentications.FirstOrDefault(p => p.AuthenticationType.Equals(parameters.authenticationType));

                if (userAuthentication != null)
                {
                    //사용자 정보도 있고 로그인 정보도 있음
                    if (parameters.authenticationType != (int)AuthenticationType.EM)
                    {
                                                
                        // RefrashCodeUpdate
                        userAuthentication.AuthenticationKey = parameters.authenticationKey;
                        _context.SaveChanges();

                        response.Result = true;
                        response.Data = (from UInfo in _context.UserInfos.Where(p => p.Email.Equals(parameters.email) && p.IsUse && !p.IsDelete)
                                         select new Ci_User()
                                         {
                                             AuthType = UInfo.AuthType,
                                             Email = UInfo.Email,
                                             Nickname = UInfo.Nickname
                                         }).FirstOrDefault();
                    }
                    else
                    {
                        // Email계정으로 등록 불가 
                        response = ProcessError<Ci_User>(ErrorCode.RE_EXIST_USER);
                    }
                }
                else
                {



                    //사용자 정보는 있으나 로그인 정보는 없음 계정 연결 
                    UserAuthentication authentication = new()
                    {
                        AuthenticationType = parameters.authenticationType,
                        AuthenticationKey = parameters.authenticationType == (int)AuthenticationType.EM ? CryptographyHelper.GetHash(parameters.authenticationKey) : parameters.authenticationKey,
                        UserInfoId = userInfo.Id,
                        IsUse = true,
                    };

                    _context.UserAuthentications.Add(authentication);
                    _context.SaveChanges();


                    if (parameters.authenticationType == (short)AuthenticationType.EM)
                    {
                        SMTP_INFO sMTP_INFO = _context.OauthInfos.Where(p => p.AuthenticationType == (short)AuthenticationType.EM).Select(p => new SMTP_INFO()
                        {
                            nSMTPPort = int.Parse(p.TokenUri),
                            sSMTPPassword = p.AuthUri,
                            sSMTPServer = p.ClientId,
                            sSMTPUser = p.ClientSecret
                        }).FirstOrDefault();

                        if (sMTP_INFO != null && EmailHelper.SendEMail(sMTP_INFO, userInfo, _mLogger))
                        {
                            if (authentication != null)
                            {
                                authentication.AuthenticationStep = 1;
                                _context.SaveChanges();

                                response.Result = true;
                                response.Data = (from UInfo in _context.UserInfos.Where(p => p.Email.Equals(parameters.email) && p.IsUse && !p.IsDelete)
                                                 select new Ci_User()
                                                 {
                                                     AuthType = UInfo.AuthType,
                                                     Email = UInfo.Email,
                                                     Nickname = UInfo.Nickname,
                                                     AuthenticationStep = authentication.AuthenticationStep,
                                                 }).FirstOrDefault();
                            }
                            else
                            {
                                response = ProcessError<Ci_User>(ErrorCode.EC_EX);
                            }
                        }
                        else
                        {
                            response = ProcessError<Ci_User>(ErrorCode.EC_EX);
                        }
                    }
                    else
                    {
                        response.Result = true;
                        response.Data = (from UInfo in _context.UserInfos.Where(p => p.Email.Equals(parameters.email) && p.IsUse && !p.IsDelete)
                                         select new Ci_User()
                                         {
                                             AuthType = UInfo.AuthType,
                                             Email = UInfo.Email,
                                             Nickname = UInfo.Nickname
                                         }).FirstOrDefault();
                    }

                }

            }
            catch (Exception Exp)
            {
                response = ExceptionError<Ci_User>(Exp.Message);
            }

            return response;
        }


        /// <summary>
        /// 인증 방법 key변경 (비밀번호 /Refrash Tokken ) 
        /// </summary>
        /// <remarks>
        ///  인증 방법 key변경 (비밀번호 /Refrash Tokken ) 
        /// </remarks>
        /// <param name="parameters">
        /// <para>요청형식 설명</para>
        /// <para>- langCode                    : 언어코드 (en-US:영어, ko-KR:한국어)</para>
        /// <para>- authenticationType          : 인증방법 </para>
        /// <para>- email                       : 이메일주소</para>
        /// <para>- authenticationKey           : 가입 Key(EM 일경우 패스워드/ 3rd Party일경우 Tokken) </para>
        /// </param> 
        /// <returns>리턴값 설명</returns>
        /// <response code="200">
        /// <para>응답형식 설명</para>
        /// <para>Result - true/false</para>
        /// <para>ErrorCode - 실패시 오류코드</para>
        /// <para>ErrorMessage - 실패시 오류메세지</para>
        /// <para>Data - 성공시 결과 true/false</para>
        /// </response>
        [Route("UpdateAuthentication")]
        [HttpPost]
        [Produces("application/json")]
        public BaseResponse<bool> UpdateAuthentication(REQ_AUTHENTICATION parameters)
        {
            LogingREQ(parameters);

            BaseResponse<bool> response = new();

            try
            {
                UserAuthentication userAuthentication = (from UAuthentications in _context.UserAuthentications.Where(p => p.AuthenticationType == (int)parameters.authenticationType && p.IsUse && !p.IsDelete)
                                                         join UInfo in _context.UserInfos.Where(p => p.Email.Equals(parameters.email) && p.IsUse && !p.IsDelete)
                                                         on UAuthentications.UserInfoId equals UInfo.Id
                                                         select UAuthentications
                                                 ).FirstOrDefault();

                if (userAuthentication != null)
                {
                    userAuthentication.AuthenticationKey = parameters.authenticationKey;
                    userAuthentication.UpdateTime = DateTime.Now;
                    _context.SaveChanges();
                }
                else
                {
                    //존재하지 않는 Authentication
                    response = ProcessError<bool>(ErrorCode.RE_NEXIST_AUTHENTICATION);
                }

            }
            catch (Exception Exp)
            {
                response = ExceptionError<bool>(Exp.Message);
            }

            LogingRES(response);

            return response;
        }


        /// <summary>
        /// Email 인증 
        /// </summary>
        /// <remarks>
        ///  Email 인증 
        /// </remarks>
        /// <param name="parameters">
        /// <para>요청형식 설명</para>
        /// <para>- langCode                    : 언어코드 (en-US:영어, ko-KR:한국어)</para>
        /// <para>- email                       : 이메일주소</para>
        /// <para>- authenticationStepKey       : 인증 키값 </para>
        /// </param> 
        /// <returns>리턴값 설명</returns>
        /// <response code="200">
        /// <para>응답형식 설명</para>
        /// <para>Result - true/false</para>
        /// <para>ErrorCode - 실패시 오류코드</para>
        /// <para>ErrorMessage - 실패시 오류메세지</para>
        /// <para>Data - 성공시 결과 true/false</para>
        /// </response>
        [Route("AuthenticationStepCheck")]
        [HttpPost]
        [Produces("application/json")]
        public BaseResponse<bool> AuthenticationStepCheck(REQ_AUTHENTICATION_STEP parameters)
        {
            LogingREQ(parameters);

            BaseResponse<bool> response = new();

            try
            {
                UserInfo user =  _context.UserInfos.Where(p => p.Email.Equals(parameters.email) && p.IsUse).FirstOrDefault();

                string Key = CryptographyHelper.GetHash(user.CreateTime.ToString("HHmmssfff")).ToString()[..6];

                if (user != null  && parameters.authenticationStepKey.Equals(Key))
                {

                    UserAuthentication authentication = _context.UserAuthentications.FirstOrDefault(p => p.UserInfoId == user.Id && p.AuthenticationType == (short)AuthenticationType.EM);

                    if (authentication != null)
                    {
                        authentication.AuthenticationStep = 0;
                        _context.SaveChanges();

                        response.Result = true;
                        response.Data = true;
                    }
                    else
                    {
                        response = ProcessError<bool>(ErrorCode.EC_EX);
                    }
                }
                else
                {
                    response = ProcessError<bool>(ErrorCode.EC_EX);
                }

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
