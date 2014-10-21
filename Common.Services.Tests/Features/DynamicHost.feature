Feature: DynamicHost

@wsdl @proxy @xml @svcutil
Scenario: Dynamic Proxy WSDL
	Given set serialization method to 'Xml'
	And set generated assembly namespace to "AAMVA.Avattar.Contracts"
	And set rehost namespace to 'AAMVA.Avattar.DynamicServices'
	And wsdl url 'http://localhost:2526/VehicleSystems/NMVTIS/PerformVehicleHistoryInquiry/Request?wsdl'
	When I generate wsdl assembly and save them to folder 'wsdl-generated' with name 'NMVTIS.NM04.EWS.Request.dll' using SvcUtil
	And I should discover 1 interface with name 'INmvtisPerformVehicleHistoryInquiry'
	And there should be a method with name 'SubmitRequest'
	And I rehost services using generated wsdl assembly
	| Field                | Value                                      |
	| ServiceAddress       | http://localhost:54321/WsdlHost_XML_NMVTIS |
	| BindingType          | BasicHttpBinding                           |
	| SecurityMode         | None                                       |
	| ClientCredentialType | None                                       |
	And download metadata from rehost metadata url to folder 'wsdl-gen\xml\NMVTIS-EWS\svcutil'

@wsdl @host @xml @svcutil @static
Scenario: Host WSDL from Static Service Publication
	Given set serialization method to 'Xml'
	And set generated assembly namespace to "AAMVA.Avattar.Contracts"
	And set rehost namespace to 'AAMVA.Avattar.DynamicServices'
	And wsdl url 'http://localhost:54321/NMVTIS_StaticServiceHost/mex'
	When I generate wsdl assembly and save them to folder 'wsdl-generated' with name 'NMVTIS.NM04.EWS.Receive.dll' using SvcUtil
	And I should discover 1 interface with name 'INmvtisReceiveResponses'
	And there should be a method with name 'VehicleHistoryInquiryResponse'
	And I rehost services using generated wsdl assembly
	| Field                | Value                                      |
	| ServiceAddress       | http://localhost:54321/WsdlHost_XML_NMVTIS |
	| BindingType          | BasicHttpBinding                           |
	| SecurityMode         | None                                       |
	| ClientCredentialType | None                                       |
	And download metadata from rehost metadata url to folder 'wsdl-gen\xml\NMVTIS-EWS\svcutil'
	Then the assembly used for wsdl host should contain non-wrapped methods

@wsdl @host @xml @svcutil @dynamic
Scenario: Host WSDL from Dynamic Service Publication
	Given set serialization method to 'Xml'
	And set generated assembly namespace to "AAMVA.Avattar.Contracts"
	And set rehost namespace to 'AAMVA.Avattar.DynamicServices'
	And wsdl url 'http://localhost:54321/NMVTIS_DynamicServiceHost/mex'
	When I generate wsdl assembly and save them to folder 'wsdl-generated' with name 'NMVTIS.NM04.EWS.Receive.dll' using SvcUtil
	And I should discover 1 interface with name 'INmvtisReceiveResponses'
	And there should be a method with name 'VehicleHistoryInquiryResponse'
	And I rehost services using generated wsdl assembly
	| Field                | Value                                      |
	| ServiceAddress       | http://localhost:54321/WsdlHost_XML_NMVTIS |
	| BindingType          | BasicHttpBinding                           |
	| SecurityMode         | None                                       |
	| ClientCredentialType | None                                       |
	And download metadata from rehost metadata url to folder 'wsdl-gen\xml\NMVTIS-EWS\svcutil'
	Then the assembly used for wsdl host should contain non-wrapped methods