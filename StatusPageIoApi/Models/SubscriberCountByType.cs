using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {

  public class SubscriberCountByType {

    /// <summary>
    /// The number of Email subscribers found by the query.
    /// </summary>
    [JsonProperty("email")]
    public int? Email { get; set; }

    /// <summary>
    /// The number of Webhook subscribers found by the query.
    /// </summary>
    [JsonProperty("sms")]
    public int? Sms { get; set; }

    /// <summary>
    /// The number of SMS subscribers found by the query.
    /// </summary>
    [JsonProperty("webhook")]
    public int? Webhook { get; set; }

    /// <summary>
    /// The number of integration partners found by the query.
    /// </summary>
    [JsonProperty("integration_partner")]
    public int? IntegrationPartner { get; set; }

    /// <summary>
    /// The number of Slack subscribers found by the query.
    /// </summary>
    [JsonProperty("slack")]
    public int? Slack { get; set; }

    /// <summary>
    /// The number of MS teams subscribers found by the query.
    /// </summary>
    [JsonProperty("teams")]
    public int? Teams { get; set; }
  }
}