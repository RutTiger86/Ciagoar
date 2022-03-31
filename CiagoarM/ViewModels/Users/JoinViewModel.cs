using Ciagoar.Control.Command;
using Ciagoar.Data.Request.Users;
using Ciagoar.Data.Response;
using Ciagoar.Data.Response.Users;
using CiagoarM.Commons;
using CiagoarM.Commons.Interface;
using CiagoarM.Languages;
using CiagoarM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                if (Regex.IsMatch(value, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                {
                    _email = value;
                    onPropertyChanged();
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        string messageBoxText = "Email형식이 맞아야 합니다.";// Resource.MSG_LoginFail;
                        string caption = Resource.Caption_Warning;
                        MessageBoxButton button = MessageBoxButton.OK;
                        MessageBoxImage icon = MessageBoxImage.Warning;

                        _ = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                    }
                    _email = String.Empty;
                    onPropertyChanged();
                }
            }
        }

        private string _nickName;
        public string NickName
        {
            get { return _nickName; }
            set
            {
                _nickName = value;
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
                CancleCommand = new RelayCommand(Cancle);
                JoinCommand = new RelayCommand(Join);
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }


        private async void Join(object param)
        {
            try
            {
                var values = (object[])param;

                if (values[0] is PasswordBox PWS1 && values[1] is PasswordBox PWS2)
                {
                    if(PWS1.Password == PWS2.Password)
                    {
                        IsEnableControl = false;

                        BaseResponse<Ci_User> JoinResult = await new UserModel().JoinUser(Ciagoar.Data.Enums.AuthenticationType.EM, Email, PWS1.Password, NickName);

                        if (JoinResult.Result)
                        {
                            //가입성공 
                            PWS1.Password = null;
                            PWS2.Password = null;
                            ReturnProcess();                            
                        }
                        else
                        {
                            LogError($"{JoinResult.ErrorCode} : {JoinResult.ErrorMessage}");
                            string messageBoxText = $"[{JoinResult.ErrorCode}]  {JoinResult.ErrorMessage}";// Resource.MSG_LoginFail;
                            string caption = Resource.Caption_Warning;
                            MessageBoxButton button = MessageBoxButton.OK;
                            MessageBoxImage icon = MessageBoxImage.Warning;

                            _ = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                        }

                        IsEnableControl = true;
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
                var values = (object[])param;

                if (values[0] is PasswordBox PWS1 && values[1] is PasswordBox PWS2)
                {
                    PWS1.Password = null;
                    PWS2.Password = null;
                }

                ReturnProcess();
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private void ReturnProcess()
        {
            try
            {
                Email = String.Empty;
                NickName = String.Empty;
                IsEnableControl = true;

                ReturnAction?.Invoke();

            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }
    }
}
