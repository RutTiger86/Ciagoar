using Ciagoar.Data.Enums;
using Ciagoar.Data.Response.RelativecCompanies;
using Ciagoar.Data.Response.Users;
using CiagoarS.DataBase;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CiagoarS.Interface
{
    public interface IRelatevCoRepository
    {
        Task<List<Ci_RELATVE_CO>> CheckUserByEmailAsync(string SearchString, string SortOrder, int PageCount, int PageIndex);

        Task<int> InsertRelativeCoAsync(RelativeCo InsertRelativeCo);

        Task<int> UpdateRelativeCoAsync(RelativeCo UpdateRelativeCo);

        Task<int> DeleteRelativeCoAsync(int DeleteRelativeCoId);
    }
}
