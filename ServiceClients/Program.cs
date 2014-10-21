using Common.Services;
using Common.Services.Utils;
using ServiceClients.ReceiveServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceClients
{
	class Program
	{
		static void Main(string[] args)
		{
			Thread.Sleep(TimeSpan.FromSeconds(3));

			Type contractType = typeof(INmvtisReceiveResponses);
			Type clientType = typeof(NmvtisReceiveResponsesClient);
			
			foreach(var interfaceMethod in contractType.GetMethods())
			{
				var clientMethods = clientType.GetMethods().Where(m=>m.Name==interfaceMethod.Name).ToList();
				foreach (var clientMethod in clientMethods)
				{
					if (clientMethod.ReturnType is Task)
					{
						Console.WriteLine("Async Method: " + clientMethod.Name);
					}
					else if(clientMethod.ReturnType.IsGenericType && 
						clientMethod.ReturnType == typeof(Task<>))
					{
						Console.WriteLine("Async Method: " + clientMethod.Name);
					}
					else if (clientMethod.ReturnType.IsGenericType &&
					clientMethod.ReturnType.BaseType == typeof(Task))
					{
						Console.WriteLine("Async Method: " + clientMethod.Name);
					}
					else
					{
						var methodArgs = clientMethod.GetParameters();
						if (methodArgs.Length == 1 &&
							methodArgs[0].ParameterType.GetCustomAttributes(typeof(MessageContractAttribute), true).Any())
						{
							Console.WriteLine("Wrapped Method: " + clientMethod.Name);
						}
						else
						{
							Console.WriteLine("Non-Wrapped Method: " + clientMethod.Name);
						}
					}
				}
			}

			var method = typeof(NmvtisReceiveResponsesClient).GetMethod("UsedVehicleInquiryResponse");
			if (method != null)
			{
				Console.WriteLine(method.Name);
				Console.WriteLine(method.GetParameters().Length);
			}

			ServiceEndpointConfiguration config = new ServiceEndpointConfiguration()
			{
				ServiceAddress = "http://localhost:54321/DynamicHost_NMVTIS"
			};
			INmvtisReceiveResponses proxy = DynamicProxy.Create<INmvtisReceiveResponses>(config);
			try
			{
				var response = proxy.UsedVehicleInquiryResponse(new UsedVehicleInquiryResponseRequest());
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			var binding = config.InitBinding();
			var endpointAddr = new EndpointAddress(config.ServiceAddress);
			var client = new NmvtisReceiveResponsesClient(binding, endpointAddr);
			try
			{
				client.UsedVehicleInquiryResponse(null, null, null);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			Console.ReadLine();
		}
	}
}
