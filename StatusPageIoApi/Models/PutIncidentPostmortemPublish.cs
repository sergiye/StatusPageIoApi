using Newtonsoft.Json;

namespace StatusPageIoApi {
  /// <summary>
  /// Publish Postmortem
  /// </summary>
  public class PutIncidentPostmortemPublish {

    [JsonProperty("postmortem")]
    public PostmortemPublish Postmortem { get; set; }
  }
}