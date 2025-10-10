using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Update a component group
  /// </summary>
  public class PutComponentGroup {

    /// <summary>
    /// Updated description of the component group.
    /// </summary>
    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("component_group")]
    public EditComponentGroup EditComponentGroup { get; set; }
  }
}