using System;

namespace ECCSA.BackOffice.Server.Model.Academy
{
    public class Attendance
	{
		public Guid Id { get; set; }
		public Guid TrainingId { get; set; }
		public Guid RegistrationId { get; set; }
		public DateTime AttendanceDate { get; set; }		
		public bool Morning { get; set; }
		public bool Afternoon { get; set; }
		public bool Evening { get; set; }
		

		public virtual Training Training { get; set; }
public virtual Registration Registration { get; set; }

	}
}