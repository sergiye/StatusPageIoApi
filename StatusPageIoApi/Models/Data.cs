using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  public class Data {

    /// <summary>
    /// Time to store the metric against
    /// </summary>
    [JsonProperty("timestamp")]
    public int Timestamp { get; set; }

    [JsonProperty("value")]
    public float Value { get; set; }
  }
}