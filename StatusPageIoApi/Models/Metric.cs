using System;
using Newtonsoft.Json;

namespace StatusPageIoApi {

  public class Metric {

    /// <summary>
    /// Metric identifier
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// Metric Provider identifier
    /// </summary>
    [JsonProperty("metrics_provider_id")]
    public string MetricsProviderId { get; set; }

    /// <summary>
    /// Metric Display identifier used to look up the metric data from the provider
    /// </summary>
    [JsonProperty("metric_identifier")]
    public string MetricIdentifier { get; set; }

    /// <summary>
    /// Name of metric
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Should the metric be displayed
    /// </summary>
    [JsonProperty("display")]
    public bool? Display { get; set; }

    [JsonProperty("tooltip_description")]
    public string TooltipDescription { get; set; }

    [JsonProperty("backfilled")]
    public bool? Backfilled { get; set; }

    [JsonProperty("y_axis_min")]
    public float? YAxisMin { get; set; }

    [JsonProperty("y_axis_max")]
    public float? YAxisMax { get; set; }

    /// <summary>
    /// Should the values on the y axis be hidden on render
    /// </summary>
    [JsonProperty("y_axis_hidden")]
    public bool? YAxisHidden { get; set; }

    /// <summary>
    /// Suffix to describe the units on the graph
    /// </summary>
    [JsonProperty("suffix")]
    public string Suffix { get; set; }

    [JsonProperty("decimal_places")]
    public int? DecimalPlaces { get; set; }

    [JsonProperty("most_recent_data_at")]
    public DateTimeOffset? MostRecentDataAt { get; set; }

    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }

    [JsonProperty("last_fetched_at")]
    public DateTimeOffset? LastFetchedAt { get; set; }

    [JsonProperty("backfill_percentage")]
    public int? BackfillPercentage { get; set; }

    [JsonProperty("reference_name")]
    public string ReferenceName { get; set; }
  }

  public class EditMetric {
    
    /// <summary>
    /// Name of metric
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Metric Display identifier used to look up the metric data from the provider
    /// </summary>
    [JsonProperty("metric_identifier")]
    public string MetricIdentifier { get; set; }
  }
  
  /// <summary>
  /// Update a metric
  /// </summary>
  public class EditMetricBody {

    [JsonProperty("metric")]
    public EditMetric Metric { get; set; }
  }

  public class CreateMetricsProviderMetric {
    
    /// <summary>
    /// Name of metric
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Metric Display identifier used to look up the metric data from the provider
    /// </summary>
    [JsonProperty("metric_identifier")]
    public string MetricIdentifier { get; set; }
    
    /// <summary>
    /// The transform to apply to metric before pulling into Statuspage. One of: "average", "count", "max", "min", or "sum"
    /// </summary>
    [JsonProperty("transform")]
    public string Transform { get; set; }

    /// <summary>
    /// The Identifier for new relic application. Required in the case of NewRelic only
    /// </summary>
    [JsonProperty("application_id")]
    public string ApplicationId { get; set; }

    /// <summary>
    /// Suffix to describe the units on the graph
    /// </summary>
    [JsonProperty("suffix")]
    public string Suffix { get; set; }

    [JsonProperty("y_axis_min")]
    public float? YAxisMin { get; set; }

    [JsonProperty("y_axis_max")]
    public float? YAxisMax { get; set; }

    /// <summary>
    /// Should the values on the y axis be hidden on render
    /// </summary>
    [JsonProperty("y_axis_hidden")]
    public bool? YAxisHidden { get; set; }

    /// <summary>
    /// Should the metric be displayed
    /// </summary>
    [JsonProperty("display")]
    public bool? Display { get; set; }

    [JsonProperty("decimal_places")]
    public int? DecimalPlaces { get; set; }

    [JsonProperty("tooltip_description")]
    public string TooltipDescription { get; set; }
  }

  public class CreateMetricsProviderMetricBody {

    [JsonProperty("metric")]
    public CreateMetricsProviderMetric Metric { get; set; }
  }
  
  public class MetricData {

    /// <summary>
    /// Time to store the metric against
    /// </summary>
    [JsonProperty("timestamp")]
    public int? Timestamp { get; set; }

    [JsonProperty("value")]
    public float? Value { get; set; }
  }

  /// <summary>
  /// Add data to a metric
  /// </summary>
  public class PostMetricData {

    [JsonProperty("data")]
    public MetricData Data { get; set; }
  }

  /// <summary>
  /// Add data points to metrics
  /// </summary>
  public class MetricAddResponse {

    /// <summary>
    /// Metric identifier to add data to
    /// </summary>
    [JsonProperty("metric_id")]
    public MetricData[] MetricId { get; set; }
  }

  /// <summary>
  /// Add data points to metrics
  /// </summary>
  public class PostMetricsData {

    [JsonProperty("data")]
    public MetricAddResponse Data { get; set; }
  }

  public class EditMetricIds {

    /// <summary>
    /// List of metrics to add
    /// </summary>
    [JsonProperty("metric_ids")]
    public string[] MetricIds { get; set; }
  }
}