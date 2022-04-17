using System;
using System.Collections.Generic;

namespace CiagoarS.DataBase
{
    public partial class OrderItem
    {
        public int Id { get; set; }
        public int OrderInfoId { get; set; }
        public int ItemInfoId { get; set; }
        public string OrderPrice { get; set; }
        public int PriceUnitType { get; set; }
        public DateTime OrderQty { get; set; }
        public bool Isdelete { get; set; }
        public DateTime Createtime { get; set; }

        public virtual ItemInfo ItemInfo { get; set; }
        public virtual OrderInfo OrderInfo { get; set; }
    }
}
