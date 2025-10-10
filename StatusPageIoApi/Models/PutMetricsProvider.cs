using Newtonsoft.Json;

namespace StatusPageIoApi {
  /// <summary>
  /// Update a metric provider
  /// </summary>
  public class PutMetricsProvider {

    [JsonProperty("metrics_provider")]
    public MetricsProvider MetricsProvider { get; set; }
  }
}