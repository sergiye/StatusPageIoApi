using Newtonsoft.Json;

namespace StatusPageIoApi {

  public class EditComponentIds {

    /// <summary>
    /// List of components codes to remove.  If omitted, all components will be removed.
    /// </summary>
    [JsonProperty("component_ids")]
    public string[] ComponentIds { get; set; }
  }
}