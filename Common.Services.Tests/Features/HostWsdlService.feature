Feature: HostWsdlService

@wsdl @rehost @data_contract @svcutil
Scenario: Rehost WSDL with Data Contract and Svcutil
	Given a referenced assembly in bin folder called 'Interop.Contracts.dll'
	And Pick an interface with name 'IOrderManager'
	And set serialization method to 'DataContract'
	And set generated assembly namespace to "AAMVA.Orders"
	And set rehost namespace to 'AAMVA.Orders.Services'
	When I generate and host service with the following setting
	| Field                | Value                                           |
	| ServiceAddress       | http://localhost:54321/WsdlPublish_DataContract |
	| BindingType          | BasicHttpBinding                                |
	| SecurityMode         | None                                            |
	| ClientCredentialType | None                                            |
	And I generate wsdl assembly and save them to folder 'wsdl-generated' with name 'OrderManager.wsdl.dll' using SvcUtil
	And I rehost services using generated wsdl assembly
	| Field                | Value                                        |
	| ServiceAddress       | http://localhost:54321/WsdlHost_DataContract |
	| BindingType          | BasicHttpBinding                             |
	| SecurityMode         | None                                         |
	| ClientCredentialType | None                                         |
	And download metadata from rehost metadata url to folder 'wsdl-gen\datacontract\orders\svcutil'
	And call service using channel factory and the same contract interface
	Then I should be able to hit service impl class and return dummy object

@wsdl @rehost @xml @svcutil
Scenario: Rehost WSDL with Xml Serialization and Svcutil
	Given a referenced assembly in bin folder called 'Interop.Contracts.dll'
	And set serialization method to 'Xml'
	And Pick an interface with name 'IRegistrationManager'
	And set generated assembly namespace to "AAMVA.Registration"
	And set rehost namespace to 'AAMVA.Registration.Services'
	When I generate and host service with the following setting
	| Field                | Value                                  |
	| ServiceAddress       | http://localhost:54321/WsdlPublish_Xml |
	| BindingType          | BasicHttpBinding                       |
	| SecurityMode         | None                                   |
	| ClientCredentialType | None                                   |
	And I generate wsdl assembly and save them to folder 'wsdl-generated' with name 'RegistrationManager.wsdl.dll' using SvcUtil
	And I rehost services using generated wsdl assembly
	| Field                | Value                               |
	| ServiceAddress       | http://localhost:54321/WsdlHost_Xml |
	| BindingType          | BasicHttpBinding                    |
	| SecurityMode         | None                                |
	| ClientCredentialType | None                                |
	And download metadata from rehost metadata url to folder 'wsdl-gen\xml\register\svcutil'
	And call service using channel factory and the same contract interface
	Then I should be able to hit service impl class and return dummy object

@wsdl @rehost @data_contract @metadata_set
Scenario: Rehost WSDL with Data Contract and Metadataset

	Given a referenced assembly in bin folder called 'Interop.Contracts.dll'
	And Pick an interface with name 'IOrderManager'
	And set serialization method to 'DataContract'
	And set generated assembly namespace to "AAMVA.Orders"
	And set rehost namespace to 'AAMVA.Orders.Services'
	When I generate and host service with the following setting
	| Field                | Value                                           |
	| ServiceAddress       | http://localhost:54321/WsdlPublish_DataContract |
	| BindingType          | BasicHttpBinding                                |
	| SecurityMode         | None                                            |
	| ClientCredentialType | None                                            |
	And I generate wsdl assembly using method MetadataSet
	And I rehost services using generated wsdl assembly
	| Field                | Value                                        |
	| ServiceAddress       | http://localhost:54321/WsdlHost_DataContract |
	| BindingType          | BasicHttpBinding                             |
	| SecurityMode         | None                                         |
	| ClientCredentialType | None                                         |
	And download metadata from rehost metadata url to folder 'wsdl-gen\datacontract\orders\metadataset'
	And call service using channel factory and the same contract interface
	Then I should be able to hit service impl class and return dummy object

@wsdl @rehost @xml @metadata_set
Scenario: Rehost WSDL with Xml and Metadataset
	Given a referenced assembly in bin folder called 'Interop.Contracts.dll'
	And set serialization method to 'Xml'
	And Pick an interface with name 'IRegistrationManager'
	And set generated assembly namespace to "AAMVA.Registration"
	And set rehost namespace to 'AAMVA.Registration.Services'
	When I generate and host service with the following setting
	| Field                | Value                                  |
	| ServiceAddress       | http://localhost:54321/WsdlPublish_Xml |
	| BindingType          | BasicHttpBinding                       |
	| SecurityMode         | None                                   |
	| ClientCredentialType | None                                   |
	And I generate wsdl assembly using method MetadataSet
	And I rehost services using generated wsdl assembly
	| Field                | Value                               |
	| ServiceAddress       | http://localhost:54321/WsdlHost_Xml |
	| BindingType          | BasicHttpBinding                    |
	| SecurityMode         | None                                |
	| ClientCredentialType | None                                |
	And download metadata from rehost metadata url to folder 'wsdl-gen\datacontract\register\metadataset'
	And call service using channel factory and the same contract interface
	Then I should be able to hit service impl class and return dummy object
