using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Create a component
  /// </summary>
  public class PostPagesPageIdComponents {

    [JsonProperty("component")]
    public Component Component { get; set; }
  }
}