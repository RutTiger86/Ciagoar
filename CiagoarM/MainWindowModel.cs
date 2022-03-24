using Ciagoar.Control.Command;
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
                    _loginWindows.Closed += (s, arg) => { ProgramShutdown(null); };
                    ((ILogin)_loginWindows.DataContext).SuccessLogin += SuccessLogin;
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
                MainClosedCommand = new RelayCommand(ProgramShutdown);
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


        private void ProgramShutdown(object param)
        {
            try
            {
                LogInfo("ProgramShutDown");
                Application.Current.MainWindow?.Close();
                loginWindows?.Close();
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }



    }
}
