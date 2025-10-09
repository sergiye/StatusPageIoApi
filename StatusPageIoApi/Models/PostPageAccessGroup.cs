using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Create a page access group
  /// </summary>
  public class PostPageAccessGroup {

    [JsonProperty("page_access_group")]
    public PageAccessGroup PageAccessGroup { get; set; }
  }
}