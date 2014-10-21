using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;

namespace Common.Services.Utils
{
	public class ReflectionUtil
	{
		public static bool IsInterfaceWrapped(Type interfaceType)
		{
			bool isWrapped = true;
			foreach (var method in interfaceType.GetMethods())
			{
				if (!IsMethodWrapped(method))
				{
					isWrapped = false;
					break;
				}
			}
			return isWrapped;
		}

		public static bool IsMethodWrapped(MethodInfo method)
		{
			var args = method.GetParameters();
			if (args.Length == 1 && args[0].ParameterType.GetCustomAttribute<MessageContractAttribute>() != null)
			{
				return true;
			}
			return false;
		}

		public static bool IsAssemblyInGAC(string assemblyFullName)
		{
			try
			{
				return Assembly.ReflectionOnlyLoad(assemblyFullName).GlobalAssemblyCache;
			}
			catch
			{
				return false;
			}
		}

		public static bool IsAssemblyInGAC(Assembly assembly)
		{
			return assembly.GlobalAssemblyCache;
		}

		public static void DiscoverDependency(Assembly assembly, List<string> assemblyLocations)
		{
			if (!IsAssemblyInGAC(assembly))
			{
				if (!string.IsNullOrEmpty(assembly.Location))
				{
					if (!assemblyLocations.Contains(assembly.Location))
					{
						assemblyLocations.Add(assembly.Location);
					}
				}
			}

			var referencedAssemblies = assembly.GetReferencedAssemblies().ToList();
			if (referencedAssemblies.Count > 0)
			{
				foreach (var refAssemblyName in referencedAssemblies)
				{
					Assembly refAssembly = Assembly.Load(refAssemblyName);
					if (!IsAssemblyInGAC(refAssembly))
					{
						if (!string.IsNullOrEmpty(refAssembly.Location))
						{
							assemblyLocations.Add(refAssembly.Location);

							DiscoverDependency(refAssembly, assemblyLocations);
						}
					}
				}
			}
		}
	}
}
