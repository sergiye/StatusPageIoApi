using System.Collections.Generic;
using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Replace metrics for page access user
  /// </summary>
  public class PostPageAccessUserMetrics {

    /// <summary>
    /// List of metrics to add
    /// </summary>
    [JsonProperty("metric_ids", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public ICollection<string> MetricIds { get; set; } =
      new System.Collections.ObjectModel.Collection<string>();
  }
}