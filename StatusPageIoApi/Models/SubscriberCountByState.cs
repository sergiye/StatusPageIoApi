using Newtonsoft.Json;

namespace StatusPageIoApi {
  public class SubscriberCountByState {

    /// <summary>
    /// The number of active subscribers found by the query.
    /// </summary>
    [JsonProperty("active")]
    public int? Active { get; set; }

    /// <summary>
    /// The number of unconfirmed subscribers found by the query.
    /// </summary>
    [JsonProperty("unconfirmed")]
    public int? Unconfirmed { get; set; }

    /// <summary>
    /// The number of quarantined subscribers found by the query.
    /// </summary>
    [JsonProperty("quarantined")]
    public int? Quarantined { get; set; }

    /// <summary>
    /// The total number of subscribers found by the query.
    /// </summary>
    [JsonProperty("total")]
    public int? Total { get; set; }
  }
}