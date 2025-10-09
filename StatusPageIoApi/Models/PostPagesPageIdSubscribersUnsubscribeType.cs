using System.Runtime.Serialization;

namespace SergiyE.StatusPageIoApi {
  public enum PostPagesPageIdSubscribersUnsubscribeType {
    [EnumMember(Value = @"email")]
    Email = 0,

    [EnumMember(Value = @"sms")]
    Sms = 1,

    [EnumMember(Value = @"slack")]
    Slack = 2,

    [EnumMember(Value = @"webhook")]
    Webhook = 3,

    [EnumMember(Value = @"teams")]
    Teams = 4,

    [EnumMember(Value = @"integration_partner")]
    IntegrationPartner = 5,
  }
}