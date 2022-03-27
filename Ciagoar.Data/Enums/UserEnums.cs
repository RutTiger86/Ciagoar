using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Ciagoar.Data.Enums
{
    public enum AuthenticationType
    {

        [Description("Email")]
        EM,
        [Description("Google")]
        GG
    }

    public enum AuthType
    {

        [Description("Admin")]
        Admin,
        [Description("User")]
        User
    }
}
