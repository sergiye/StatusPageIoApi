using Newtonsoft.Json;

namespace StatusPageIoApi {
  public class MetricData {

    /// <summary>
    /// Time to store the metric against
    /// </summary>
    [JsonProperty("timestamp")]
    public int? Timestamp { get; set; }

    [JsonProperty("value")]
    public float? Value { get; set; }
  }
}