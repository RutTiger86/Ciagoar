using System;
using System.Collections.Generic;

namespace CiagoarS.DataBase
{
    public partial class RelativeCoStaff
    {
        public int Id { get; set; }
        public int RelativeCoId { get; set; }
        public string StaffName { get; set; }
        public string PhoneNumber { get; set; }
        public string StaffRank { get; set; }
        public string Memo { get; set; }
        public bool? Isuse { get; set; }
        public bool Isdelete { get; set; }
        public DateTime Createtime { get; set; }
        public DateTime? Updatetime { get; set; }

        public virtual RelativeCo RelativeCo { get; set; }
    }
}
