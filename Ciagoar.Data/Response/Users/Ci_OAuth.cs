using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciagoar.Data.Response.Users
{
    public class Ci_OAuth
    {
        public int AuthenticationType { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthUri { get; set; }
        public string TokenUri { get; set; }
    }
}
