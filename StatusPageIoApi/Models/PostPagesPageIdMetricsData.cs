using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Add data points to metrics
  /// </summary>
  public class PostPagesPageIdMetricsData {

    [JsonProperty("data", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public MetricAddResponse Data { get; set; } = new MetricAddResponse();
  }
}