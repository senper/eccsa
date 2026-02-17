using ECCSA.BackOffice.Server.Model.Academy;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Academy
{
    public record CreateAttendanceInput (
		Guid TrainingId
	, Guid RegistrationId
	, DateTime AttendanceDate
	, bool Morning,
		bool Afternoon,
		bool Evening
);


public record UpdateAttendanceInput (
	Guid Id
	, Guid TrainingId
	, Guid RegistrationId
	, DateTime AttendanceDate
	, bool Morning,
		bool Afternoon,
		bool Evening
);




    
    
    public record AttendancePayload(Attendance PayLoad);
    
}
