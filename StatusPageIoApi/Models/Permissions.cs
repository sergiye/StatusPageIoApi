using Newtonsoft.Json;

namespace StatusPageIoApi {

  public class Permissions {

    [JsonProperty("data")]
    public PermissionsData Data { get; set; }
  }

  public class PermissionsData {

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