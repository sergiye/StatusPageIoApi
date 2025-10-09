using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Add a page access user
  /// </summary>
  public class PostPagesPageIdPageAccessUsers {

    [JsonProperty("page_access_user")]
    public PageAccessUser PageAccessUser { get; set; }
  }
}