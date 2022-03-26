using System;
using System.Collections.Generic;

namespace CiagoarS.DataBase
{
    public partial class UserInfo
    {
        public UserInfo()
        {
            UserAuthentications = new HashSet<UserAuthentication>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public short AuthType { get; set; }
        public string Nickname { get; set; }
        public bool IsUse { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int? UpdateUserId { get; set; }

        public virtual ICollection<UserAuthentication> UserAuthentications { get; set; }
    }
}
