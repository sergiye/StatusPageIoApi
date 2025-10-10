using Newtonsoft.Json;

namespace StatusPageIoApi {
  /// <summary>
  /// Create an incident subscriber
  /// </summary>
  public class PostIncidentSubscriber {

    [JsonProperty("subscriber")]
    public IncidentSubscriber Subscriber { get; set; }
  }
}