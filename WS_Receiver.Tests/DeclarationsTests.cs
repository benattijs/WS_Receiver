using System;
using System.Collections.Generic;
using System.Web;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WS_Receiver.Tests
{
    [TestClass]
    public class DeclarationsTests
    {
        [TestMethod]
        public void PostWithSuccess()
        {
            // Arrange
            Declarations controller = new Declarations();
            string xmlInput = @"<InputDocument>
	                                <DeclarationList>
		                                <Declaration Command=""DEFAULT"" Version=""5.13"">
			                                <DeclarationHeader>
				                                <Jurisdiction>IE</Jurisdiction>
				                                <CWProcedure>IMPORT</CWProcedure>
				                                <DeclarationDestination>CUSTOMSWAREIE</DeclarationDestination>
				                                <DocumentRef>71Q0019681</DocumentRef>
				                                <SiteID>DUB</SiteID>
				                                <AccountCode>G0779837</AccountCode>
			                                </DeclarationHeader>
		                                </Declaration>
	                                </DeclarationList>
                                </InputDocument>";


            xmlInput = HttpUtility.HtmlDecode(xmlInput);

            // Act
            var response = controller.ReceiveDeclarations(xmlInput);


            // Assert
            XDocument parsedResponse = XDocument.Parse(response);

            Assert.AreEqual("0", parsedResponse.Element("Response").Element("ResponseStatus").Value);

            IList<string> expectedResponse = new List<string>() { "0" };

            TestResponse(expectedResponse, parsedResponse);

        }
        [TestMethod]
        public void PostWithDeclarationError()
        {
            // Arrange
            Declarations controller = new Declarations();
            string xmlInput = @"<InputDocument>
	                                <DeclarationList>
		                                <Declaration Command=""CUSTOM"" Version=""5.13"">
			                                <DeclarationHeader>
				                                <Jurisdiction>IE</Jurisdiction>
				                                <CWProcedure>IMPORT</CWProcedure>
				                                <DeclarationDestination>CUSTOMSWAREIE</DeclarationDestination>
				                                <DocumentRef>71Q0019681</DocumentRef>
				                                <SiteID>DUB</SiteID>
				                                <AccountCode>G0779837</AccountCode>
			                                </DeclarationHeader>
		                                </Declaration>
	                                </DeclarationList>
                                </InputDocument>";

            xmlInput = HttpUtility.HtmlDecode(xmlInput);

            // Act
            var response = controller.ReceiveDeclarations(xmlInput);


            // Assert
            XDocument parsedResponse = XDocument.Parse(response);

            IList<string> expectedResponse = new List<string>() { "-1" };

            TestResponse(expectedResponse, parsedResponse);

        }
        [TestMethod]
        public void PostWithSiterIDError()
        {
            // Arrange
            Declarations controller = new Declarations();
            string xmlInput = @"<InputDocument>
	                                <DeclarationList>
		                                <Declaration Command=""DEFAULT"" Version=""5.13"">
			                                <DeclarationHeader>
				                                <Jurisdiction>IE</Jurisdiction>
				                                <CWProcedure>IMPORT</CWProcedure>
				                                <DeclarationDestination>CUSTOMSWAREIE</DeclarationDestination>
				                                <DocumentRef>71Q0019681</DocumentRef>
				                                <SiteID>COR</SiteID>
				                                <AccountCode>G0779837</AccountCode>
			                                </DeclarationHeader>
		                                </Declaration>
	                                </DeclarationList>
                                </InputDocument>";

            xmlInput = HttpUtility.HtmlDecode(xmlInput);

            // Act
            var response = controller.ReceiveDeclarations(xmlInput);

            // Assert
            XDocument parsedResponse = XDocument.Parse(response);


            IList<string> expectedResponse = new List<string>() { "-2" };

            TestResponse(expectedResponse, parsedResponse);


        }
        [TestMethod]
        public void PostWithMultipleErrors()
        {
            // Arrange
            Declarations controller = new Declarations();
            string xmlInput = @"<InputDocument>
	                                <DeclarationList>
		                                <Declaration Command=""CUSTOM"" Version=""5.13"">
			                                <DeclarationHeader>
				                                <Jurisdiction>IE</Jurisdiction>
				                                <CWProcedure>IMPORT</CWProcedure>
				                                <DeclarationDestination>CUSTOMSWAREIE</DeclarationDestination>
				                                <DocumentRef>71Q0019681</DocumentRef>
				                                <SiteID>COR</SiteID>
				                                <AccountCode>G0779837</AccountCode>
			                                </DeclarationHeader>
		                                </Declaration>
	                                </DeclarationList>
                                </InputDocument>";

            xmlInput = HttpUtility.HtmlDecode(xmlInput);

            // Act
            var response = controller.ReceiveDeclarations(xmlInput);


            // Assert
            XDocument parsedResponse = XDocument.Parse(response);
            IList<string> expectedResponse = new List<string>() { "-1", "-2" };

            TestResponse(expectedResponse, parsedResponse);



        }


        public void TestResponse(IList<string> expectedResponseCodes, XDocument parsedResponse)
        {

            //Check the first expectedResponseCode to match the ResponseStatus
            Assert.AreEqual(expectedResponseCodes[0], parsedResponse.Element("Response").Element("ResponseStatus").Value);

            int totalElementsReturned = 0;
            foreach (XElement el in parsedResponse.Element("Response").Element("ErrorList")?.Elements("ErrorItem"))
            {
                Assert.IsTrue(expectedResponseCodes.Contains(el.Element("ErrorCode").Value));
                totalElementsReturned++;
            }

            //Check if the correct amount of elements is returned.
            Assert.AreEqual((expectedResponseCodes[0] == "0") ? expectedResponseCodes[0] : expectedResponseCodes.Count.ToString(), totalElementsReturned.ToString());

        }
    }
}
