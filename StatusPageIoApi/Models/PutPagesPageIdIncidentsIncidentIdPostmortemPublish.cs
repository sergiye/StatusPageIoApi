using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Publish Postmortem
  /// </summary>
  public class PutPagesPageIdIncidentsIncidentIdPostmortemPublish {

    [JsonProperty("postmortem")]
    public Postmortem3 Postmortem { get; set; }
  }
}