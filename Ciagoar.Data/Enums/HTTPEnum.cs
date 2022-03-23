using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Ciagoar.Data.Enums
{
    public enum HEADER_METHOD
    {
        [Description("GET")]
        HTTP_GET,
        [Description("POST")]
        HTTP_POST,
        [Description("PUT")]
        HTTP_PUT,
        [Description("DELETE")]
        HTTP_DELETE
    };
    public enum HEADER_ACCEPT
    {
        [Description("application/xml")]
        RESPONSE_XML,
        [Description("application/json")]
        RESPONSE_JSON
    };
}
