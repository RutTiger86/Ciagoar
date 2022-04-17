using System;
using System.Collections.Generic;

namespace CiagoarS.DataBase
{
    public partial class InventoryStorage
    {
        public InventoryStorage()
        {
            DeliveryInventories = new HashSet<DeliveryInventory>();
        }

        public int Id { get; set; }
        public int InventoryInfoId { get; set; }
        public int StorageInfoId { get; set; }
        public string SerialNo { get; set; }
        public bool? Isuse { get; set; }
        public bool Isdelete { get; set; }
        public DateTime Createtime { get; set; }
        public DateTime? Updatetime { get; set; }

        public virtual InventoryInfo InventoryInfo { get; set; }
        public virtual StorageInfo StorageInfo { get; set; }
        public virtual ICollection<DeliveryInventory> DeliveryInventories { get; set; }
    }
}
