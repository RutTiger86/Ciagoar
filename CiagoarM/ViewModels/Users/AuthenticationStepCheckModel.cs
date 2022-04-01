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
    }
}
