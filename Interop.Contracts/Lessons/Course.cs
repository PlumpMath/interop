using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interop.Contracts.Lessons
{
	[System.CodeDom.Compiler.GeneratedCodeAttribute("TextTransform", "12.0.21005.1")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://aamva.org/xsd/nmvtis/exchange/pvhi/3.1")]
	[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://aamva.org/xsd/nmvtis/exchange/pvhi/3.1", IsNullable = true)]
	public class Course
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Capacity { get; set; }
		public Instructor Instructor { get; set; }
		public List<Student> Students { get; set; }
	}
}
