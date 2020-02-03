using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using WS_Receiver.Helpers;

namespace WS_Receiver
{
    /// <summary>
    /// Summary description for Declarations
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Declarations : System.Web.Services.WebService
    {
        List<ValidationErrors?> responseCode = new List<ValidationErrors?>();

        [WebMethod]
        public string ReceiveDeclarations(string inputDocument)
        {

            if (string.IsNullOrEmpty(inputDocument))
            {
                responseCode.Add(ValidationErrors.UnableToParseXML);
            }
            else
            {
                inputDocument = HttpUtility.HtmlDecode(inputDocument);
                XDocument document = XDocument.Parse(inputDocument);

                ValidateInputDocument(document.Element("InputDocument"));

            }

            /*
             <Response>
                <ResponseStatus>-1<ResponseStatus>
	            <ErrorList>
                    <ErrorItem>
                        <ErrorCode>-1</ErrorCode>
                    </ErrorItem>
                    <ErrorItem>
                        <ErrorCode>-2</ErrorCode>
                    </ErrorItem>
                </ErrorList>   
            </Response>
             */

            XDocument response = new XDocument(
                new XElement("Response",
                    new XElement("ResponseStatus", responseCode.Any() ? (int)responseCode.FirstOrDefault() : (int)ValidationErrors.Success),
                    new XElement("ErrorList",
                    responseCode.Select(x =>
                        new XElement("ErrorItem",
                            new XElement("ErrorCode", (int)x)
                        )
                    )
                    )
                )
            );
            return response.ToString();

        }

        #region Helpers
        private void ValidateInputDocument(XElement inputElement)
        {
            List<XElement> xElementList = inputElement.Elements().ToList();
            foreach (XElement elem in xElementList)
            {
                switch (elem.Name.LocalName)
                {
                    case "Declaration":
                        if (elem.Attribute("Command").Value != "DEFAULT")
                        {
                            responseCode.Add(ValidationErrors.InvalidDeclarationCommand);
                        }
                        break;
                    case "SiteID":
                        if (elem.Value != "DUB")
                        {
                            responseCode.Add(ValidationErrors.InvalidSiteID);
                        }
                        break;
                }
                if (elem.HasElements)
                {
                    ValidateInputDocument(elem);//Recursive to list through all the chield elements to validate.
                }
            }

        }
        #endregion
    }
}
