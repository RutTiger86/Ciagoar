using Azure;
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
using CiagoarS.Interface;
using CiagoarS.Repositorys;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiagoarS.Controllers
{
    /// <summary>
    /// 사용자 관련 API
    /// </summary>
    public class UsersController : BaseController
    {
        private const string mSalt = "ciagoar";
        private const int mIt_count = 5;
        private const int mLength = 128;

        private readonly IUserRepository mUsers;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        public UsersController(IUserRepository users, ILogger<UsersController> logger, CiagoarContext context)
        {
            _mLogger = logger;

            _mContext = context;

            mUsers = users;
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
        /// <para>- authType          : 인증방법 </para>
        /// <para>- email                       : 이메일주소</para>
        /// <para>- authKey           : 가입 Key(EM 일경우 패스워드/ 3rd Party일경우 Tokken) </para>
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
                switch ((AuthType)parameters.authType)
                {
                    case AuthType.EM: response = await SetUserLoginEmail(parameters.email, parameters.authKey); break;
                    case AuthType.GG: response = await SetUserLoginGoogle(parameters.email); break;
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

        private async Task<BaseResponse<Ci_User>> SetUserLoginEmail(string Email, string Password)
        {
            try
            {
                Password = CryptographyHelper.GetPbkdf2(Password, mSalt, mIt_count, mLength);

                Ci_User CiUser = await mUsers.GetUserByEmailPWAsync(Email, Password);

                if (CiUser != null)
                {
                    return new BaseResponse<Ci_User>
                    {
                        Result = true,
                        Data = CiUser
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

        private async Task<BaseResponse<Ci_User>> SetUserLoginGoogle(string Email)
        {
            try
            {
                Tuple<Ci_User, string> CiUserAuth = await mUsers.GetUserAuthkeyByEmailAsync(Email);

                if (CiUserAuth != default)
                {
                    List<AuthInfo> oAuth_Info_List = await mUsers.GetAuthInfoByTypeAsync(AuthType.GG);

                    if (oAuth_Info_List.Count > 3
                        && oAuth_Info_List.Any(p => p.FieldName.Equals("authuri"))
                        && oAuth_Info_List.Any(p => p.FieldName.Equals("clientid"))
                        && oAuth_Info_List.Any(p => p.FieldName.Equals("clientsecret"))
                        && oAuth_Info_List.Any(p => p.FieldName.Equals("tokenuri")))
                    {

                        Ci_OAuth ci_OAuth = new()
                        {
                            TypeCode = (short)AuthType.GG,
                            AuthUri = oAuth_Info_List.Where(p => p.FieldName.Equals("authuri")).First().FieldValue,
                            ClientId = oAuth_Info_List.Where(p => p.FieldName.Equals("clientid")).First().FieldValue,
                            ClientSecret = oAuth_Info_List.Where(p => p.FieldName.Equals("clientsecret")).First().FieldValue,
                            TokenUri = oAuth_Info_List.Where(p => p.FieldName.Equals("tokenuri")).First().FieldValue
                        };

                        BaseResponse<GoogleOAuth> response = await Google.RefrashAccessToken(ci_OAuth, CiUserAuth.Item2);

                        if (response.Result)
                        {
                            //엑세스 토큰 성공 호출 로그인 진행 
                            return new BaseResponse<Ci_User>
                            {
                                Result = true,
                                Data = CiUserAuth.Item1
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
                        return ProcessError<Ci_User>(ErrorCode.RE_OAUTH_UNSUPPORTED_3RDPARTY_LOGIN);
                    }

                }
                else
                {
                    // 구글 로그인 사용자 없음 
                    if (await mUsers.CheckUserByEmailAsync(Email))
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
        /// <para>- authType          : 인증방법 </para>
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
        public  async Task<BaseResponse<Ci_OAuth>> GetOAuthInfo([FromQuery] REQ_OAUTH_INFO parameters)
        {
            LogingREQ(parameters);

            BaseResponse<Ci_OAuth> response = new();

            try
            {

                List<AuthInfo> oAuth_Info_List = await mUsers.GetAuthInfoByTypeAsync((AuthType)parameters.authType);

                if (oAuth_Info_List.Count > 3
                    && oAuth_Info_List.Any(p => p.FieldName.Equals("authuri"))
                    && oAuth_Info_List.Any(p => p.FieldName.Equals("clientid"))
                    && oAuth_Info_List.Any(p => p.FieldName.Equals("clientsecret"))
                    && oAuth_Info_List.Any(p => p.FieldName.Equals("tokenuri")))
                {

                    Ci_OAuth ci_OAuth = new()
                    {
                        TypeCode = (short)parameters.authType,
                        AuthUri = oAuth_Info_List.Where(p => p.FieldName.Equals("authuri")).First().FieldValue,
                        ClientId = oAuth_Info_List.Where(p => p.FieldName.Equals("clientid")).First().FieldValue,
                        ClientSecret = oAuth_Info_List.Where(p => p.FieldName.Equals("clientsecret")).First().FieldValue,
                        TokenUri = oAuth_Info_List.Where(p => p.FieldName.Equals("tokenuri")).First().FieldValue
                    };

                    response = new() { Result = true, Data = ci_OAuth };
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
        /// <para>- authType          : 인증방법 </para>
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
        public async Task<BaseResponse<Ci_User>> SetJoinUser(REQ_USER_JOIN parameters)
        {
            LogingREQ(parameters);

            BaseResponse<Ci_User> Response ;

            try
            {

                if (await mUsers.CheckUserByEmailAsync(parameters.email))
                {
                    Response = await InsertUserInfoAsync(parameters);
                }
                else
                {
                    Response = await ConnectUserAuthenticationAsync(parameters);
                }

            }
            catch (Exception Exp)
            {
                Response = ExceptionError<Ci_User>(Exp.Message);
            }

            LogingRES(Response);

            return Response;
        }

        private async Task<BaseResponse<Ci_User>> InsertUserInfoAsync(REQ_USER_JOIN parameters)
        {
            BaseResponse<Ci_User> Response = new();

            try
            {
                string Password = CryptographyHelper.GetPbkdf2(parameters.authKey, mSalt, mIt_count, mLength);
              
                //신규가입
                UserInfo UInfo = new()
                {
                    Email = parameters.email,
                    TypeCode = (int)UserType.User,
                    Nickname = parameters.nickname,
                    Isuse = true,
                    UserAuths = new List<UserAuth>()
                    {
                        new UserAuth()
                        {
                            TypeCode = parameters.authType,
                            AuthKey = Password,
                            AuthStep = (short)(parameters.authType == (short)AuthType.EM? 2 : 0),
                            Isuse = true,
                        }
                    }
                };

                Ci_User CiUser = await mUsers.InsertUserInfoAsync(UInfo);

                if (CiUser != null)
                {
                    Response.Result = true;
                    Response.Data = CiUser;
                }                

                if (parameters.authType == (short)AuthType.EM  && !SendEmailProcess(UInfo))
                {
                    Response = ProcessError<Ci_User>(ErrorCode.EC_EX);
                }
            }
            catch (Exception Exp)
            {
                Response = ExceptionError<Ci_User>(Exp.Message);
            }

            return Response;
        }

        private async Task<BaseResponse<Ci_User>> ConnectUserAuthenticationAsync( REQ_USER_JOIN parameters)
        {
            BaseResponse<Ci_User> Response = new();

            try
            {
                UserInfo UInfo = await mUsers.GetUserByEmailAsync(parameters.email);

                UserAuth UInfoAuth = await mUsers.GetUserAuthsAsync(UInfo.Id, parameters.authType);

                if (UInfoAuth != null)
                {
                    //사용자 정보도 있고 로그인 정보도 있음
                    if (parameters.authType != (int)AuthType.EM)
                    {
                        // RefrashCodeUpdate
                        _ = await mUsers.UpdateAuthKeyAsync(UInfoAuth.Id, parameters.authKey);

                        Response.Result = true;
                        Response.Data = new Ci_User()
                        {
                            TypeCode = UInfo.TypeCode,
                            Email = UInfo.Email,
                            Nickname = UInfo.Nickname
                        };
                    }
                    else
                    {
                        // Email계정으로 등록 불가 
                        Response = ProcessError<Ci_User>(ErrorCode.RE_EXIST_USER);
                    }
                }
                else
                {
                    string Password = CryptographyHelper.GetPbkdf2(parameters.authKey, mSalt, mIt_count, mLength);

                    //사용자 정보는 있으나 로그인 정보는 없음 계정 연결 
                    UserAuth userAuth = new()
                    {
                        TypeCode = parameters.authType,
                        AuthKey = parameters.authType == (int)AuthType.EM ? Password : parameters.authKey,
                        UserInfoId = UInfo.Id,
                        Isuse = true,
                    };
                    
                    if(await mUsers.InsertUserAuthAsync(userAuth) != -1)
                    {
                        Response.Result = true;
                        Response.Data =  new Ci_User()
                                         {
                                             TypeCode = UInfo.TypeCode,
                                             Email = UInfo.Email,
                                             Nickname = UInfo.Nickname
                                         };
                    }


                    if (parameters.authType == (short)AuthType.EM && !SendEmailProcess(UInfo))
                    {
                        Response = ProcessError<Ci_User>(ErrorCode.EC_EX);
                    }

                }

            }
            catch (Exception Exp)
            {
                Response = ExceptionError<Ci_User>(Exp.Message);
            }

            return Response;
        }

        private bool SendEmailProcess(UserInfo UInfo)
        {
            try
            {
                List<AuthInfo> SMTP_Info_List = _mContext.AuthInfos.Where(p => p.TypeCode == (short)AuthType.EM).ToList();

                if (SMTP_Info_List.Count > 3
                    && SMTP_Info_List.Any(p => p.FieldName.Equals("smtpport"))
                    && SMTP_Info_List.Any(p => p.FieldName.Equals("smtppassword"))
                    && SMTP_Info_List.Any(p => p.FieldName.Equals("smtpserver"))
                    && SMTP_Info_List.Any(p => p.FieldName.Equals("smtpuser")))
                {

                    SMTP_INFO sMTP_INFO = new()
                    {
                        nSMTPPort = Convert.ToInt32(SMTP_Info_List.Where(p => p.FieldName.Equals("smtpport")).First().FieldValue),
                        sSMTPPassword = SMTP_Info_List.Where(p => p.FieldName.Equals("smtppassword")).First().FieldValue,
                        sSMTPServer = SMTP_Info_List.Where(p => p.FieldName.Equals("smtpserver")).First().FieldValue,
                        sSMTPUser = SMTP_Info_List.Where(p => p.FieldName.Equals("smtpuser")).First().FieldValue

                    };

                    if (sMTP_INFO != null)
                    {
                        return EmailHelper.SendEMail(sMTP_INFO, UInfo, _mLogger);
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

            return false;
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
        /// <para>- authType          : 인증방법 </para>
        /// <para>- email                       : 이메일주소</para>
        /// <para>- authKey           : 가입 Key(EM 일경우 패스워드/ 3rd Party일경우 Tokken) </para>
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
        [HttpPut]
        [Produces("application/json")]
        public async Task<BaseResponse<bool>> UpdateAuthentication(REQ_AUTH parameters)
        {
            LogingREQ(parameters);

            BaseResponse<bool> Response = new();

            try
            {
                UserAuth UAuth = await mUsers.GetUserAuthsByEmailAsync(parameters.email, parameters.authType);

                if (UAuth != null)
                {
                    if(await mUsers.UpdateAuthKeyAsync(UAuth.Id, parameters.authKey) != -1)
                    {
                        Response.Result = true;
                        Response.Data = true;
                    }
                }
                else
                {
                    //존재하지 않는 Authentication
                    Response = ProcessError<bool>(ErrorCode.RE_NEXIST_AUTHENTICATION);
                }

            }
            catch (Exception Exp)
            {
                Response = ExceptionError<bool>(Exp.Message);
            }

            LogingRES(Response);

            return Response;
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
        /// <para>- authStepKey       : 인증 키값 </para>
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
        [HttpPut]
        [Produces("application/json")]
        public BaseResponse<bool> AuthenticationStepCheck(REQ_AUTH_STEP parameters)
        {
            LogingREQ(parameters);

            BaseResponse<bool> response = new();

            try
            {
                UserInfo user = _mContext.UserInfos.Where(p => p.Email.Equals(parameters.email) && (bool)p.Isuse).FirstOrDefault();

                string Key = CryptographyHelper.GetHash(user.Createtime.ToString("HHmmssfff")).ToString()[..6];

                if (user != null && parameters.authStepKey.Equals(Key))
                {

                    UserAuth auth = _mContext.UserAuths.FirstOrDefault(p => p.UserInfoId == user.Id && p.TypeCode == (short)AuthType.EM);

                    if (auth != null)
                    {
                        auth.AuthStep = 0;
                        _mContext.SaveChanges();

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
