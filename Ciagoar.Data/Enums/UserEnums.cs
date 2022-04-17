using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Ciagoar.Data.Enums
{
    public enum AuthType
    {

        [Description("Email")]
        EM =0,
        [Description("Google")]
        GG =1
    }

    public enum UserType
    {

        [Description("Admin")]
        Admin = 0,
        [Description("User")]
        User = 1
    }
}
