using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Update a metric provider
  /// </summary>
  public class PatchMetricsProvider {

    [JsonProperty("metrics_provider")]
    public MetricsProvider MetricsProvider { get; set; }
  }
}