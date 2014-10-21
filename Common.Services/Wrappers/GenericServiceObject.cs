using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;

namespace Common.Services.Wrappers
{
	public class GenericServiceObject 
	{
		private readonly Type _contractType;
		private readonly Dictionary<string, string> _methodMappings = new Dictionary<string, string>();
		private readonly Dictionary<string, List<MethodInfo>> _callers = new Dictionary<string, List<MethodInfo>>();
		public Dictionary<string, string> MethodMappings
		{
			get { return _methodMappings; }
		}

		public GenericServiceObject(Type contractType)
		{
			_contractType = contractType;
			foreach (MethodInfo method in contractType.GetMethods())
			{
				var args = method.GetParameters().ToList();
				if (args.Count == 1 && method.ReturnType==typeof(void))
				{
					AddMethodForCaller(method, "Method1a");
				}
				if (args.Count == 1 && method.ReturnType!=typeof(void))
				{
					AddMethodForCaller(method, "Method1b");
				}
				else if (args.Count == 2 && method.ReturnType==typeof(void))
				{
					AddMethodForCaller(method, "Method2a");
				}
				else if (args.Count == 2 && method.ReturnType == typeof(string) && args[0].ParameterType==typeof(byte[]))
				{
					AddMethodForCaller(method, "Method2b");
				}
				else if (args.Count == 2 && method.ReturnType == typeof(void) && args[0].ParameterType != typeof(byte[]))
				{
					AddMethodForCaller(method, "Method2c");
				}
				else if (args.Count == 2 && method.ReturnType != typeof(void) && args[0].ParameterType == typeof(string) && args[1].ParameterType==typeof(byte[]))
				{
					AddMethodForCaller(method, "Method2d");
				}
				else if (args.Count == 3)
				{
					AddMethodForCaller(method, "Method3");
				}
				else
				{
					if(method.ReturnType==typeof(void))
					{
						_methodMappings.Add(method.Name, "Invoke");
					}
					else
					{
						_methodMappings.Add(method.Name, "Process");
					}
				}
			}
		}

		private void AddMethodForCaller(MethodInfo method, string methodName)
		{
			_methodMappings.Add(method.Name, methodName);
			List<MethodInfo> callers = new List<MethodInfo>();
			if (_callers.ContainsKey(methodName))
				callers = _callers[methodName];
			else
				_callers.Add(methodName, callers);
			callers.Add(method);
		}

		public void Invoke(string contractName, string methodName, params object[] args)
		{
			LogFactory.GetLog().Information("Entering method {0}->{1}", methodName, "Invoke");
			throw new FaultException(new FaultReason("Method Invoke Is Not implemented"));
		}

		public object Process(string contractName, string methodName, params object[] args)
		{
			LogFactory.GetLog().Information("Entering method {0}->{1}", methodName, "Process");
			throw new FaultException(new FaultReason("Method Process Is Not implemented"));
		}

		private void DisplayMethodCaller(string methodName)
		{
			if(_callers.ContainsKey(methodName))
			{
				foreach(var method in _callers[methodName])
				{
					StringBuilder sb = new StringBuilder();
					sb.AppendFormat("{0}(", method.Name);
					var args = method.GetParameters();
					for (int i = 0; i < args.Length;i++)
					{
						sb.Append(args[i].ParameterType.Name);
						if(i<args.Length -1)
						{
							sb.Append(", ");
						}
					}
					sb.Append(")");
					LogFactory.GetLog().Information("ServiceObject: Entering {0}", sb.ToString());
				}
			}
		}

		#region IServiceObject Members

		public void Method1a(object request)
		{
			DisplayMethodCaller("Method1a");
			throw new FaultException(new FaultReason("Method1a Is Not implemented"));
		}

		public object Method1b(object request)
		{
			DisplayMethodCaller("Method1b");
			throw new FaultException(new FaultReason("Method1b Is Not implemented"));
		}

		public void Method2a(byte[] authToken, object request)
		{
			DisplayMethodCaller("Method2a");
			throw new FaultException(new FaultReason("Method2a Is Not implemented"));
		}

		public string Method2b(byte[] authToken, object request)
		{
			DisplayMethodCaller("Method2b");
			throw new FaultException(new FaultReason("Method2b Is Not implemented"));
		}

		public void Method2c(object obj1, object obj2)
		{
			DisplayMethodCaller("Method2c");
			throw new FaultException(new FaultReason("Method2c Is Not implemented"));
		}

		public object Method2d(string obj1, byte[] obj2)
		{
			DisplayMethodCaller("Method2d");
			throw new FaultException(new FaultReason("Method2d Is Not implemented"));
		}

		public void Method3(byte[] authToken, string ackToken, object response)
		{
			DisplayMethodCaller("Method3");
			throw new FaultException(new FaultReason("Method3 Is Not implemented"));
		}

		#endregion

	}
}
