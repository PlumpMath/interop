using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;

namespace Common.Services.Utils
{
	public static class BindingEx
	{
		public static Binding InitBinding(this ServiceEndpointConfiguration config)
		{
			Binding binding = null;
			switch (config.BindingType)
			{
				case ServiceBindingTypes.BasicHttpBinding:
					binding = new BasicHttpBinding()
					{
						MaxReceivedMessageSize = int.MaxValue,
						MaxBufferPoolSize = int.MaxValue,
						MaxBufferSize = int.MaxValue,
					};
					break;
				case ServiceBindingTypes.NetTcpBinding:
					binding = new NetTcpBinding()
					{
						MaxReceivedMessageSize = int.MaxValue,
						MaxBufferPoolSize = int.MaxValue,
						MaxBufferSize = int.MaxValue,
					};
					break;
				case ServiceBindingTypes.WSHttpBinding:
					binding = new WSHttpBinding()
					{
						MaxReceivedMessageSize = int.MaxValue,
						MaxBufferPoolSize = int.MaxValue,
					};
					break;
				case ServiceBindingTypes.WsDualHttpBinding:
					binding = new WSDualHttpBinding()
					{
						MaxReceivedMessageSize = int.MaxValue,
						MaxBufferPoolSize = int.MaxValue,
					};
					break;
			}
			if (binding != null)
			{
				binding.ApplySecuritySettings(config);

				XmlDictionaryReaderQuotas myReaderQuotas = new XmlDictionaryReaderQuotas();
				myReaderQuotas.MaxStringContentLength = int.MaxValue;
				myReaderQuotas.MaxArrayLength = int.MaxValue;
				myReaderQuotas.MaxBytesPerRead = int.MaxValue;
				myReaderQuotas.MaxDepth = int.MaxValue;
				myReaderQuotas.MaxNameTableCharCount = int.MaxValue;
				binding.GetType().GetProperty("ReaderQuotas").SetValue(binding, myReaderQuotas, null);
			}
			return binding;
		}

		private static void ApplySecuritySettings(this Binding binding, ServiceEndpointConfiguration config)
		{
			#region BasicHttpBinding
			BasicHttpBinding basicHttpBinding = binding as BasicHttpBinding;
			if (basicHttpBinding != null)
			{
				switch (config.SecurityMode)
				{
					case ServiceSecurityModes.None:
						basicHttpBinding.Security.Mode = BasicHttpSecurityMode.None;
						break;
					case ServiceSecurityModes.Transport:
						basicHttpBinding.Security.Mode = BasicHttpSecurityMode.Transport;
						break;
					case ServiceSecurityModes.Message:
						basicHttpBinding.Security.Mode = BasicHttpSecurityMode.Message;
						break;
					case ServiceSecurityModes.TransportWithMessaegCredential:
						basicHttpBinding.Security.Mode = BasicHttpSecurityMode.TransportWithMessageCredential;
						break;
					case ServiceSecurityModes.TransportCredentialOnly:
						basicHttpBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
						break;
					default:
						basicHttpBinding.Security.Mode = BasicHttpSecurityMode.None;
						break;
				}

				switch (config.ClientCredentialType)
				{
					case ClientCredentialTypes.UserName:
						basicHttpBinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
						break;
					case ClientCredentialTypes.Certificate:
						basicHttpBinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.Certificate;
						break;
				}
				return;
			}
			#endregion

			#region NetTcpBinding
			NetTcpBinding netTcpBinding = binding as NetTcpBinding;
			if (netTcpBinding != null)
			{
				switch (config.SecurityMode)
				{
					case ServiceSecurityModes.None:
						netTcpBinding.Security.Mode = SecurityMode.None;
						break;
					case ServiceSecurityModes.Transport:
						netTcpBinding.Security.Mode = SecurityMode.Transport;
						break;
					case ServiceSecurityModes.Message:
						netTcpBinding.Security.Mode = SecurityMode.Message;
						break;
					case ServiceSecurityModes.TransportWithMessaegCredential:
						netTcpBinding.Security.Mode = SecurityMode.TransportWithMessageCredential;
						break;
					default:
						netTcpBinding.Security.Mode = SecurityMode.None;
						break;
				}


				switch (config.ClientCredentialType)
				{
					case ClientCredentialTypes.UserName:
						netTcpBinding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
						break;
					case ClientCredentialTypes.Certificate:
						netTcpBinding.Security.Message.ClientCredentialType = MessageCredentialType.Certificate;
						break;
					case ClientCredentialTypes.Windows:
						netTcpBinding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
						break;
					case ClientCredentialTypes.IssuedToken:
						netTcpBinding.Security.Message.ClientCredentialType = MessageCredentialType.IssuedToken;
						break;
					default:
						netTcpBinding.Security.Message.ClientCredentialType = MessageCredentialType.None;
						break;
				}
				return;
			}
			#endregion

			#region WSHttpBinding
			WSHttpBinding wsHttpBinding = binding as WSHttpBinding;
			if (wsHttpBinding != null)
			{
				switch (config.SecurityMode)
				{
					case ServiceSecurityModes.None:
						wsHttpBinding.Security.Mode = SecurityMode.None;
						break;
					case ServiceSecurityModes.Transport:
						wsHttpBinding.Security.Mode = SecurityMode.Transport;
						break;
					case ServiceSecurityModes.Message:
						wsHttpBinding.Security.Mode = SecurityMode.Message;
						break;
					case ServiceSecurityModes.TransportWithMessaegCredential:
						wsHttpBinding.Security.Mode = SecurityMode.TransportWithMessageCredential;
						break;
					default:
						wsHttpBinding.Security.Mode = SecurityMode.None;
						break;
				}

				switch (config.ClientCredentialType)
				{
					case ClientCredentialTypes.UserName:
						wsHttpBinding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
						break;
					case ClientCredentialTypes.Certificate:
						wsHttpBinding.Security.Message.ClientCredentialType = MessageCredentialType.Certificate;
						break;
					case ClientCredentialTypes.Windows:
						wsHttpBinding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
						break;
					case ClientCredentialTypes.IssuedToken:
						wsHttpBinding.Security.Message.ClientCredentialType = MessageCredentialType.IssuedToken;
						break;
					default:
						wsHttpBinding.Security.Message.ClientCredentialType = MessageCredentialType.None;
						break;
				}
				return;
			}
			#endregion
		}
	}
}
