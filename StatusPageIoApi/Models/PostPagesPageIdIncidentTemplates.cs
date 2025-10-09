using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Create a template
  /// </summary>
  public class PostPagesPageIdIncidentTemplates {

    [JsonProperty("template")]
    public Template Template { get; set; }
  }
}