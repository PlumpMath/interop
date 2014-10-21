using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Interop.Contracts.Lessons
{
	[ServiceContract(Namespace = "AAMVA.Registration")]
	[XmlSerializerFormat]
	public interface IRegistrationManager
	{
		[OperationContract]
		//[FaultContract(typeof(ClassFullError)), FaultContract(typeof(StudentAlreadyRegisteredForThisClassError))]
		void RegisterClass(Course course, Student student);

		[OperationContract]
		List<Student> GetStudentsRegisteredForClass(Course course);
	}
}
