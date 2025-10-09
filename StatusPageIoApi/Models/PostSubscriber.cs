using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Create a subscriber. Not applicable for Slack subscribers.
  /// </summary>
  public class PostSubscriber {

    [JsonProperty("subscriber")]
    public Subscriber Subscriber { get; set; }
  }
}