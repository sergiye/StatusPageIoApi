using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Get a user's permissions
  /// </summary>
  public class Permissions {

    [JsonProperty("data")]
    public Data3 Data { get; set; }
  }
}