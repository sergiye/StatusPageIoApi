using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Create a component group
  /// </summary>
  public class PostComponentGroup {

    /// <summary>
    /// Description of the component group.
    /// </summary>
    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("component_group")]
    public EditComponentGroup EditComponentGroup { get; set; }
  }
}