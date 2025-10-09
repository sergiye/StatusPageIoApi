using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Add data to a metric
  /// </summary>
  public class SingleMetricAddResponse {

    [JsonProperty("data")]
    public Data Data { get; set; }
  }
}