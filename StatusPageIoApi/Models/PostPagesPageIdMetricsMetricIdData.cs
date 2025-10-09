using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Add data to a metric
  /// </summary>
  public class PostPagesPageIdMetricsMetricIdData {

    [JsonProperty("data", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public Data Data { get; set; } = new Data();
  }
}