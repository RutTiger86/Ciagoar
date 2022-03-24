using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciagoar.Data.HTTPS
{
    public class Error
    {
        public string type { get; set; }

        public string title { get; set; }

        public int status { get; set; }

        public string traceId { get; set; }

        public object errors { get; set; }
    }
}
