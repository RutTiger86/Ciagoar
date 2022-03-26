using System;
using System.Collections.Generic;

namespace CiagoarS.DataBase
{
    public partial class UserAuthentication
    {
        public int Id { get; set; }
        public int UserInfoId { get; set; }
        public short AuthenticationType { get; set; }
        public string AuthenticationKey { get; set; }
        public bool IsUse { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }

        public virtual UserInfo UserInfo { get; set; }
    }
}
