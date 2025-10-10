using Newtonsoft.Json;

namespace StatusPageIoApi {
  /// <summary>
  /// Update a previous incident update
  /// </summary>
  public class PutIncidentUpdates {

    [JsonProperty("incident_update")]
    public IncidentUpdate IncidentUpdate { get; set; }
  }
}