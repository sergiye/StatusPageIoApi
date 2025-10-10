using Newtonsoft.Json;

namespace StatusPageIoApi {

  public class ErrorEntity {

    [JsonProperty("message")]
    public string Message { get; set; }
  }
}