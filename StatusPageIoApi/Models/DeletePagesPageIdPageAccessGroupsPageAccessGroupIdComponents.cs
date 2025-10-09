using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Delete components for a page access group
  /// </summary>
  public class DeletePagesPageIdPageAccessGroupsPageAccessGroupIdComponents {

    [JsonProperty("component_ids")]
    public string[] ComponentIds { get; set; }
  }
}