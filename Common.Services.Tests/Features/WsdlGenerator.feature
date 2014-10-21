Feature: WsdlGenerator

@wsdl @dynamic_host @data_contract_serializer
Scenario: Should Be Able To Generate and Host Dummy Service From Interface with Data Contract Serializer
	Given a referenced assembly in bin folder called 'Interop.Contracts.dll'
	And Pick an interface with name 'IOrderManager'
	And set generated assembly namespace to "AAMVA.Orders"
	When I generate and host service with the following setting
	| Field                | Value                                           |
	| ServiceAddress       | http://localhost:54321/WsdlPublish_DataContract |
	| BindingType          | BasicHttpBinding                                |
	| SecurityMode         | None                                            |
	| ClientCredentialType | None                                            |
	Then I should be able to download wsdl files to folder '~\wsdls\datacontract\'

@wsdl @dynamic_host @xml_serializer
Scenario: Should Be Able To Generate and Host Dummy Service From Interface With XML Serializer
	Given a referenced assembly in bin folder called 'Interop.Contracts.dll'
	And Pick an interface with name 'IRegistrationManager'
	And set generated assembly namespace to "AAMVA.Registration"
	When I generate and host service with the following setting
	| Field                | Value                                  |
	| ServiceAddress       | http://localhost:54321/DynamicHost_Xml |
	| BindingType          | BasicHttpBinding                       |
	| SecurityMode         | None                                   |
	| ClientCredentialType | None                                   |
	Then I should be able to download wsdl files to folder '~\wsdls\xml\'
