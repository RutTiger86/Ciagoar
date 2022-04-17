using System;
using System.Collections.Generic;

namespace CiagoarS.DataBase
{
    public partial class ItemInfo
    {
        public ItemInfo()
        {
            DeliveryItems = new HashSet<DeliveryItem>();
            InventoryInfos = new HashSet<InventoryInfo>();
            OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public string ItemName { get; set; }
        public string ItemNameEng { get; set; }
        public string ItemCode { get; set; }
        public int RelativeCoId { get; set; }
        public string Unit { get; set; }
        public string ModelCode { get; set; }
        public int? BasePrice { get; set; }
        public int? PriceUnitType { get; set; }
        public bool? Isuse { get; set; }
        public bool Isdelete { get; set; }
        public DateTime Createtime { get; set; }
        public DateTime? Updatetime { get; set; }

        public virtual RelativeCo RelativeCo { get; set; }
        public virtual ICollection<DeliveryItem> DeliveryItems { get; set; }
        public virtual ICollection<InventoryInfo> InventoryInfos { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
