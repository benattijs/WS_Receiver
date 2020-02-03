using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WS_Receiver.Helpers
{

    public enum ValidationErrors
    {

        [Description("Invalid Site specified.")]
        UnableToParseXML = -3,

        [Description("Invalid Site specified.")]
        InvalidSiteID = -2,

        [Description("Invalid command specified.")]
        InvalidDeclarationCommand = -1,

        [Description("The document was structured correctly.")]
        Success = 0,


    }
}