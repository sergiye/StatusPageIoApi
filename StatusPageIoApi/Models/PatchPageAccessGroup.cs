using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Update a page access group
  /// </summary>
  public class PatchPageAccessGroup {

    [JsonProperty("page_access_group")]
    public PageAccessGroup PageAccessGroup { get; set; }
  }
}