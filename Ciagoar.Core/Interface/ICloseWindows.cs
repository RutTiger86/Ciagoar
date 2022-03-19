using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciagoar.Core.Interface
{
    public interface ICloseWindows
    {
        Action Close { get; set; }
    }
}
