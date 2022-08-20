using CiagoarM.Commons;
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

    }
}
