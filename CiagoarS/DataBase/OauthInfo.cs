using System;
using System.Collections.Generic;

namespace CiagoarS.DataBase
{
    public partial class OauthInfo
    {
        public int Id { get; set; }
        public short AuthenticationType { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthUri { get; set; }
        public string TokenUri { get; set; }
        public bool IsUse { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
