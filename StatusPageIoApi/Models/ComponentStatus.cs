using System.Runtime.Serialization;

namespace StatusPageIoApi {
  public enum ComponentStatus {
    [EnumMember(Value = @"operational")]
    Operational = 0,

    [EnumMember(Value = @"under_maintenance")]
    UnderMaintenance = 1,

    [EnumMember(Value = @"degraded_performance")]
    DegradedPerformance = 2,

    [EnumMember(Value = @"partial_outage")]
    PartialOutage = 3,

    [EnumMember(Value = @"major_outage")]
    MajorOutage = 4,

    [EnumMember(Value = @"")]
    Empty = 5,
  }
}