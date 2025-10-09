using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Create an incident subscriber
  /// </summary>
  public class PostPagesPageIdIncidentsIncidentIdSubscribers {

    [JsonProperty("subscriber")]
    public Subscriber3 Subscriber { get; set; }
  }
}