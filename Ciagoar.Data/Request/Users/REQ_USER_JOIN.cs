using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciagoar.Data.Request.Users
{
    public class REQ_USER_JOIN
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(5)]
        [DefaultValue("en-US")]
        public string langCode { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        [EmailAddress]
        [DefaultValue("lwtlovelove@gmail.com")]
        public string email { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        [DefaultValue("TEST1")]
        public string nickname { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DefaultValue(0)]
        public short authenticationType { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(512)]
        [DefaultValue("12345678")]
        public string authenticationKey { get; set; }
    }
}
