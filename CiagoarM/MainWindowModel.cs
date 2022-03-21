using Ciagoar.Control.Command;
using Ciagoar.Core.Interface;
using CiagoarM.Commons;
using CiagoarM.Commons.Interface;
using CiagoarM.Views.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

namespace CiagoarM
{
    public class MainWindowModel : BaseModel
    {
        #region Command

        public RelayCommand MainClosedCommand
        {
            get;
            private set;
        }

        #endregion

        #region Variable

        LoginWindows _loginWindows;
        private LoginWindows loginWindows
        {
            get
            {
                if (_loginWindows == null)
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
                ShowLoginWindow();
                LogInfo("loginWindows Show");
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
                MainClosedCommand = new RelayCommand(CloseApplication);
                LogInfo("SettingCommand Done");
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private void ShowLoginWindow()
        {
            try
            {
                ((ILogin)loginWindows.DataContext).SuccessLogin += SuccessLogin;
                loginWindows.Show();
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private void SuccessLogin()
        {
            try
            {
                loginWindows.Visibility = Visibility.Collapsed;
                App.Current.MainWindow.Show();
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
                App.Current.MainWindow?.Close();
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private void CloseApplication(object param)
        {
            try
            {
                if(loginWindows!=null)
                {
                    loginWindows.Close();                    
                }
                App.Current.Shutdown();
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

    }
}
