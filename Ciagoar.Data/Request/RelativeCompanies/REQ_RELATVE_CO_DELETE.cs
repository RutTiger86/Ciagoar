using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciagoar.Data.Request.RelativeCompanies
{
    public class REQ_RELATVE_CO_DELETE
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(5)]
        [DefaultValue("en-US")]
        public string langCode { get; set; }

        [Required(AllowEmptyStrings = false)]
        public int id { get; set; }
    }
}
