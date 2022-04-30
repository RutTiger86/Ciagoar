using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciagoar.Data.Request.RelativeCompanies
{
    public class REQ_RELATVE_CO_PUT
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(5)]
        [DefaultValue("en-US")]
        public string langCode { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(150)]
        public string coName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(150)]
        public string coAddress { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(150)]
        public string phoneNumber { get; set; }

        [StringLength(10)]
        public string connectUrl { get; set; }

        [StringLength(400)]
        public string memo { get; set; }
        public bool? isuse { get; set; }
    }
}
