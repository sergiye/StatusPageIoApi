using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Update a metric
  /// </summary>
  public class PutPagesPageIdMetrics {

    [JsonProperty("metric")]
    public Metric Metric { get; set; }
  }
}