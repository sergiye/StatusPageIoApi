using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  public class Data3 {

    /// <summary>
    /// User identifier
    /// </summary>
    [JsonProperty("user_id")]
    public string UserId { get; set; }

    /// <summary>
    /// Pages accessible by the user.
    /// </summary>
    [JsonProperty("pages")]
    public Pages Pages { get; set; }
  }
}