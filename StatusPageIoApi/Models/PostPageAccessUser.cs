using Newtonsoft.Json;

namespace StatusPageIoApi {
  /// <summary>
  /// Add a page access user
  /// </summary>
  public class PostPageAccessUser {

    [JsonProperty("page_access_user")]
    public PageAccessUser PageAccessUser { get; set; }
  }
}