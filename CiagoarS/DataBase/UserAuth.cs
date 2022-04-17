using System;
using System.Collections.Generic;

namespace CiagoarS.DataBase
{
    public partial class UserAuth
    {
        public int Id { get; set; }
        public int UserInfoId { get; set; }
        public short TypeCode { get; set; }
        public string AuthKey { get; set; }
        public short AuthStep { get; set; }
        public bool? Isuse { get; set; }
        public bool Isdelete { get; set; }
        public DateTime Createtime { get; set; }
        public DateTime? Updatetime { get; set; }

        public virtual UserInfo UserInfo { get; set; }
    }
}
