using Ciagoar.Data.Response;
using Ciagoar.Data.Response.Users;
using CiagoarM.Commons;
using CiagoarM.Commons.Messenger;
using CiagoarM.Languages;
using CiagoarM.Models;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CiagoarM.ViewModels.Users
{
    public class AuthenticationStepCheckModel : BaseModel
    {

        #region Bindeing Value
        public string _checkNum_1 = String.Empty;
        public string CheckNum_1
        {
            get => _checkNum_1;
            set => SetProperty(ref _checkNum_1, value);
        }

        public string _checkNum_2 = String.Empty;
        public string CheckNum_2
        {
            get => _checkNum_2;
            set => SetProperty(ref _checkNum_2, value);
        }

        public string _checkNum_3 = String.Empty;
        public string CheckNum_3
        {
            get => _checkNum_3;
            set => SetProperty(ref _checkNum_3, value);
        }

        public string _checkNum_4 = String.Empty;
        public string CheckNum_4
        {
            get => _checkNum_4;
            set => SetProperty(ref _checkNum_4, value);
        }

        public string _checkNum_5 = String.Empty;
        public string CheckNum_5
        {
            get => _checkNum_5;
            set => SetProperty(ref _checkNum_5, value);
        }

        public string _checkNum_6 = String.Empty;
        public string CheckNum_6
        {
            get => _checkNum_6;
            set => SetProperty(ref _checkNum_6, value);
        }

        private bool _isEnableControl = true;
        public bool IsEnableControl
        {
            get => _isEnableControl;
            set => SetProperty(ref _isEnableControl, value);
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

        private async void Check()
        {
            try
            {
                string CheckNum_Key = $"{CheckNum_1}{CheckNum_2}{CheckNum_3}{CheckNum_4}{CheckNum_5}{CheckNum_6}";

                if (CheckNum_Key.Trim().Length==6)
                {
                    IsEnableControl = false;

                    BaseResponse<bool> JoinResult = await new UsersModel().AuthenticationStepCheck(Localproperties.LoginUser.Email, CheckNum_Key);

                    if (JoinResult.Result)
                    {
                        _ = MessageBox.Show(Resource.MSG_AuthKeyCheckComplete, Resource.Caption_Warning, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.Yes);
                        ReturnProcess();
                    }
                    else
                    {
                        LogError($"{JoinResult.ErrorCode} : {JoinResult.ErrorMessage}");
                        _ = MessageBox.Show(Resource.MSG_AuthKeyCheckFail, Resource.Caption_Warning, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.Yes);
                    }

                    IsEnableControl = true;
                }
                else
                {
                    _ = MessageBox.Show(Resource.MSG_KeyInputMiss, Resource.Caption_Warning, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.Yes);
                }
            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }



        private void Cancle()
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


                WeakReferenceMessenger.Default.Send(new CheckViewHiddenMessage(true));

            }
            catch (Exception ex)
            {
                LogException(ex.Message);
            }
        }
    }
}
