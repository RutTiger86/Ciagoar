using Ciagoar.Core.Attributes;
using Ciagoar.Data.Response.RelativecCompanies;
using CiagoarM.Languages;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CiagoarM.Datas.Company
{
    public class RelayCompInfo : ObservableObject
    {
        private Ci_RELATVE_CO _companyInfo;

        [Browsable(false)]
        public Ci_RELATVE_CO CompanyInfo
        {
            get => _companyInfo;
            set => SetProperty(ref _companyInfo, value);
        }


        [LocalizedDisplayName("Title_ID", typeof(Resource))]
        public int ID
        {
            get => _companyInfo.Id;
            set 
            { 
                _companyInfo.Id = value;
                OnPropertyChanged();
            }
        }




        public RelayCompInfo(Ci_RELATVE_CO MainData)
        {
            _companyInfo = MainData;
        }
    }
}
