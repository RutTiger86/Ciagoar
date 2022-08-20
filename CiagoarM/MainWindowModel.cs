using CiagoarM.Commons;
using CiagoarM.Views.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using CiagoarM.Commons.Messenger;
using CommunityToolkit.Mvvm.Messaging;

namespace CiagoarM
{
    public class MainWindowModel : BaseModel,IRecipient<LoginMessage>
    {
        #region Command

        /// <summary>
        /// 메인윈도우 닫힘 커멘드
        /// </summary>
        public RelayCommand MainClosedCommand
        {
            get;
            private set;
        }


        public RelayCommand LogoutCommand
        {
            get;
            private set;
        }

        public RelayCommand<BaseView> MenuClickCommand
        {
            get;
            private set;
        }

        #endregion

        #region  Binding Value

        private string _userName = string.Empty;
        public string UserName 
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        private LoginWindows _loginWindows;
        private LoginWindows loginWindows
        {
            get
            {
                if (_loginWindows == null)
                {
                    _loginWindows = new LoginWindows();
                    _loginWindows.Closed += (s, arg) => { ProgramShutdown(); };
                }

                return _loginWindows;
            }
        }


        private object _mainView = null;

        public object MainView
        {
            get => _mainView;
            set => SetProperty(ref _mainView, value);            
        }

        #endregion
        public MainWindowModel()
        {
            try
            {
                LogInfo("★★★★★ MainWindowModel Start ★★★★★");
                SettingCommand();
                SettingMessage();
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
                LogoutCommand = new RelayCommand(Logout);
                MenuClickCommand = new RelayCommand<BaseView>(p=> MenuClick(p));
                LogInfo("SettingCommand Done");
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private void SettingMessage()
        {
            try
            {
                WeakReferenceMessenger.Default.Register<LoginMessage>(this);
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private void MenuClick(BaseView parm)
        {
            try
            {
                MainView = parm;
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private void Logout()
        {
            try
            {
                ShowLoginWindow();
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }



        public void ShowLoginWindow()
        {
            try
            {
                App.Current.MainWindow.Visibility = Visibility.Hidden;
                loginWindows.Show();
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        public void Receive(LoginMessage message)
        {
            try
            {
                UserName = Localproperties.LoginUser.Nickname;
                loginWindows.Visibility = Visibility.Collapsed;
                App.Current.MainWindow.Visibility = Visibility.Visible;
                App.Current.MainWindow.Show();
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private void ProgramShutdown()
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
