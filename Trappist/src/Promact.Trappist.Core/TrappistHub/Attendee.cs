using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Promact.Trappist.Core.TrappistHub;

[ExcludeFromCodeCoverage]
public class Attendee
{
    public int AttendeeId { get; set; }
    public DateTime StartDate { get; set; }
    public HashSet<string> ConnectionIds { get; set; }
    public bool IsAttendeeReset { get; set; }
}