using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Update a metric
  /// </summary>
  public class PutMetrics {

    [JsonProperty("metric")]
    public Metric Metric { get; set; }
  }
}