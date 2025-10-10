using Newtonsoft.Json;

namespace StatusPageIoApi {
  /// <summary>
  /// Add data to a metric
  /// </summary>
  public class PostMetricData {

    [JsonProperty("data", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public MetricData Data { get; set; } = new MetricData();
  }
}