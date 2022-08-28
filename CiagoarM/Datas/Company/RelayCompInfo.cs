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
        [CustomColumnAttributes(MinWidth = 60, StarWidth =60)]
        public int ID
        {
            get => _companyInfo.Id;
            set 
            { 
                _companyInfo.Id = value;
                OnPropertyChanged();
            }
        }

        [LocalizedDisplayName("Title_CoName", typeof(Resource))]
        [CustomColumnAttributes(MinWidth = 120, StarWidth = 120)]
        public string CoName
        {
            get => _companyInfo.CoName;
            set
            {
                _companyInfo.CoName = value;
                OnPropertyChanged();
            }
        }

        [LocalizedDisplayName("Title_CoAddress", typeof(Resource))]
        [Browsable(false)]
        public string CoAddress
        {
            get => _companyInfo.CoAddress;
            set
            {
                _companyInfo.CoAddress = value;
                OnPropertyChanged();
            }
        }

        [LocalizedDisplayName("Title_PhoneNumber", typeof(Resource))]
        [CustomColumnAttributes(MinWidth = 100, StarWidth = 100)]
        public string PhoneNumber
        {
            get => _companyInfo.PhoneNumber;
            set
            {
                _companyInfo.PhoneNumber = value;
                OnPropertyChanged();
            }
        }

        [LocalizedDisplayName("Title_Memo", typeof(Resource))]
        [Browsable(false)]
        public string Memo
        {
            get => _companyInfo.Memo;
            set
            {
                _companyInfo.Memo = value;
                OnPropertyChanged();
            }
        }

        [LocalizedDisplayName("Title_CreateTime", typeof(Resource))]
        [CustomColumnAttributes(MinWidth = 120, StarWidth = 120)]
        public DateTime CreateTime
        {
            get => _companyInfo.CreateTime;
            set
            {
                _companyInfo.CreateTime = value;
                OnPropertyChanged();
            }
        }

        [LocalizedDisplayName("Title_UpdateTime", typeof(Resource))]
        [CustomColumnAttributes(MinWidth = 120, StarWidth = 120)]
        public DateTime? UpdateTime
        {
            get => _companyInfo.UpdateTime;
            set
            {
                _companyInfo.UpdateTime = value;
                OnPropertyChanged();
            }
        }

        public RelayCompInfo(Ci_RELATVE_CO MainData)
        {
            _companyInfo = MainData;
        }
    }
}
