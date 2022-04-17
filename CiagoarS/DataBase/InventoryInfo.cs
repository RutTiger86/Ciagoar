using System;
using System.Collections.Generic;

namespace CiagoarS.DataBase
{
    public partial class InventoryInfo
    {
        public InventoryInfo()
        {
            InventoryStorages = new HashSet<InventoryStorage>();
        }

        public int Id { get; set; }
        public int ItemInfoId { get; set; }
        public int OrderInfoId { get; set; }
        public DateTime ReceiptDate { get; set; }
        public DateTime? MfgDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Memo { get; set; }
        public bool? Isuse { get; set; }
        public bool Isdelete { get; set; }
        public DateTime Createtime { get; set; }
        public DateTime? Updatetime { get; set; }

        public virtual ItemInfo ItemInfo { get; set; }
        public virtual OrderInfo OrderInfo { get; set; }
        public virtual ICollection<InventoryStorage> InventoryStorages { get; set; }
    }
}
