using Ciagoar.Data.Enums;
using CiagoarM.Commons;
using CiagoarM.Commons.Messenger;
using CiagoarM.Languages;
using CiagoarM.Models;
using CiagoarM.Views.Users;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CiagoarM.ViewModels.Users
{
    public class LoginWindowsModel : BaseModel, IRecipient<CheckViewHiddenMessage>, IRecipient<JoinViewHiddenMessage>
    {
        #region Binding Value

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);  
        }

        private bool _isEnableControl = true;
        public bool IsEnableControl
        {
            get => _isEnableControl;
            set => SetProperty(ref _isEnableControl, value);
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
                OnPropertyChanged();
            }
        }

        private JoinView _JoinView { get; set; }

        private AuthenticationStepCheckView _CheckView { get; set; }
        #endregion


        #region Command
        public RelayCommand<PasswordBox> LoginCommand
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

        public RelayCommand<object[]> LoadedCommand
        {
            get;
            private set;
        }


        #endregion


        public LoginWindowsModel()
        {
            try
            {
                SettingCommand();
                SettingMessage();
                AutoLoginCheck();
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
                if (Properties.Settings.Default.IsAutoLogin && !string.IsNullOrWhiteSpace(Properties.Settings.Default.AutoLoginID))
                {
                    IsEnableControl = false;
                    asyncLogin((AuthType)Properties.Settings.Default.AutoAuthenticationType, Properties.Settings.Default.AutoLoginID, Properties.Settings.Default.AutoLoginPW);
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
                LoginCommand = new RelayCommand<PasswordBox>(p=> Login(p));
                JoinCommand = new RelayCommand(Join);
                GoogleStartCommand = new RelayCommand(GoogleStart);
                LoadedCommand = new RelayCommand<object[]>(p=>Loaded(p));
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
                WeakReferenceMessenger.Default.Register<CheckViewHiddenMessage>(this);
                WeakReferenceMessenger.Default.Register<JoinViewHiddenMessage>(this);
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private void Loaded(object[] param)
        {
            try
            {
                if (param[0] is JoinView joinView && param[1] is AuthenticationStepCheckView stepCheckView)
                {
                    _JoinView = joinView;
                    _JoinView.Visibility = Visibility.Hidden;

                    _CheckView = stepCheckView;
                    _CheckView.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }


        public void Receive(CheckViewHiddenMessage message)
        {
            try
            {
                if (message.Value)
                {
                    _CheckView.Visibility = Visibility.Hidden;
                    IsEnableControl = true;
                }
                else
                {
                    _CheckView.Visibility = Visibility.Visible;
                    IsEnableControl = false;
                }
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        public void Receive(JoinViewHiddenMessage message)
        {
            try
            {
                if (message.Value)
                {
                    _JoinView.Visibility = Visibility.Hidden;
                    IsEnableControl = true;
                }
                else
                {
                    _JoinView.Visibility = Visibility.Visible;
                    IsEnableControl = false;
                }

            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private void Login(PasswordBox param)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(param.Password))
                {
                    IsEnableControl = false;
                    asyncLogin(AuthType.EM, Email, param.Password);
                }
                else
                {
                    _ = MessageBox.Show(Resource.MSG_Input_User_Info, Resource.Caption_Warning, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.Yes);
                    IsEnableControl = true;
                }
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private async void asyncLogin(AuthType authentication, string Email = null, string AuthenticationKey = null)
        {
            try
            {
                bool success = await new UsersModel().Login(authentication, Email, AuthenticationKey);

                if (success)
                {
                    if (authentication == AuthType.EM && Localproperties.LoginUser.AuthStep != 0)
                    {
                        /// AuthenticationStep 인증  
                        _CheckView.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        if (IsAutoLogin)
                        {

                            Properties.Settings.Default.AutoAuthenticationType = (short)authentication;
                            Properties.Settings.Default.AutoLoginID = Localproperties.LoginUser.Email;
                            Properties.Settings.Default.AutoLoginPW = AuthenticationKey;
                            Properties.Settings.Default.Save();

                        }

                        IsEnableControl = true;
                        WeakReferenceMessenger.Default.Send(new LoginMessage(true));
                    }

                }
                else
                {
                    _ = MessageBox.Show(Resource.MSG_LoginFail, Resource.Caption_Warning, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.Yes);
                    IsEnableControl = true;
                }

            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }


        private void Join()
        {
            try
            {
                IsEnableControl = false;
                _JoinView.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private void GoogleStart()
        {
            try
            {
                asyncLogin(AuthType.GG);
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

    }
}
