# WS Receiver

## Background:
Write a webservice that accepts the following XML document as the payload:

```
<InputDocument>
	<DeclarationList>
		<Declaration Command="DEFAULT" Version="5.13">
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
</InputDocument>
```

The webservice should respond with a status code which is based on parsing the contents of the XML payload

a.	If the XML document is given here is passed then return a status of ‘0’ – which means the document was structured correctly.
b.	If the Declararation’s Command <> ‘DEFAULT’ then return ‘-1’ – which means invalid command specified.
c.	If the SiteID <> ‘DUB’ then return ‘-2’ – invalid Site specified.


### Input:
The input expected for the Declaration Endpoint will be a string containing a XML.

```
<InputDocument>
	<DeclarationList>
		<Declaration Command="DEFAULT" Version="5.13">
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
</InputDocument>
```

### Output:
The output is a string containtin a XML with Response Status accordingly to the valdiations.
```
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
```

### SoapUI Project Exported
Link for the exported SoapUI project XML: [SOAP UI Declarations Project](Declarations-soapui-project.xml)

## Code documentation
Project is divided and 2 projects:

### 1. WS_Receiver
This is the Main Web Applcation to host the Endpoints.

#### WS_Receiver/Declarations.asmx
It's the endpoit that contains the logic that receive the XML Input and perform the appropriate validations before returning the response. 

#### WS_Receiver/Helpers/ValidationErrors.cs
Containts the list of possible erros to be returned.


### 2. WS_Receiver.Tests
Contains the Unit Tests created to validate the endpoits. 

#### WS_Receiver.Tests/DeclarationTests.cs 
Contains the test cases for the Declarations Endpoint.
