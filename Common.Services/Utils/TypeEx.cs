using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Utils
{
	public static class TypeEx
	{
		public static void Log(this Type contractType, string title)
		{
			LogFactory.GetLog().Information(title);
			LogFactory.GetLog().Information(contractType.FullName);
			foreach (var method in contractType.GetMethods())
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendFormat("{0} {1} (", method.ReturnType, method.Name);
				var args = method.GetParameters();
				for (int i = 0; i < args.Length; i++)
				{
					sb.Append(args[i].ParameterType.Name);
					if (i < args.Length - 1)
					{
						sb.Append(",");
					}
				}
				sb.Append(")");
				LogFactory.GetLog().Information(sb.ToString());
			}
		}
	}
}
