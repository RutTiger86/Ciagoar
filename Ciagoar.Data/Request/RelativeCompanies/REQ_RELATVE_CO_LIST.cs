using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciagoar.Data.Request.RelativeCompanies
{
    public class REQ_RELATVE_CO_LIST
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(5)]
        [DefaultValue("en-US")]
        public string langCode { get; set; }
               
        [DefaultValue("id")]
        public string sortOrder { get; set; }

        [DefaultValue("")]
        public string searchString { get; set; }

        [DefaultValue(10)]
        public int pageCount { get; set; }

        [DefaultValue(1)]
        public int pageIndex { get; set; }
    }
}
