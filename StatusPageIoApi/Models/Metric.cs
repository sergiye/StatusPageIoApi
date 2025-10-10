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
  }
}