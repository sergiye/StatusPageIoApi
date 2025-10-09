using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Add data points to metrics
  /// </summary>
  public class MetricAddResponse {

    /// <summary>
    /// Metric identifier to add data to
    /// </summary>
    [JsonProperty("metric_id")]
    public MetricId[] MetricId { get; set; }
  }
}