using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Get a histogram of subscribers by type and then state
  /// </summary>
  public class SubscriberCountByTypeAndState {

    [JsonProperty("email")]
    public SubscriberCountByState Email { get; set; }

    [JsonProperty("sms")]
    public SubscriberCountByState Sms { get; set; }

    [JsonProperty("webhook")]
    public SubscriberCountByState Webhook { get; set; }

    [JsonProperty("integration_partner")]
    public SubscriberCountByState IntegrationPartner { get; set; }

    [JsonProperty("slack")]
    public SubscriberCountByState Slack { get; set; }

    [JsonProperty("teams")]
    public SubscriberCountByState Teams { get; set; }
  }
}