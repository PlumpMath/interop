using System;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;
using Common.Services.Utils;
using Common.Services.Behaviors;

namespace Common.Services
{
    public class DynamicProxy
    {
		public static IContractType Create<IContractType>(ServiceEndpointConfiguration endPointConfig)
			where IContractType : class 
		{
			if (!typeof(IContractType).IsInterface)
				throw new ArgumentException("The generic type passed in must be an interface");

			EndpointAddress endpointAdress = new EndpointAddress(endPointConfig.ServiceAddress);
			Binding binding = endPointConfig.InitBinding();
			if (binding == null)
			{
				throw new NotSupportedException("Binding is not supported");
			}
			var factory = new ChannelFactory<IContractType>(binding, endpointAdress);
			factory.Endpoint.EndpointBehaviors.Add(new DebugMessageBehavior());
			IContractType client = factory.CreateChannel();
			
			return client;
		}

	    public static object Create(Type contractType, ServiceEndpointConfiguration endpointConfig)
	    {
			EndpointAddress endpointAdress = new EndpointAddress(endpointConfig.ServiceAddress);
			Binding binding = endpointConfig.InitBinding();
			if (binding == null)
			{
				throw new NotSupportedException("Binding is not supported");
			}
			object factory = Activator.CreateInstance(
				typeof(ChannelFactory<>).MakeGenericType(contractType), binding, endpointAdress);
			var channelFactory = factory as ChannelFactory;
			if(channelFactory!=null)
			{
				channelFactory.Endpoint.EndpointBehaviors.Add(new DebugMessageBehavior());
			}
			MethodInfo createFactory = factory.GetType().GetMethod("CreateChannel", new Type[] { });
			//now dynamic proxy generation using reflection
			return createFactory.Invoke(factory, null);
	    }
    }
}
