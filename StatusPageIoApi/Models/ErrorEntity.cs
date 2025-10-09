using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {

  public class ErrorEntity {

    [JsonProperty("message")]
    public string Message { get; set; }
  }
}