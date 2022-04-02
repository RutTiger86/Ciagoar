using CiagoarM.Commons;
using CiagoarM.Commons.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiagoarM.ViewModels.Users
{
    public class AuthenticationStepCheckModel : BaseModel, IReturnAction
    {
        public Action ReturnAction { get; set; }

        public string _checkNum_1 = "0";
        public string CheckNum_1
        {
            get { return _checkNum_1; }
            set
            {
                _checkNum_1 = value;
                onPropertyChanged();
            }
        }

        public string _checkNum_2 = "0";
        public string CheckNum_2
        {
            get { return _checkNum_2; }
            set
            {
                _checkNum_2 = value;
                onPropertyChanged();
            }
        }

        public string _checkNum_3 = "0";
        public string CheckNum_3
        {
            get { return _checkNum_3; }
            set
            {
                _checkNum_3 = value;
                onPropertyChanged();
            }
        }

        public string _checkNum_4 = "0";
        public string CheckNum_4
        {
            get { return _checkNum_4; }
            set
            {
                _checkNum_4 = value;
                onPropertyChanged();
            }
        }

        public string _checkNum_5 = "0";
        public string CheckNum_5
        {
            get { return _checkNum_5; }
            set
            {
                _checkNum_5 = value;
                onPropertyChanged();
            }
        }

        public string _checkNum_6 = "0";
        public string CheckNum_6
        {
            get { return _checkNum_6; }
            set
            {
                _checkNum_6 = value;
                onPropertyChanged();
            }
        }
    }
}
