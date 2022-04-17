using System;
using System.Collections.Generic;

namespace CiagoarS.DataBase
{
    public partial class AuthInfo
    {
        public int Id { get; set; }
        public short TypeCode { get; set; }
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
