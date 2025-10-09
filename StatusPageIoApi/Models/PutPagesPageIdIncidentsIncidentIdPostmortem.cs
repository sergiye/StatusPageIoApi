using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Create Postmortem
  /// </summary>
  public class PutPagesPageIdIncidentsIncidentIdPostmortem {

    [JsonProperty("postmortem")]
    public Postmortem2 Postmortem { get; set; }
  }
}