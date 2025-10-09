using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Update a page access group
  /// </summary>
  public class PutPagesPageIdPageAccessGroups {

    [JsonProperty("page_access_group")]
    public PageAccessGroup PageAccessGroup { get; set; }
  }
}