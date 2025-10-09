using System.Collections.Generic;
using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Add metrics for page access user
  /// </summary>
  public class PutPagesPageIdPageAccessUsersPageAccessUserIdMetrics {

    /// <summary>
    /// List of metrics to add
    /// </summary>
    [JsonProperty("metric_ids", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public ICollection<string> MetricIds { get; set; } =
      new System.Collections.ObjectModel.Collection<string>();
  }
}