using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciagoar.Data.Response.Users
{
    public class Ci_User
    {
        public string Email { get; set; }
        public string Nickname { get; set; }
        public short AuthType { get; set; }

        public short AuthenticationStep { get; set; }
    }
}
