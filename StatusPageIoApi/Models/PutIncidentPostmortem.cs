using Newtonsoft.Json;

namespace StatusPageIoApi {
  /// <summary>
  /// Create Postmortem
  /// </summary>
  public class PutIncidentPostmortem {

    [JsonProperty("postmortem")]
    public Postmortem2 Postmortem { get; set; }
  }
}