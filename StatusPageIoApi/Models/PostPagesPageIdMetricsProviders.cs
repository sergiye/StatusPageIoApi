using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Create a metric provider
  /// </summary>
  public class PostPagesPageIdMetricsProviders {

    [JsonProperty("metrics_provider")]
    public MetricsProvider MetricsProvider { get; set; }
  }
}