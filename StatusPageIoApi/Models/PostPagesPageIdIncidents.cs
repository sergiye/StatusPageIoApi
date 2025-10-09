using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Create an incident
  /// </summary>
  public class PostPagesPageIdIncidents {

    [JsonProperty("incident")]
    public Incident Incident { get; set; }
  }
}