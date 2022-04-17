using System;
using System.Collections.Generic;

namespace CiagoarS.DataBase
{
    public partial class ReturnInfo
    {
        public int Id { get; set; }
        public int ApplyUserId { get; set; }
        public DateTime OrderDate { get; set; }
        public int DeliveryInventoryId { get; set; }
        public short RetrunType { get; set; }
        public string Memo { get; set; }
        public short ReturnStep { get; set; }
        public int? MgrUserId { get; set; }
        public DateTime? ReturnDatetime { get; set; }
        public bool? Isuse { get; set; }
        public bool Isdelete { get; set; }
        public DateTime Createtime { get; set; }
        public DateTime? Updatetime { get; set; }

        public virtual UserInfo ApplyUser { get; set; }
        public virtual DeliveryInventory DeliveryInventory { get; set; }
        public virtual UserInfo MgrUser { get; set; }
    }
}
