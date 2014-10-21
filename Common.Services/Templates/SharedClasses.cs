using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Services.Templates
{
	public partial class ServiceImplSkeletonTemplate
	{
		public string NameSpace { get; private set; }
		public List<Type> ContractTypes { get; private set; }
		public string ServiceName { get; private set; }

		public ServiceImplSkeletonTemplate(
			string nameSpace, Type contractType,
			string serviceName)
		{
			this.NameSpace = nameSpace;
			this.ContractTypes = new List<Type>() { contractType };
			this.ServiceName = serviceName;
		}

		public ServiceImplSkeletonTemplate(
			string nameSpace, List<Type> contractTypes,
			string serviceName)
		{
			this.NameSpace = nameSpace;
			this.ContractTypes = contractTypes;
			this.ServiceName = serviceName;
		}
	}
}
