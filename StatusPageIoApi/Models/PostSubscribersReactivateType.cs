using System.Runtime.Serialization;

namespace StatusPageIoApi {
  public enum PostSubscribersReactivateType {
    [EnumMember(Value = @"email")]
    Email = 0,

    [EnumMember(Value = @"sms")]
    Sms = 1,

    [EnumMember(Value = "slack")]
    Slack = 2,

    [EnumMember(Value = "webhook")]
    Webhook = 3,

    [EnumMember(Value = "integration_partner")]
    IntegrationPartner = 4,
  }
}