using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Delete metrics for page access user
  /// </summary>
  public class DeletePagesPageIdPageAccessUsersPageAccessUserIdMetrics {

    /// <summary>
    /// List of metrics to remove
    /// </summary>
    [JsonProperty("metric_ids")]
    public string[] MetricIds { get; set; }
  }
}