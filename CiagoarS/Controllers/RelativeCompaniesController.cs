using Ciagoar.Data.Request.RelativeCompanies;
using Ciagoar.Data.Response;
using Ciagoar.Data.Response.RelativecCompanies;
using CiagoarS.Common;
using CiagoarS.Common.Enums;
using CiagoarS.DataBase;
using CiagoarS.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CiagoarS.Controllers
{
    /// <summary>
    /// 관계사 관련( 제조사, 납품사 등)
    /// </summary>
    public class RelativeCompaniesController : BaseController
    {

        private readonly IRelatevCoRepository mRelaytevCo;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="RelaytevCo"></param> 
        /// <param name="logger"></param>
        /// <param name="context"></param>
        public RelativeCompaniesController(IRelatevCoRepository RelaytevCo, ILogger<RelativeCompaniesController> logger, CiagoarContext context)
        {
            _mLogger = logger;
            mRelaytevCo = RelaytevCo;
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
        public async Task<BaseResponse<List<Ci_RELATVE_CO>>> GetRelativeCompanies([FromQuery] REQ_RELATVE_CO_LIST parameters)
        {
            LogingREQ(parameters);

            BaseResponse<List<Ci_RELATVE_CO>> response = new();

            try
            {
                response.Result = true;
                response.Data = await mRelaytevCo.CheckUserByEmailAsync(parameters.searchString, parameters.sortOrder, parameters.pageCount, parameters.pageIndex);
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
                RelativeCo mRelativeCo = new()
                {
                    CoAddress = parameters.coAddress,
                    CoName = parameters.coName,
                    ConnectUrl = parameters.connectUrl,
                    Createtime = DateTime.Now.ToUniversalTime(),
                    Memo = parameters.memo,
                    Isuse = parameters.isuse,
                    Isdelete = false,
                    PhoneNumber = parameters.phoneNumber,
                    Updatetime = null
                };

                mRelaytevCo.InsertRelativeCoAsync(mRelativeCo);

                response.Result = true;
                response.Data = mRelativeCo.Id;
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
        public async Task<BaseResponse<int>> UpdateRelativeCompany([FromBody] REQ_RELATVE_CO_POST parameters)
        {
            LogingREQ(parameters);

            BaseResponse<int> response = new();

            try
            {
                RelativeCo relativeCo = new RelativeCo()
                {
                    Id = parameters.id,
                    CoName = parameters.coName,
                    CoAddress = parameters.coAddress,
                    PhoneNumber = parameters.phoneNumber,
                    ConnectUrl = parameters.connectUrl,
                    Memo = parameters.memo,
                    Isuse = parameters.isuse,
                    Updatetime = DateTime.Now.ToUniversalTime()

                };

                if (await mRelaytevCo.UpdateRelativeCoAsync(relativeCo) >0)
                {
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
        public async Task<BaseResponse<int>> DeleteRelativeCompany([FromBody] REQ_RELATVE_CO_DELETE parameters)
        {
            LogingREQ(parameters);

            BaseResponse<int> response = new();

            try
            {
                int result = await mRelaytevCo.DeleteRelativeCoAsync(parameters.id);

                if (result > 0)
                {
                    response.Result = true;
                    response.Data = parameters.id;
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
