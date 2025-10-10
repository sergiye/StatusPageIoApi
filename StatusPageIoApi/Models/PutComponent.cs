using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Update a component
  /// </summary>
  public class PutComponent {

    [JsonProperty("component")]
    public EditComponent Component { get; set; }
  }
}