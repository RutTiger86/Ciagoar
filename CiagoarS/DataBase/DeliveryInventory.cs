using System;
using System.Collections.Generic;

namespace CiagoarS.DataBase
{
    public partial class DeliveryInventory
    {
        public DeliveryInventory()
        {
            ReturnInfos = new HashSet<ReturnInfo>();
        }

        public int Id { get; set; }
        public int DeliveryItemId { get; set; }
        public int InveontoryStorageId { get; set; }
        public bool Isdelete { get; set; }
        public DateTime Createtime { get; set; }

        public virtual DeliveryItem DeliveryItem { get; set; }
        public virtual InventoryStorage InveontoryStorage { get; set; }
        public virtual ICollection<ReturnInfo> ReturnInfos { get; set; }
    }
}
