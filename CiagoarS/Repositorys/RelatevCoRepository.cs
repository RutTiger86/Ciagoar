using Azure;
using Ciagoar.Core.Helper;
using Ciagoar.Data.Enums;
using Ciagoar.Data.HTTPS;
using Ciagoar.Data.Request.Users;
using Ciagoar.Data.Response;
using Ciagoar.Data.Response.RelativecCompanies;
using Ciagoar.Data.Response.Users;
using CiagoarS.Common;
using CiagoarS.Controllers;
using CiagoarS.DataBase;
using CiagoarS.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CiagoarS.Repositorys
{
    public class RelatevCoRepository : IRelatevCoRepository
    {
        protected CiagoarContext mContext;
        public RelatevCoRepository(CiagoarContext Context)
        {
            mContext = Context;
        }

        public async Task<List<Ci_RELATVE_CO>> CheckUserByEmailAsync(string SearchString, string SortOrder, int PageCount, int PageIndex)
        {
            try
            {
                IQueryable<RelativeCo> mRelativeCos = mContext.RelativeCos.Where(p => p.Isdelete == false);

                if (!string.IsNullOrWhiteSpace(SearchString))
                {
                    mRelativeCos = mRelativeCos.Where(p => p.Id.ToString().Contains(SearchString)
                    || p.CoName.Contains(SearchString)
                    || p.PhoneNumber.Contains(SearchString)
                    || p.CoAddress.Contains(SearchString));
                }

                if (!string.IsNullOrWhiteSpace(SortOrder))
                {
                    switch (SortOrder.ToUpper())
                    {
                        case "ID_DESC": mRelativeCos = mRelativeCos.OrderByDescending(p => p.Id); break;
                        case "CONAME": mRelativeCos = mRelativeCos.OrderBy(p => p.CoName); break;
                        case "CONAME_DESC": mRelativeCos = mRelativeCos.OrderByDescending(p => p.CoName); break;
                        case "COADDRESS": mRelativeCos = mRelativeCos.OrderBy(p => p.CoAddress); break;
                        case "COADDRESS_DESC": mRelativeCos = mRelativeCos.OrderByDescending(p => p.CoAddress); break;
                        case "PHONENUMBER": mRelativeCos = mRelativeCos.OrderBy(p => p.PhoneNumber); break;
                        case "PHONENUMBER_DESC": mRelativeCos = mRelativeCos.OrderByDescending(p => p.PhoneNumber); break;
                        default: mRelativeCos = mRelativeCos.OrderBy(p => p.Id); break;
                    }
                }
                else
                {
                    mRelativeCos = mRelativeCos.OrderBy(p => p.Id);
                }
                
                return await mRelativeCos.Skip(PageCount * PageIndex - 1)
                    .Take(PageCount)
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
                    }).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> InsertRelativeCoAsync(RelativeCo InsertRelativeCo)
        {
            try
            {
                mContext.RelativeCos.Add(InsertRelativeCo);
                return await mContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateRelativeCoAsync(RelativeCo UpdateRelativeCo)
        {
            try
            {
                if (await mContext.RelativeCos.FindAsync(UpdateRelativeCo.Id) is RelativeCo mRelativeCo)
                {
                    mRelativeCo.CoName = UpdateRelativeCo.CoName;
                    mRelativeCo.CoAddress = UpdateRelativeCo.CoAddress;
                    mRelativeCo.PhoneNumber = UpdateRelativeCo.PhoneNumber;
                    mRelativeCo.ConnectUrl = UpdateRelativeCo.ConnectUrl;
                    mRelativeCo.Memo = UpdateRelativeCo.Memo;
                    mRelativeCo.Isuse = UpdateRelativeCo.Isuse;
                    mRelativeCo.Updatetime = DateTime.Now.ToUniversalTime();

                    return await mContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return -1;
        }

        public async Task<int> DeleteRelativeCoAsync(int DeleteRelativeCoId)
        {
            try
            {
                RelativeCo relativeCo = mContext.RelativeCos.FirstOrDefault(p => p.Id == DeleteRelativeCoId);

                if (relativeCo != null)
                {
                    relativeCo.Isdelete = true;
                    relativeCo.Updatetime = DateTime.Now.ToUniversalTime();
                }

                return await mContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }




    }
}
