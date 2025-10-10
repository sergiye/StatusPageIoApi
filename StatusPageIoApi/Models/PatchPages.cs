using Newtonsoft.Json;

namespace StatusPageIoApi {
  /// <summary>
  /// Update a page
  /// </summary>
  public class PatchPages {

    [JsonProperty("page")]
    public Page Page { get; set; }
  }
}