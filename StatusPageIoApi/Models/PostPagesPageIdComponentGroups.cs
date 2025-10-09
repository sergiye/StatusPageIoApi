using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Create a component group
  /// </summary>
  public class PostPagesPageIdComponentGroups {

    /// <summary>
    /// Description of the component group.
    /// </summary>
    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("component_group")]
    public ComponentGroup ComponentGroup { get; set; }
  }
}