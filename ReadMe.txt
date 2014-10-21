the solution will implement schema-first SOA 
	1. using Microsoft technology 
		a) WCF
		b) SQL server
		c) MSMQ
	2. plus open source technology
		a) Use RabbitMQ for message queue
		b) Use AngularJS, Bootstrap, d3js for web
		c) Use serilog for structured logging
		d) Use MongoDB for log, message storage
		e) Use Lucene and SOLR for free text search

A. Data Objects
	1. For data objects, it will use both DataContract serializer and Xml serializer
	2. For data object classes, it will use both manually crafted c sharp classes and XSD generated objects 

B. Service Contracts
	1. service operation arguments will have the following scenarios
		a) 0, 1 or more input arguments
		b) input arguments is either primitive type of complex type
		c) output argument can be void, primitive or complex type
	2. service operations can be in one of the following scenarios
		a) one-way operation
		b) callback
		c) request-response
	3. service behaviors
		1) option to enable service profile and track usage and performance
		2) option to enable raw msg logging before receive and after send
		3) interceptors to check soap header, WS-addressing, and faults
		4) structured logging with serilog and choose MongoDB for log sink, create web UI for log query
			
C. Binding to support http/tcp protocols and SSL
	1. basicHttpBinding
	2. wsHttpBinding
	3. wsDualBinding

D. Security 
	1. None
	2. Username
	3. Certificate
	4. Claims

D. Dynamic Proxy given address, binding and behavior
	1. create proxy based on WSDL file and contract name
	2. create proxy based on assembly and contract type

E. Dynamic Host given address, binding and behavior
	1. create host based on WSDL file and contract name
	2. create host based on assembly and contract name
	3. uses T4 to create svc impl classes, method hooks should use message queue

F. Message Queue
	1. MSMQ
	2. RabbitMQ
	3. ZeroQ
	4. create web UI for message queue management and monitoring   

G. Workflow
	1. Workflow to pick message from sender queue and call web service 
	2. Workflow to pick message from receiver queue
	3. Web UI for workflow designer
	4. Web UI for workflow management (processing and monitoring)