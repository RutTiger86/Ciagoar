using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciagoar.Data.Request.User
{
    public class REQ_USER_JOIN
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(5)]
        [DefaultValue("en-US")]
        public string langCode { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(2)]
        [DefaultValue("EM")]
        public string authenticationType { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        [EmailAddress]
        [DefaultValue("**@google.com")]
        public string email { get; set; }


        [Required(AllowEmptyStrings = false)]
        [StringLength(512)]
        [DefaultValue("")]
        public string authenticationKey { get; set; }

    }
}
