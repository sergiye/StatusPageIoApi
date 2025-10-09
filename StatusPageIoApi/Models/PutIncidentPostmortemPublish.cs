using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Publish Postmortem
  /// </summary>
  public class PutIncidentPostmortemPublish {

    [JsonProperty("postmortem")]
    public PostmortemPublish Postmortem { get; set; }
  }
}