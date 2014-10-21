using Common.Services.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Utils
{
	public static class ServiceHostBehaviorEx
	{
		public static void ApplyMexBehavior(this ServiceHost serviceHost, string metadataUrl)
		{
			ServiceMetadataBehavior meta = serviceHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
			if (meta != null)
			{
				meta.HttpGetEnabled = true;
				meta.HttpGetUrl = new Uri(metadataUrl);
			}
			else
			{
				meta = new ServiceMetadataBehavior();
				meta.HttpGetEnabled = true;
				meta.HttpGetUrl = new Uri(metadataUrl);
				serviceHost.Description.Behaviors.Add(meta);
			}
		}

		public static void ApplyDebugBehavior(this ServiceHost serviceHost)
		{
			ServiceDebugBehavior debugBehavior = serviceHost.Description.Behaviors.Find<ServiceDebugBehavior>();
			if (debugBehavior == null)
			{
				debugBehavior = new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true };
				serviceHost.Description.Behaviors.Add(debugBehavior);
			}
			else
			{
				debugBehavior.IncludeExceptionDetailInFaults = true;
			}
		}

		public static void ApplyInstanceModeBehavior(this ServiceHost serviceHost, InstanceContextMode instanceMode)
		{
			ServiceBehaviorAttribute srvBehavior = serviceHost.Description.Behaviors.Find<ServiceBehaviorAttribute>();
			if (srvBehavior == null)
			{
				srvBehavior = new ServiceBehaviorAttribute { InstanceContextMode = instanceMode };
				serviceHost.Description.Behaviors.Add(srvBehavior);
			}
			else
			{
				srvBehavior.InstanceContextMode = instanceMode;
			}
		}

		public static void ApplyMessageLogBehavior(this ServiceHost serviceHost)
		{
			foreach (var endPoint in serviceHost.Description.Endpoints)
			{
				endPoint.Behaviors.Add(new DebugMessageBehavior());
			}
		}
	}
}
