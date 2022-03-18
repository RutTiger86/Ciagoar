using Ciagoar.Control.Command;
using CiagoarM.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CiagoarM
{
    public class MainWindowModel
    {
        #region Command
      
        #endregion


        LoginWindows _loginWindows;
        private LoginWindows loginWindows
        {
            get
            {
                if(_loginWindows == null)
                {
                    _loginWindows = new LoginWindows();
                }

                return _loginWindows;
            }
        }


        public MainWindowModel()
        {
            try
            {                
                loginWindows.Show();
            }
            catch (Exception e)
            {
            }
        }

    }
}
