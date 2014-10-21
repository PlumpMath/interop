using Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceHosts
{
	class Program
	{
		static void Main(string[] args)
		{
			const string metadataUrl = "http://localhost:12536/VehicleSystems/NMVTIS/NmvtisReceiveResponses/Receive?wsdl";
			const string defaultNS = "AAMVA.Avattar.DynamicServices";
			ServiceEndpointConfiguration config=new ServiceEndpointConfiguration()
			{
				ServiceAddress="http://localhost:54321/DynamicHost_NMVTIS"
			};
			DynamicHostFromWsdlUrl host = new DynamicHostFromWsdlUrl(metadataUrl, true, defaultNS, config);
			try
			{
				host.CreateWsdlAssemblyAndHost();
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			Console.WriteLine("Service is listening at {0}", config.ServiceAddress);

			Console.ReadLine();
		}
	}
}
