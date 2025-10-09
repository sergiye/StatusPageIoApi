using System.Runtime.Serialization;

namespace SergiyE.StatusPageIoApi {
  public enum IncidentImpactOverride {
    [EnumMember(Value = "none")]
    None = 0,

    [EnumMember(Value = "maintenance")]
    Maintenance = 1,

    [EnumMember(Value = "minor")]
    Minor = 2,

    [EnumMember(Value = "major")]
    Major = 3,

    [EnumMember(Value = "critical")]
    Critical = 4,
  }
}