using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  public class MetricId {

    [JsonProperty("timestamp")]
    public int Timestamp { get; set; }

    [JsonProperty("value")]
    public float Value { get; set; }
  }
}