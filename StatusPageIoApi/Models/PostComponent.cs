using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Create a component
  /// </summary>
  public class PostComponent {

    [JsonProperty("component")]
    public EditComponent Component { get; set; }
  }
}