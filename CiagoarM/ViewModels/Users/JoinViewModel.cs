using Ciagoar.Control.Command;
using Ciagoar.Data.Request.Users;
using CiagoarM.Commons;
using CiagoarM.Commons.Interface;
using CiagoarM.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CiagoarM.ViewModels.Users
{
    public class JoinViewModel : BaseModel, IReturnAction
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

        private string _nickName;
        public string NickName
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
        #endregion

        #region Command

        public RelayCommand JoinCommand
        {
            get;
            private set;
        }

        public RelayCommand CancleCommand
        {
            get;
            private set;
        }

        public RelayCommand LoadedCommand
        {
            get;
            private set;
        }

        public Action ReturnAction { get; set; }

        #endregion

        public JoinViewModel()
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

        private void SettingCommand()
        {
            try
            {
                LoadedCommand = new RelayCommand(Loaded);
                CancleCommand = new RelayCommand(Cancle);
                JoinCommand = new RelayCommand(Join);
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private void Loaded(object param)
        {
            try
            {
                var values = (object[])param;

                if (values != null && values[0] is PasswordBox PWS1 && values[1] is PasswordBox PWS2)
                { 
                    PWS1.Password = null;
                    PWS2.Password = null;
                }

                Email = String.Empty;
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
                var values = (object[])param;

                if (values[0] is PasswordBox PWS1 && values[1] is PasswordBox PWS2)
                {
                    if(PWS1.Password == PWS2.Password)
                    {
                        IsEnableControl = false;

                    }
                    else
                    {
                        string messageBoxText = "Password 불일치";// Resource.MSG_LoginFail;
                        string caption = Resource.Caption_Warning;
                        MessageBoxButton button = MessageBoxButton.OK;
                        MessageBoxImage icon = MessageBoxImage.Warning;

                        _ = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private void Cancle(object param)
        {
            try
            {
                ReturnAction?.Invoke();
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }
    }
}
