using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Update a component
  /// </summary>
  public class PatchComponent {

    [JsonProperty("component")]
    public EditComponent Component { get; set; }
  }
}