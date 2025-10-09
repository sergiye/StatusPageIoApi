using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Update a previous incident update
  /// </summary>
  public class PatchIncidentUpdate {

    [JsonProperty("incident_update")]
    public IncidentUpdate IncidentUpdate { get; set; }
  }
}