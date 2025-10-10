using System.Runtime.Serialization;

namespace StatusPageIoApi {
  public enum TemplateUpdateStatus {
    [EnumMember(Value = "investigating")]
    Investigating = 0,

    [EnumMember(Value = "identified")]
    Identified = 1,

    [EnumMember(Value = "monitoring")]
    Monitoring = 2,

    [EnumMember(Value = "resolved")]
    Resolved = 3,

    [EnumMember(Value = "scheduled")]
    Scheduled = 4,

    [EnumMember(Value = "in_progress")]
    InProgress = 5,

    [EnumMember(Value = "verifying")]
    Verifying = 6,

    [EnumMember(Value = "completed")]
    Completed = 7,
  }
}