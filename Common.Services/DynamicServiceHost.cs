using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using Common.Services.Utils;

namespace Common.Services
{
	public class DynamicServiceHost
	{
		#region props

		// public string ServiceAddress { get; private set; }
		public Type ContractType { get; private set; }
		// public object ServiceObject { get; private set; }
		public EventHandler<CommunicationState> StateChanged;
		public ServiceHost ServiceHost { get; private set; }
		public string MetaDataUrl { get; private set; }
		#endregion

		#region ctor
		public DynamicServiceHost(Type contractType)
		{
			ContractType = contractType;
		}
		#endregion

		#region create
		public void CreateAndStartService(Type serviceType, ServiceEndpointConfiguration config)
		{
			//ServiceAddress = config.ServiceAddress;
			this.ServiceHost = new ServiceHost(serviceType);
			SetConfiguration(config);
			StartService();
		}
		public void CreateAndStartService(object serviceObject, ServiceEndpointConfiguration config)
		{
			//ServiceAddress = config.ServiceAddress;
			// ServiceObject = genericServiceObject;
			this.ServiceHost = new ServiceHost(serviceObject);
			SetConfiguration(config);
			StartService();
		}

		private void SetConfiguration(ServiceEndpointConfiguration config)
		{
			Binding binding = config.InitBinding();
			this.ServiceHost.AddServiceEndpoint(ContractType, binding, config.ServiceAddress);
			//foreach (var endPoint in this.ServiceHost.Description.Endpoints)
			//{
			//	endPoint.Behaviors.Add(new DebugMessageBehavior());
			//}
			this.MetaDataUrl = config.ServiceAddress + "/mex";
			this.ServiceHost.ApplyMexBehavior(this.MetaDataUrl);
			this.ServiceHost.ApplyDebugBehavior();
			this.ServiceHost.ApplyInstanceModeBehavior(InstanceContextMode.Single);
			this.ServiceHost.ApplyMessageLogBehavior();
			
		}
		#endregion

		#region start/close
		private void StartService()
		{
			this.ServiceHost.Closed += OnClosed;
			this.ServiceHost.Opened += OnOpened;
			this.ServiceHost.Faulted += OnFaulted;
			this.ServiceHost.Open();
		}

		private void OnClosed(object sender, EventArgs e)
		{
			if (StateChanged != null)
				StateChanged(this, CommunicationState.Closed);
		}

		private void OnOpened(object sender, EventArgs e)
		{
			if (StateChanged != null)
				StateChanged(this, CommunicationState.Opened);
		}

		private void OnFaulted(object sender, EventArgs e)
		{
			if (StateChanged != null)
				StateChanged(this, CommunicationState.Faulted);
		}

		public void Close()
		{
			if (ServiceHost != null)
			{
				if (ServiceHost.State == CommunicationState.Faulted)
					ServiceHost.Abort();
				else
					ServiceHost.Close();
			}
		}
		#endregion

		#region discover
		public IEnumerable<string> Methods
		{
			get { return ContractType.GetMethods().Select(x => x.Name); }
		}
		#endregion
	}
}
