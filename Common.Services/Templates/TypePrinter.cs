using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Templates
{
	internal static class TypePrinter
	{
		public static string ResolveTypeFullName(Type type)
		{
			if (type == typeof(void))
				return "void";

			if (type.IsGenericType)
			{
				string genericTypeName = type.GetGenericTypeDefinition().FullName;
				genericTypeName = genericTypeName.Substring(0, genericTypeName.IndexOf('`'));
				StringBuilder sb = new StringBuilder();
				sb.AppendFormat("{0}<", genericTypeName);
				var argumentTypes = type.GetGenericArguments();
				if (argumentTypes.Length > 0)
				{
					for (int i = 0; i < argumentTypes.Length; i++)
					{
						sb.Append(argumentTypes[i].FullName);
						if (i < argumentTypes.Length - 1)
							sb.Append(", ");
					}
				}
				sb.Append(">");
				return sb.ToString();
			}
			return type.FullName;
		}

		public static string ResolveTypeFullName(List<Type> types)
		{
			if (types == null || types.Count == 0)
				return "";

			StringBuilder sb = new StringBuilder();
			foreach (var type in types)
			{
				string typeFullName = ResolveTypeFullName(type);
				if (sb.Length > 0) sb.Append(", ");
				sb.Append(typeFullName);
			}
			return sb.ToString();
		}
	}
}
