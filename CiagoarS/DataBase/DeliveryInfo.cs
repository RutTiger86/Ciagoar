using System;
using System.Collections.Generic;

namespace CiagoarS.DataBase
{
    public partial class DeliveryInfo
    {
        public DeliveryInfo()
        {
            DeliveryItems = new HashSet<DeliveryItem>();
        }

        public int Id { get; set; }
        public int RelativeCoId { get; set; }
        public int ApplyUserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Memo { get; set; }
        public short OderStep { get; set; }
        public int? MgrUserId { get; set; }
        public DateTime? DeliveryDatetime { get; set; }
        public bool? Isuse { get; set; }
        public bool Isdelete { get; set; }
        public DateTime Createtime { get; set; }
        public DateTime? Updatetime { get; set; }

        public virtual UserInfo ApplyUser { get; set; }
        public virtual UserInfo MgrUser { get; set; }
        public virtual RelativeCo RelativeCo { get; set; }
        public virtual ICollection<DeliveryItem> DeliveryItems { get; set; }
    }
}
