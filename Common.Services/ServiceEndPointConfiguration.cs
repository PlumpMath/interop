namespace Common.Services
{
	public enum ServiceBindingTypes
	{
		BasicHttpBinding,
		NetTcpBinding,
		NetNamedPipeBinding,
		WSHttpBinding,
		NetMsmqBinding,
		WsDualHttpBinding
	}

	public enum ServiceSecurityModes
	{
		None,
		Transport,
		Message,
		Both,
		TransportWithMessaegCredential,
		TransportCredentialOnly
	}

	public enum ClientCredentialTypes
	{
		None,
		Windows,
		UserName,
		Certificate,
		IssuedToken
	}

	public class ServiceEndpointConfiguration
	{
		public string ServiceAddress { get; set; }
		public ServiceBindingTypes BindingType { get; set; }
		public ServiceSecurityModes SecurityMode { get; set; }
		public ClientCredentialTypes ClientCredentialType { get; set; }
	}
}
