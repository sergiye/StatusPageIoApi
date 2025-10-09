using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  public class RelatedEvents {

    /// <summary>
    /// Incident identifier
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// Component identifier
    /// </summary>
    [JsonProperty("component_id")]
    public string ComponentId { get; set; }

    /// <summary>
    /// Related incidents
    /// </summary>
    [JsonProperty("incidents")]
    public Incidents Incidents { get; set; }
  }
}