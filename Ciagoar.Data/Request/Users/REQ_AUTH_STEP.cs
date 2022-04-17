using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciagoar.Data.Request.Users
{
    public class REQ_AUTH_STEP
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(5)]
        [DefaultValue("en-US")]
        public string langCode { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        [EmailAddress]
        [DefaultValue("**@gmail.com")]
        public string email { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(6)]
        [DefaultValue("")]
        public string authStepKey { get; set; }
    }
}
