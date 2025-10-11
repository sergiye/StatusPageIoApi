using Newtonsoft.Json;

namespace StatusPageIoApi {

  public class EditComponentIds {

    /// <summary>
    /// List of components codes
    /// </summary>
    [JsonProperty("component_ids")]
    public string[] ComponentIds { get; set; }
  }
}