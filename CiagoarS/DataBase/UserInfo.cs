using System;
using System.Collections.Generic;

namespace CiagoarS.DataBase
{
    public partial class UserInfo
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
        public short AuthenticationType { get; set; }
        public string AuthenticationKey { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public short AuthType { get; set; }
    }
}
