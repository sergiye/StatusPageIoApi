using Newtonsoft.Json;

namespace StatusPageIoApi {
  /// <summary>
  /// Update a page access group
  /// </summary>
  public class PutPageAccessGroup {

    [JsonProperty("page_access_group")]
    public PageAccessGroup PageAccessGroup { get; set; }
  }
}