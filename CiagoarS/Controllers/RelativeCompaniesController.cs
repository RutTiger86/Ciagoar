using Ciagoar.Data.Request.RelativeCompanies;
using Ciagoar.Data.Response;
using Ciagoar.Data.Response.RelativecCompanies;
using CiagoarS.Common;
using CiagoarS.Common.Enums;
using CiagoarS.DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CiagoarS.Controllers
{
    /// <summary>
    /// 관계사 관련( 제조사, 납품사 등)
    /// </summary>
    public class RelativeCompaniesController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        public RelativeCompaniesController(ILogger<UsersController> logger, CiagoarContext context)
        {
            _mLogger = logger;
            _mContext = context;
        }

        /// <summary>
        /// 관계사 리스트
        /// </summary>
        /// <remarks>
        ///  관계사 리스트를 가져온다
        /// </remarks>
        /// <param name="parameters">
        /// <para>요청형식 설명</para>
        /// <para>- langCode          : 언어코드 (en-US:영어, ko-KR:한국어)</para>
        /// <para>- sortOrder         : 정렬값 (필드값_정렬타입) ex: ID_DESC</para>
        /// <para>- searchString      : 검색값 (ID, CoName, PhoneNumber, CoAddress 지원) </para>
        /// <para>- pageCount         : 페이지당 갯수 </para>
        /// <para>- pageIndex         : 페이지 인덱스 </para>
        /// </param> 
        /// <returns>리턴값 설명</returns>
        /// <response code="200">
        /// <para>응답형식 설명</para>
        /// <para>Result - true/false</para>
        /// <para>ErrorCode - 실패시 오류코드</para>
        /// <para>ErrorMessage - 실패시 오류메세지</para>
        /// <para>Data - 성공시 결과</para>
        /// <para>     - Id           : ID 키 </para>
        /// <para>     - CoName       : 관계사명 </para>
        /// <para>     - CoAddress    : 주소 </para>
        /// <para>     - PhoneNumber  : 연락처 </para>
        /// <para>     - ConnectUrl   : 연락 URL </para>
        /// <para>     - Memo         : 메모 </para>
        /// </response>
       
        [HttpGet]
        [Produces("application/json")]
        public BaseResponse<List<Ci_RELATVE_CO>> GetRelativeCompanies([FromQuery] REQ_RELATVE_CO_LIST parameters)
        {
            LogingREQ(parameters);

            BaseResponse<List<Ci_RELATVE_CO>> response = new();

            try
            {
                IQueryable<RelativeCo> relativeCos = _mContext.RelativeCos.Where(p => p.Isdelete == false);

                if (!string.IsNullOrWhiteSpace(parameters.searchString))
                {
                    relativeCos = relativeCos.Where(p => p.Id.ToString().Contains(parameters.searchString)
                    || p.CoName.Contains(parameters.searchString)
                    || p.PhoneNumber.Contains(parameters.searchString)
                    || p.CoAddress.Contains(parameters.searchString));
                }

                if (!string.IsNullOrWhiteSpace(parameters.sortOrder))
                {
                    switch (parameters.sortOrder.ToUpper())
                    {
                        case "ID_DESC": relativeCos = relativeCos.OrderByDescending(p => p.Id); break;
                        case "CONAME": relativeCos = relativeCos.OrderBy(p => p.CoName); break;
                        case "CONAME_DESC": relativeCos = relativeCos.OrderByDescending(p => p.CoName); break;
                        case "COADDRESS": relativeCos = relativeCos.OrderBy(p => p.CoAddress); break;
                        case "COADDRESS_DESC": relativeCos = relativeCos.OrderByDescending(p => p.CoAddress); break;
                        case "PHONENUMBER": relativeCos = relativeCos.OrderBy(p => p.PhoneNumber); break;
                        case "PHONENUMBER_DESC": relativeCos = relativeCos.OrderByDescending(p => p.PhoneNumber); break;
                        default: relativeCos = relativeCos.OrderBy(p => p.Id); break;
                    }
                }
                else
                {
                    relativeCos = relativeCos.OrderBy(p => p.Id);
                }

                response.Result = true;
                response.Data = relativeCos.Skip(parameters.pageCount* parameters.pageIndex-1)
                    .Take(parameters.pageCount)
                    .Select(p => new Ci_RELATVE_CO()
                    {
                        Id = p.Id,
                        CoAddress = p.CoAddress,
                        CoName = p.CoName,
                        ConnectUrl = p.ConnectUrl,
                        Memo = p.Memo,
                        PhoneNumber = p.PhoneNumber,
                        CreateTime = p.Createtime,
                        UpdateTime = p.Updatetime
                    }).ToList();
            }
            catch (Exception Exp)
            {
                response = ExceptionError<List<Ci_RELATVE_CO>>(Exp.Message);
            }

            LogingRES(response);

            return response;
        }

        /// <summary>
        /// 관계사 추가
        /// </summary>
        /// <remarks>
        ///  관계사를 추가 한다.
        /// </remarks>
        /// <param name="parameters">
        /// <para>요청형식 설명</para>
        /// <para>- langCode          : 언어코드 (en-US:영어, ko-KR:한국어)</para>
        /// <para>- coName            : 관계사 명</para>
        /// <para>- coAddress         : 주소 </para>
        /// <para>- phoneNumber       : 연락 처 </para>
        /// <para>- connectUrl        : 연락 URL </para>
        /// <para>- memo              : 메모 </para>
        /// <para>- isuse             : 사용여부 </para>
        /// </param> 
        /// <returns>리턴값 설명</returns>
        /// <response code="200">
        /// <para>응답형식 설명</para>
        /// <para>Result - true/false</para>
        /// <para>ErrorCode - 실패시 오류코드</para>
        /// <para>ErrorMessage - 실패시 오류메세지</para>
        /// <para>Data - 입력된 Company ID </para>
        /// </response>
        [Route("RelativeCompany")]
        [HttpPost]
        [Produces("application/json")]
        public BaseResponse<int> InserRelativeCompany([FromBody] REQ_RELATVE_CO_PUT parameters)
        {
            LogingREQ(parameters);

            BaseResponse<int> response = new();

            try
            {
                RelativeCo relativeCo = new()
                {
                    CoAddress = parameters.coAddress,
                    CoName=parameters.coName,
                    ConnectUrl=parameters.connectUrl,
                    Createtime = DateTime.Now.ToUniversalTime(),
                    Memo = parameters.memo,
                    Isuse = parameters.isuse,
                    Isdelete = false,
                    PhoneNumber = parameters.phoneNumber,
                    Updatetime = null
                };

                _mContext.RelativeCos.Add(relativeCo);
                _mContext.SaveChanges();


                response.Result = true;
                response.Data = relativeCo.Id;
            }
            catch (Exception Exp)
            {
                response = ExceptionError<int>(Exp.Message);
            }

            LogingRES(response);

            return response;
        }


        /// <summary>
        /// 관계사 수정
        /// </summary>
        /// <remarks>
        ///  관계사를 수정 한다.
        /// </remarks>
        /// <param name="parameters">
        /// <para>요청형식 설명</para>
        /// <para>- langCode          : 언어코드 (en-US:영어, ko-KR:한국어)</para>
        /// <para>- id                : 관계사 ID</para>
        /// <para>- coName            : 관계사 명</para>
        /// <para>- coAddress         : 주소 </para>
        /// <para>- phoneNumber       : 연락 처 </para>
        /// <para>- connectUrl        : 연락 URL </para>
        /// <para>- memo              : 메모 </para>
        /// <para>- isuse             : 사용여부 </para>
        /// </param> 
        /// <returns>리턴값 설명</returns>
        /// <response code="200">
        /// <para>응답형식 설명</para>
        /// <para>Result - true/false</para>
        /// <para>ErrorCode - 실패시 오류코드</para>
        /// <para>ErrorMessage - 실패시 오류메세지</para>
        /// <para>Data - 수정된 Company ID </para>
        /// </response>
        [Route("RelativeCompany")]
        [HttpPut]
        [Produces("application/json")]
        public BaseResponse<int> UpdateRelativeCompany([FromBody] REQ_RELATVE_CO_POST parameters)
        {
            LogingREQ(parameters);

            BaseResponse<int> response = new();

            try
            {
                RelativeCo relativeCo = _mContext.RelativeCos.FirstOrDefault(p => p.Id == parameters.id);

                if(relativeCo != null)
                {
                    relativeCo.CoName = parameters.coName;
                    relativeCo.CoAddress = parameters.coAddress;
                    relativeCo.PhoneNumber = parameters.phoneNumber;
                    relativeCo.ConnectUrl = parameters.connectUrl;
                    relativeCo.Memo = parameters.memo;
                    relativeCo.Isuse = parameters.isuse;
                    relativeCo.Updatetime = DateTime.Now.ToUniversalTime();

                    _mContext.SaveChanges();

                    response.Result = true;
                    response.Data = relativeCo.Id;

                }
                else
                {
                    response = ProcessError<int>(ErrorCode.RE_NEXIST_RELATIVECOMPANY_ID);
                }
            }
            catch (Exception Exp)
            {
                response = ExceptionError<int>(Exp.Message);
            }

            LogingRES(response);

            return response;
        }

        /// <summary>
        /// 관계사 삭제
        /// </summary>
        /// <remarks>
        ///  관계사를 삭제 한다.
        /// </remarks>
        /// <param name="parameters">
        /// <para>요청형식 설명</para>
        /// <para>- langCode          : 언어코드 (en-US:영어, ko-KR:한국어)</para>
        /// <para>- id                : 관계사 ID</para>
        /// </param> 
        /// <returns>리턴값 설명</returns>
        /// <response code="200">
        /// <para>응답형식 설명</para>
        /// <para>Result - true/false</para>
        /// <para>ErrorCode - 실패시 오류코드</para>
        /// <para>ErrorMessage - 실패시 오류메세지</para>
        /// <para>Data - 삭제된 Company ID </para>
        /// </response>
        [Route("RelativeCompany")]
        [HttpDelete]
        [Produces("application/json")]
        public BaseResponse<int> DeleteRelativeCompany([FromBody] REQ_RELATVE_CO_DELETE parameters)
        {
            LogingREQ(parameters);

            BaseResponse<int> response = new();

            try
            {
                RelativeCo relativeCo = _mContext.RelativeCos.FirstOrDefault(p => p.Id == parameters.id);

                if (relativeCo != null)
                {
                    relativeCo.Isdelete = true;
                    relativeCo.Updatetime = DateTime.Now.ToUniversalTime();

                    _mContext.SaveChanges();

                    response.Result = true;
                    response.Data = relativeCo.Id;
                }
                else
                {
                    response = ProcessError<int>(ErrorCode.RE_NEXIST_RELATIVECOMPANY_ID);
                }
            }
            catch (Exception Exp)
            {
                response = ExceptionError<int>(Exp.Message);
            }

            LogingRES(response);

            return response;
        }
    }
}
