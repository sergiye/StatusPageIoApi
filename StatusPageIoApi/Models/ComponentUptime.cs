using System;
using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {

  public class ComponentUptime {

    /// <summary>
    /// Start date used for uptime calculation (see the warnings field in the response if this value does not match the start parameter you provided).
    /// </summary>
    [JsonProperty("range_start")]
    public DateTimeOffset? RangeStart { get; set; }

    /// <summary>
    /// End date used for uptime calculation (see the warnings field in the response if this value does not match the end parameter you provided).
    /// </summary>
    [JsonProperty("range_end")]
    public DateTimeOffset? RangeEnd { get; set; }

    /// <summary>
    /// Uptime percentage for a component
    /// </summary>
    [JsonProperty("uptime_percentage")]
    public float? UptimePercentage { get; set; }

    /// <summary>
    /// Seconds of major outage
    /// </summary>
    [JsonProperty("major_outage")]
    public int? MajorOutage { get; set; }

    /// <summary>
    /// Seconds of partial outage
    /// </summary>
    [JsonProperty("partial_outage")]
    public int? PartialOutage { get; set; }

    /// <summary>
    /// Warning messages related to the uptime query that may occur
    /// </summary>
    [JsonProperty("warnings")]
    public string Warnings { get; set; }

    /// <summary>
    /// Component identifier
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// Component display name
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Related incidents
    /// </summary>
    [JsonProperty("related_events")]
    public RelatedEvents RelatedEvents { get; set; }
  }
}