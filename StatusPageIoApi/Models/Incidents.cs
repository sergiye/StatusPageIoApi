using Newtonsoft.Json;

namespace StatusPageIoApi {
  public class Incidents {

    /// <summary>
    /// Incident identifier
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }
  }
}