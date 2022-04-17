using System;
using System.Collections.Generic;

namespace CiagoarS.DataBase
{
    public partial class OrderInfo
    {
        public OrderInfo()
        {
            InventoryInfos = new HashSet<InventoryInfo>();
            OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public int RelativeCoId { get; set; }
        public int UserInfoId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Memo { get; set; }
        public short OderStep { get; set; }
        public bool? Isuse { get; set; }
        public bool Isdelete { get; set; }
        public DateTime Createtime { get; set; }
        public DateTime? Updatetime { get; set; }

        public virtual RelativeCo RelativeCo { get; set; }
        public virtual UserInfo UserInfo { get; set; }
        public virtual ICollection<InventoryInfo> InventoryInfos { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
