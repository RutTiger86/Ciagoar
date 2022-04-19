using Ciagoar.Control.Command;
using CiagoarM.Commons;
using CiagoarM.Views.Setting;
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

        public RelayCommand MenuSelectCommand
        {
            get;
            private set;
        }


        private object _settingView = null;

        public object SettingView
        {
            get
            {
                return _settingView;
            }
            set
            {
                _settingView = value;
                onPropertyChanged();
            }
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
                MenuSelectCommand = new RelayCommand(MenuSelect);
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private void Search(object parm)
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private void MenuSelect(object parm)
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
