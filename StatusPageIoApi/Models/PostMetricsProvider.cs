using Newtonsoft.Json;

namespace StatusPageIoApi {
  /// <summary>
  /// Create a metric provider
  /// </summary>
  public class PostMetricsProvider {

    [JsonProperty("metrics_provider")]
    public MetricsProvider MetricsProvider { get; set; }
  }
}