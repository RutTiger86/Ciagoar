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

namespace CiagoarM.ViewModels.Users
{
    public class LoginWindowsModel : BaseModel, ILogin
    {
        #region Binding Value

        private string _Id;
        public string ID
        {
            get { return _Id; }
            set
            {
                _Id = value;
                onPropertyChanged();
            }
        }

        private string _passWord;
        public string PassWord
        {
            get { return _passWord; }
            set
            {
                _passWord = value;
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
        public Action SuccessLogin { get; set; }

        #endregion


        public LoginWindowsModel()
        {
            try
            {
                SettingCommand();
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        public void SettingCommand()
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

        public void Login(object param)
        {
            try
            {
                asyncLogin(AuthenticationType.EM, PassWord, ID);
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        public async void asyncLogin(AuthenticationType authentication, string AuthenticationKey, string Email = null)
        {
            try
            {
                bool success = await new UserModel().Login(authentication, AuthenticationKey, Email);

                if (success)
                {
                    SuccessLogin?.Invoke();
                }
                else
                {
                    string messageBoxText = Resource.MSG_LoginFail;
                    string caption = Resource.Caption_Warning;
                    MessageBoxButton button = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Warning;

                    _ = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                }
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        public void Join(object param)
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        public void GoogleStart(object param)
        {
            try
            {
                string Resettoken = "";

                asyncLogin(AuthenticationType.GG, Resettoken, ID);

            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }



    }
}
