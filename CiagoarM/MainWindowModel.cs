using Ciagoar.Control.Command;
using Ciagoar.Core.Interface;
using CiagoarM.Commons;
using CiagoarM.Views.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CiagoarM
{
    public class MainWindowModel: BaseViewModel, ICloseWindows
    {
        #region Command

        public RelayCommand WindowLoadedCommand
        {
            get;
            private set;
        }

        #endregion

        #region Variable
        private MainWindow _main { get; set; }

        public Action Close { get; set; }       

        LoginWindows _loginWindows;
        private LoginWindows loginWindows
        {
            get
            {
                if(_loginWindows == null)
                {
                    _loginWindows = new LoginWindows();
                    _loginWindows.Closed += _loginWindows_Closed;
                }

                return _loginWindows;
            }
        }

        #endregion
        public MainWindowModel()
        {
            try
            {
                LogInfo("★★★★★ MainWindowModel Start ★★★★★");
                SettingCommand();
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private void SettingCommand()
        {
            WindowLoadedCommand = new RelayCommand(WindowLoaded);

            LogInfo("SettingCommand Done");
        }

        private void WindowLoaded(object param)
        {
            try
            {
                _main = param as MainWindow;

                if (_main != null)
                {
                    LogInfo("MainWindow Loaded");

                    Close += () =>
                    {
                        _main.Close();
                    };

                    _main.Visibility = Visibility.Hidden;
                }
                else
                {
                    LogInfo("★★ MainWindow As Null");
                    LogError("★★ MainWindow As Null");
                }

                loginWindows.Show();
                LogInfo("loginWindows Show");
            }
            catch (Exception ex)
            {

                LogException(ex.Message);
            }
        }

        private void _loginWindows_Closed(object sender, EventArgs e)
        {
            try
            {
                LogInfo("loginWindows Closed, MainWindow Closing");
                Close?.Invoke();
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }


    }
}
