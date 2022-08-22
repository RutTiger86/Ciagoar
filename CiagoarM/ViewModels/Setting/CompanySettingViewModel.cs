using Ciagoar.Data.Response.RelativecCompanies;
using CiagoarM.Commons;
using CiagoarM.Datas.Company;
using CiagoarM.Models;
using CiagoarM.Views.Setting;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiagoarM.ViewModels.Setting
{
    public class CompanySettingViewModel : BaseModel
    {
        #region BindingProperty

        private ObservableCollection<RelayCompInfo> _companyList = new ();

        public ObservableCollection<RelayCompInfo> CompanyList
        {
            get => _companyList;
            set => SetProperty(ref _companyList, value);
        }


        #endregion

        public RelayCommand LoadedCommand
        {
            get;
            private set;
        }

        public RelayCommand AddCompanyCommand
        {
            get;
            private set;
        }

        public RelayCommand DeleteCompanyCommand
        {
            get;
            private set;
        }

        public RelayCommand UpdateCompanyCommand
        {
            get;
            private set;
        }

        public CompanySettingViewModel()
        {
            SettingCommand();
        }

        private void SettingCommand()
        {
            try
            {
                LoadedCommand = new RelayCommand(LoadedView);
                AddCompanyCommand = new RelayCommand(AddCompany);
                DeleteCompanyCommand = new RelayCommand(DeleteCompany);
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private void AddCompany()
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private void DeleteCompany()
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private void UpdateCompany()
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private async void LoadedView()
        {
            try
            {
                List<Ci_RELATVE_CO> compInfos =  await new RelativeCompanyModel().GetRelativeCompany("", "", 1000, 1);
                CompanyList = new ObservableCollection<RelayCompInfo>(compInfos.Select(p=> new RelayCompInfo(p)).ToList());
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

    }
}
