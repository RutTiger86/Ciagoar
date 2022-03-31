using Ciagoar.Control.Command;
using Ciagoar.Data.Enums;
using CiagoarM.Commons;
using CiagoarM.Commons.Interface;
using CiagoarM.Languages;
using CiagoarM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CiagoarM.ViewModels.Users
{
    public class LoginWindowsModel : BaseModel, IReturnAction
    {
        #region Binding Value

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                onPropertyChanged();
            }
        }

        private bool _isEnableControl = true;
        public bool IsEnableControl
        {
            get { return _isEnableControl; }
            set
            {
                _isEnableControl = value;
                onPropertyChanged();
            }
        }

        private bool _isAutoLogin = Properties.Settings.Default.IsAutoLogin;
        public bool IsAutoLogin
        {
            get { return _isAutoLogin; }
            set
            {
                _isAutoLogin = value;
                Properties.Settings.Default.IsAutoLogin = value;
                Properties.Settings.Default.Save();
                onPropertyChanged();
            }
        }

        private Visibility _isJoinViewVisible = Visibility.Hidden;
        public Visibility IsJoinViewVisible
        {
            get { return _isJoinViewVisible; }
            set
            {
                _isJoinViewVisible = value;
                onPropertyChanged();
            }
        }
        #endregion


        #region Command
        public RelayCommand LoginCommand
        {
            get;
            private set;
        }

        public RelayCommand JoinCommand
        {
            get;
            private set;
        }

        public RelayCommand GoogleStartCommand
        {
            get;
            private set;
        }
        public Action ReturnAction { get; set; }

        #endregion


        public LoginWindowsModel()
        {
            try
            {
                SettingCommand();
                //AutoLoginCheck();

                IsJoinViewVisible = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private void AutoLoginCheck()
        {
            try
            {
                if(Properties.Settings.Default.IsAutoLogin && !string.IsNullOrWhiteSpace(Properties.Settings.Default.AutoLoginID))
                {
                    switch(Properties.Settings.Default.AutoAuthenticationType)
                    {
                        case (int)AuthenticationType.EM: asyncLogin(AuthenticationType.EM, Properties.Settings.Default.AutoLoginID, Properties.Settings.Default.AutoLoginPW); break;
                        case (int)AuthenticationType.GG: asyncLogin(AuthenticationType.GG, Properties.Settings.Default.AutoLoginID); break;
                    }
                }

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
                LoginCommand = new RelayCommand(Login);
                JoinCommand = new RelayCommand(Join);
                GoogleStartCommand = new RelayCommand(GoogleStart);
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }

        }

        private void Login(object param)
        {
            try
            {
                if (param is PasswordBox)
                {
                    PasswordBox passwordBox = (PasswordBox)param;
                    IsEnableControl = false;
                    asyncLogin(AuthenticationType.EM, Email, passwordBox.Password);
                }
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private async void asyncLogin(AuthenticationType authentication, string Email = null, string AuthenticationKey = null)
        {
            try
            {
                bool success = await new UserModel().Login(authentication, Email, AuthenticationKey);

                if (success)
                {

                    if(IsAutoLogin)
                    {
                       
                        Properties.Settings.Default.AutoAuthenticationType = (short)authentication;
                        Properties.Settings.Default.AutoLoginID = Localproperties.LoginUser.Email;
                        Properties.Settings.Default.AutoLoginPW = AuthenticationKey;
                        Properties.Settings.Default.Save();

                    }

                    ReturnAction?.Invoke();
                }
                else
                {
                    string messageBoxText = Resource.MSG_LoginFail;
                    string caption = Resource.Caption_Warning;
                    MessageBoxButton button = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Warning;

                    _ = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                }
                IsEnableControl = true;
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private void Join(object param)
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private void GoogleStart(object param)
        {
            try
            {
                asyncLogin(AuthenticationType.GG);

            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }



    }
}
