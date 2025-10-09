using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Create an incident
  /// </summary>
  public class PostIncident {

    [JsonProperty("incident")]
    public Incident Incident { get; set; }
  }
}