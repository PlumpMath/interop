using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Common.Services.Behaviors
{
	public static class MessageLogger
	{
		public static void LogMessage(this Message message, string messageType)
		{
			MemoryStream ms = new MemoryStream();
			XmlWriter writer = XmlWriter.Create(ms);
			message.WriteMessage(writer);
			writer.Flush();
			ms.Position = 0;
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.PreserveWhitespace = true;
			xmlDoc.Load(ms);

			Console.WriteLine("{0}: {1}", DateTime.Now, messageType);
			LogFactory.GetLog().Information(messageType);
			LogFactory.GetLog().Information(xmlDoc.OuterXml);
		}
	}
}
