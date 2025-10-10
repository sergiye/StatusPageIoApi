using Newtonsoft.Json;

namespace StatusPageIoApi {
  /// <summary>
  /// Create a metric for a metric provider
  /// </summary>
  public class PostMetricsProviderMetric {

    [JsonProperty("metric")]
    public Metric Metric { get; set; }
  }
}