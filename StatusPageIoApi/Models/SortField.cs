using System.Runtime.Serialization;

namespace StatusPageIoApi {
  public enum SortField {
    [EnumMember(Value = @"primary")]
    Primary = 0,

    [EnumMember(Value = @"created_at")]
    CreatedAt = 1,

    [EnumMember(Value = @"quarantined_at")]
    QuarantinedAt = 2,

    [EnumMember(Value = @"relevance")]
    Relevance = 3,
  }
}