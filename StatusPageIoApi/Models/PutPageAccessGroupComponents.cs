using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Add components to page access group
  /// </summary>
  public class PutPageAccessGroupComponents {

    /// <summary>
    /// List of Component identifiers
    /// </summary>
    [JsonProperty("component_ids")]
    public string[] ComponentIds { get; set; }
  }
}