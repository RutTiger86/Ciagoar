using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciagoar.Data.Response.RelativecCompanies
{
    public class Ci_RELATVE_CO
    {
        public int Id { get; set; }
        public string CoName { get; set; }
        public string CoAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string ConnectUrl { get; set; }
        public string Memo { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
