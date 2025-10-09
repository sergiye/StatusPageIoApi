using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Update an incident
  /// </summary>
  public class PutIncident {

    [JsonProperty("incident")]
    public Incident Incident { get; set; }
  }
}