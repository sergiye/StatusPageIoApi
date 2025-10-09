using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Update a page
  /// </summary>
  public class PutPages {

    [JsonProperty("page")]
    public Page Page { get; set; }
  }
}