using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Ciagoar.Data.Enums
{
    public enum MediaType
    {
        [Description("application/xml")]
        APPLICATION_XML,
        [Description("application/json")]
        APPLICATION_JSON,
        [Description("application/x-www-form-urlencoded")]
        APPLICATION_x_WWW_FORM
    };
}
