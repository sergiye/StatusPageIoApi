using Newtonsoft.Json;

namespace StatusPageIoApi {
  /// <summary>
  /// Update a metric
  /// </summary>
  public class PutMetrics {

    [JsonProperty("metric")]
    public Metric Metric { get; set; }
  }
}