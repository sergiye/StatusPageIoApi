using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Update an incident
  /// </summary>
  public class PutPagesPageIdIncidents {

    [JsonProperty("incident")]
    public Incident Incident { get; set; }
  }
}