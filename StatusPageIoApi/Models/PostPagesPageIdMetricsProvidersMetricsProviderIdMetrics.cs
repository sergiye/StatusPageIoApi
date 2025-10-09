using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Create a metric for a metric provider
  /// </summary>
  public class PostPagesPageIdMetricsProvidersMetricsProviderIdMetrics {

    [JsonProperty("metric")]
    public Metric Metric { get; set; }
  }
}