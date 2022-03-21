using Ciagoar.Control.Command;
using Ciagoar.Data.Enums;
using CiagoarM.Commons;
using CiagoarM.Commons.Interface;
using CiagoarM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                if (new UserModel().Login(AuthenticationType.EM, PassWord, ID))
                {
                    SuccessLogin?.Invoke();
                }
                else
                {
                    //FailLogin
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
                if (new UserModel().Login(AuthenticationType.GG, Resettoken))
                {

                }
                else
                {
                    //FailLogin
                }
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }



    }
}
