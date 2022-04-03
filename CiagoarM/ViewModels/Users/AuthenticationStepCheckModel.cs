using Ciagoar.Control.Command;
using Ciagoar.Data.Response;
using Ciagoar.Data.Response.Users;
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
    public class AuthenticationStepCheckModel : BaseModel, IReturnAction
    {

        #region Bindeing Value
        public string _checkNum_1 = String.Empty;
        public string CheckNum_1
        {
            get { return _checkNum_1; }
            set
            {
                _checkNum_1 = value;
                onPropertyChanged();
            }
        }

        public string _checkNum_2 = String.Empty;
        public string CheckNum_2
        {
            get { return _checkNum_2; }
            set
            {
                _checkNum_2 = value;
                onPropertyChanged();
            }
        }

        public string _checkNum_3 = String.Empty;
        public string CheckNum_3
        {
            get { return _checkNum_3; }
            set
            {
                _checkNum_3 = value;
                onPropertyChanged();
            }
        }

        public string _checkNum_4 = String.Empty;
        public string CheckNum_4
        {
            get { return _checkNum_4; }
            set
            {
                _checkNum_4 = value;
                onPropertyChanged();
            }
        }

        public string _checkNum_5 = String.Empty;
        public string CheckNum_5
        {
            get { return _checkNum_5; }
            set
            {
                _checkNum_5 = value;
                onPropertyChanged();
            }
        }

        public string _checkNum_6 = String.Empty;
        public string CheckNum_6
        {
            get { return _checkNum_6; }
            set
            {
                _checkNum_6 = value;
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

        #region command
        public RelayCommand CheckCommand
        {
            get;
            private set;
        }

        public RelayCommand CancleCommand
        {
            get;
            private set;
        }

        #endregion


        public Action ReturnAction { get; set; }


        public AuthenticationStepCheckModel()
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
                CheckCommand = new RelayCommand(Check);
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }

        private async void Check(object param)
        {
            try
            {
                string CheckNum_Key = $"{CheckNum_1}{CheckNum_2}{CheckNum_3}{CheckNum_4}{CheckNum_5}{CheckNum_6}";

                if (CheckNum_Key.Trim().Length==6)
                {
                    IsEnableControl = false;

                    BaseResponse<bool> JoinResult = await new UserModel().AuthenticationStepCheck(Localproperties.LoginUser.Email, CheckNum_Key);

                    if (JoinResult.Result)
                    {
                        string messageBoxText = $"모든 인증이 완료 되었습니다. {Environment.NewLine} 재 로그인을 부탁드립니다.";// Resource.MSG_LoginFail;
                        string caption = Resource.Caption_Warning;
                        MessageBoxButton button = MessageBoxButton.OK;
                        MessageBoxImage icon = MessageBoxImage.Warning;

                        _ = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

                        ReturnProcess();
                    }
                    else
                    {
                        LogError($"{JoinResult.ErrorCode} : {JoinResult.ErrorMessage}");
                        string messageBoxText = $"인증이 실패 되었습니다. {Environment.NewLine} 인증Key를 확인해주세요.";// Resource.MSG_LoginFail;
                        string caption = Resource.Caption_Warning;
                        MessageBoxButton button = MessageBoxButton.OK;
                        MessageBoxImage icon = MessageBoxImage.Warning;

                        _ = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                    }

                    IsEnableControl = true;
                }
                else
                {
                    string messageBoxText = "Key입력이 누락되었습니다.";// Resource.MSG_LoginFail;
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



        private void Cancle(object param)
        {
            try
            {
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
                CheckNum_1 = String.Empty;
                CheckNum_2 = String.Empty;
                CheckNum_3 = String.Empty;
                CheckNum_4 = String.Empty;
                CheckNum_5 = String.Empty;
                CheckNum_6 = String.Empty;

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
