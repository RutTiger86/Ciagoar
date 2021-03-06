using Ciagoar.Data.Enums;
using CiagoarM.Commons;
using CiagoarM.Commons.Interface;
using CiagoarM.Languages;
using CiagoarM.Models;
using CiagoarM.Views.Users;
using CommunityToolkit.Mvvm.Input;
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

        public RelayCommand<UserControl[]> LoadedCommand
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
                LoadedCommand = new RelayCommand<UserControl[]>(p=>Loaded(p));
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }

        }

        private void Loaded(UserControl[] param)
        {
            try
            {
                if (param[0] is JoinView joinView && param[1] is AuthenticationStepCheckView stepCheckView)
                {
                    _JoinView = joinView;
                    _JoinView.Visibility = Visibility.Hidden;
                    ((IReturnAction)_JoinView.DataContext).ReturnAction += JoinViewHiden;

                    _CheckView = stepCheckView;
                    _CheckView.Visibility = Visibility.Hidden;
                    ((IReturnAction)_CheckView.DataContext).ReturnAction += CheckViewHiden;
                }
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private void JoinViewHiden()
        {
            try
            {
                _JoinView.Visibility = Visibility.Hidden;
                IsEnableControl = true;
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private void CheckViewHiden()
        {
            try
            {
                _CheckView.Visibility = Visibility.Hidden;
                IsEnableControl = true;
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
                        ReturnAction?.Invoke();
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
