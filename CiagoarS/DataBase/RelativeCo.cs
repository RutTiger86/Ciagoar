using System;
using System.Collections.Generic;

namespace CiagoarS.DataBase
{
    public partial class RelativeCo
    {
        public RelativeCo()
        {
            DeliveryInfos = new HashSet<DeliveryInfo>();
            ItemInfos = new HashSet<ItemInfo>();
            OrderInfos = new HashSet<OrderInfo>();
            RelativeCoStaffs = new HashSet<RelativeCoStaff>();
        }

        public int Id { get; set; }
        public string CoName { get; set; }
        public string CoAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string ConnectUrl { get; set; }
        public string Memo { get; set; }
        public bool? Isuse { get; set; }
        public bool Isdelete { get; set; }
        public DateTime Createtime { get; set; }
        public DateTime? Updatetime { get; set; }

        public virtual ICollection<DeliveryInfo> DeliveryInfos { get; set; }
        public virtual ICollection<ItemInfo> ItemInfos { get; set; }
        public virtual ICollection<OrderInfo> OrderInfos { get; set; }
        public virtual ICollection<RelativeCoStaff> RelativeCoStaffs { get; set; }
    }
}
