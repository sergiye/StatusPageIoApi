using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Remove components for page access user
  /// </summary>
  public class DeletePageAccessUserComponents {

    /// <summary>
    /// List of components codes to remove.  If omitted, all components will be removed.
    /// </summary>
    [JsonProperty("component_ids")]
    public string[] ComponentIds { get; set; }
  }
}