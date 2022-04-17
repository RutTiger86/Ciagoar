using System;
using System.Collections.Generic;

namespace CiagoarS.DataBase
{
    public partial class StorageInfo
    {
        public StorageInfo()
        {
            InventoryStorages = new HashSet<InventoryStorage>();
        }

        public int Id { get; set; }
        public string StorageName { get; set; }
        public string StorageCode { get; set; }
        public string StorageLocation { get; set; }
        public string Memo { get; set; }
        public bool? Isuse { get; set; }
        public bool Isdelete { get; set; }
        public DateTime Createtime { get; set; }
        public DateTime Updatetime { get; set; }

        public virtual ICollection<InventoryStorage> InventoryStorages { get; set; }
    }
}
