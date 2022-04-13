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
        /// <summary>
        /// 메인윈도우 닫힘 커멘드
        /// </summary>
        public RelayCommand SearchCommand
        {
            get;
            private set;
        }

        private ObservableCollection<MenuView> _subMenuList = new ObservableCollection<MenuView>();
        public ObservableCollection<MenuView> SubMenuList
        {
            get => _subMenuList;
            set { _subMenuList = value; onPropertyChanged(); }
        }


        public SettingMainViewModel()
        {
            SettingCommand();
            SettingSubMenuList();
        }
        private void SettingSubMenuList()
        {
            try
            {
                SubMenuList = new ObservableCollection<MenuView>()
                {
                    new MenuView(),
                    new MenuView(),
                    new MenuView(),
                    new MenuView(),
                    new MenuView(),
                };
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private void SettingCommand()
        {
            try
            {
                SearchCommand = new RelayCommand(Search);
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
    }
}
