using System;
using System.Collections.Generic;

namespace CiagoarS.DataBase
{
    public partial class UserInfo
    {
        public UserInfo()
        {
            DeliveryInfoApplyUsers = new HashSet<DeliveryInfo>();
            DeliveryInfoMgrUsers = new HashSet<DeliveryInfo>();
            OrderInfos = new HashSet<OrderInfo>();
            ReturnInfoApplyUsers = new HashSet<ReturnInfo>();
            ReturnInfoMgrUsers = new HashSet<ReturnInfo>();
            UserAuths = new HashSet<UserAuth>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public short TypeCode { get; set; }
        public string Nickname { get; set; }
        public bool? Isuse { get; set; }
        public bool Isdelete { get; set; }
        public DateTime Createtime { get; set; }
        public DateTime? Updatetime { get; set; }

        public virtual ICollection<DeliveryInfo> DeliveryInfoApplyUsers { get; set; }
        public virtual ICollection<DeliveryInfo> DeliveryInfoMgrUsers { get; set; }
        public virtual ICollection<OrderInfo> OrderInfos { get; set; }
        public virtual ICollection<ReturnInfo> ReturnInfoApplyUsers { get; set; }
        public virtual ICollection<ReturnInfo> ReturnInfoMgrUsers { get; set; }
        public virtual ICollection<UserAuth> UserAuths { get; set; }
    }
}
