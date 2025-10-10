using Newtonsoft.Json;

namespace StatusPageIoApi {
  /// <summary>
  /// Update a page
  /// </summary>
  public class PutPages {

    [JsonProperty("page")]
    public Page Page { get; set; }
  }
}