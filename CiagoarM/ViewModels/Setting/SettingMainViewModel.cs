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
    public class SettingMainViewModel : BaseModel
    {
        public RelayCommand SearchCommand
        {
            get;
            private set;
        }

        public RelayCommand<BaseView> MenuSelectCommand
        {
            get;
            private set;
        }


        private BaseView _settingView = null;

        public BaseView SettingView
        {
            get => _settingView;
            set => SetProperty(ref _settingView, value);
        }

        public SettingMainViewModel()
        {
            SettingCommand();
        }

        private void SettingCommand()
        {
            try
            {
                SearchCommand = new RelayCommand(Search);
                MenuSelectCommand = new RelayCommand<BaseView>(p=> MenuSelect(p));
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private void Search()
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private void MenuSelect(BaseView parm)
        {
            try
            {
                SettingView = parm;
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }
    }
}
