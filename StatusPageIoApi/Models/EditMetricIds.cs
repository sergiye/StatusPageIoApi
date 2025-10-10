using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {

  public class EditMetricIds {

    /// <summary>
    /// List of metrics to add
    /// </summary>
    [JsonProperty("metric_ids")]
    public string[] MetricIds { get; set; }
  }
}