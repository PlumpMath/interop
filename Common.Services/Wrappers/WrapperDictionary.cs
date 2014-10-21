using System;
using System.Collections.Concurrent;

namespace Common.Services.Wrappers
{
	internal class WrapperDictionary
	{
		private readonly ConcurrentDictionary<string, Type> _wrapperTypes = new ConcurrentDictionary<string, Type>();

		private static string GenerateKey(Type interfaceType, Type realObjectType)
		{
			return interfaceType.Name + "->" + realObjectType.Name;
		}

		public Type GetType(Type interfaceType, Type realObjectType)
		{
			string key = GenerateKey(interfaceType, realObjectType);
			if (_wrapperTypes.ContainsKey(key))
			{
				return _wrapperTypes[key];
			}
			return null;
		}

		public void SetType(Type interfaceType, Type realObjectType, Type wrapperType)
		{
			string key = GenerateKey(interfaceType, realObjectType);
			_wrapperTypes[key] = wrapperType;
		}
	}
}
