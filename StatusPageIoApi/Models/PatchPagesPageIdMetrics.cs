using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Update a metric
  /// </summary>
  public class PatchPagesPageIdMetrics {

    [JsonProperty("metric")]
    public Metric Metric { get; set; }
  }
}