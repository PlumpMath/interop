using System;
using System.ServiceModel;
using Aamva.Nmvtis.EwsR2.JurServiceInterface;
using Common.Services;
using Common.Services.Utils;

namespace NmvtisServiceHost
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				Type contractType = typeof (INmvtisReceiveResponses);
				string contractAssemblyPath = contractType.Assembly.Location;
				const string defaultNamespace = "AAMVA.Avattar.Contracts";
				ServiceEndpointConfiguration config = new ServiceEndpointConfiguration()
				{
					ServiceAddress = "http://localhost:54321/NMVTIS_DynamicServiceHost"
				};
				DynamicHost dynamicHost = new DynamicHost(contractAssemblyPath, config);
				try
				{
					string metaDataUrl = dynamicHost.CreateDummyHost(contractType, defaultNamespace, "svc");
					Console.WriteLine("Dynamic NMVTIS service is hosted on "+metaDataUrl);
				}
				catch (Exception ex)
				{
					LogFactory.GetLog().Error(ex, "Unable to create service host");
					Console.WriteLine(ex.Message);
				}

				const string svcAddr = "http://localhost:54321/NMVTIS_StaticServiceHost";
				ServiceHost svcHost = new ServiceHost(typeof(ServiceImpl), new Uri(svcAddr));
				svcHost.ApplyDebugBehavior();
				svcHost.ApplyInstanceModeBehavior(InstanceContextMode.Single);
				svcHost.ApplyMessageLogBehavior();
				svcHost.ApplyMexBehavior(svcAddr + "/mex");
				svcHost.Open();
				Console.WriteLine("Service is listening at {0}", svcAddr);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			Console.ReadLine();
		}
	}
}
