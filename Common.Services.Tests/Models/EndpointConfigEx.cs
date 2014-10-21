using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Common.Services.Tests.Models
{
	public static class EndpointConfigEx
	{
		public static ServiceEndpointConfiguration GetServiceEndPointConfiguration(this Table table)
		{
			ServiceEndpointConfiguration config = new ServiceEndpointConfiguration();
			var pairs = table.CreateSet<BindableNameValue>().ToList();
			foreach (var pair in pairs)
			{
				if (pair.Field == "ServiceAddress")
					config.ServiceAddress = pair.Value;
				if (pair.Field == "BindingType")
					config.BindingType = pair.Value.EnumValue<ServiceBindingTypes>();
				if (pair.Field == "SecurityMode")
					config.SecurityMode = pair.Value.EnumValue<ServiceSecurityModes>();
				if (pair.Field == "ClientCredentialType")
					config.ClientCredentialType = pair.Value.EnumValue<ClientCredentialTypes>();
			}
			return config;
		}
	}
}
