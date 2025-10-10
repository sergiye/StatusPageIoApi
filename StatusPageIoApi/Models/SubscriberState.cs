using System.Runtime.Serialization;

namespace StatusPageIoApi {
  public enum SubscriberState {
    [EnumMember(Value = @"active")]
    Active = 0,

    [EnumMember(Value = @"unconfirmed")]
    Unconfirmed = 1,

    [EnumMember(Value = @"quarantined")]
    Quarantined = 2,

    [EnumMember(Value = @"all")]
    All = 3,
  }
}