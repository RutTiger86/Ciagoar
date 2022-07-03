using Ciagoar.Data.Request.Users;
using Ciagoar.Data.Response;
using Ciagoar.Data.Response.Users;
using CiagoarM.Commons;
using CiagoarM.Commons.Interface;
using CiagoarM.Languages;
using CiagoarM.Models;
using CommunityToolkit.Mvvm.Input;
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
            get => _email;
            set
            {
                if (Regex.IsMatch(value, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                {
                    _email = value;
                    OnPropertyChanged();
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
                    OnPropertyChanged();
                }
            }
        }

        private string _nickName;
        public string NickName
        {
            get => _nickName;
            set => SetProperty(ref _nickName, value);
        }

        private bool _isEnableControl = true;
        public bool IsEnableControl
        {

            get => _isEnableControl;
            set => SetProperty(ref _isEnableControl, value);
        }
        #endregion

        #region Command

        public RelayCommand<PasswordBox[]> JoinCommand
        {
            get;
            private set;
        }

        public RelayCommand<PasswordBox[]> CancleCommand
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
                CancleCommand = new RelayCommand<PasswordBox[]>(p=>Cancle(p));
                JoinCommand = new RelayCommand<PasswordBox[]>(p=>Join(p));
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }


        private async void Join(PasswordBox[] param)
        {
            try
            {
                if (param[0] is PasswordBox PWS1 && param[1] is PasswordBox PWS2)
                {
                    if(PWS1.Password == PWS2.Password)
                    {
                        IsEnableControl = false;

                        BaseResponse<Ci_User> JoinResult = await new UsersModel().JoinUser(Ciagoar.Data.Enums.AuthType.EM, Email, PWS1.Password, NickName);

                        if (JoinResult.Result)
                        {
                            _ = MessageBox.Show(Resource.MSG_LoginEmailCheck, Resource.Caption_Warning, MessageBoxButton.OK, MessageBoxImage.Warning);

                            //가입성공 
                            PWS1.Password = null;
                            PWS2.Password = null;
                            ReturnProcess();                            
                        }
                        else
                        {
                            LogError($"{JoinResult.ErrorCode} : {JoinResult.ErrorMessage}");
                            string messageBoxText = $"[{JoinResult.ErrorCode}]  {JoinResult.ErrorMessage}";
                            _ = MessageBox.Show(messageBoxText, Resource.Caption_Warning, MessageBoxButton.OK, MessageBoxImage.Warning);
                        }

                        IsEnableControl = true;
                    }
                    else
                    {
                        _ = MessageBox.Show(Resource.MSG_passwordMismatch, Resource.Caption_Warning, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.Yes);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        

        private void Cancle(PasswordBox[] param)
        {
            try
            {
                if (param[0] is PasswordBox PWS1 && param[1] is PasswordBox PWS2)
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
