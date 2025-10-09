using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Get a list of users
  /// </summary>
  public class ErrorEntity {

    [JsonProperty("message")]
    public string Message { get; set; }
  }
}