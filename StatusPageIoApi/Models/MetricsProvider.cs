using System;
using Newtonsoft.Json;

namespace StatusPageIoApi {

  public class MetricsProvider {

    /// <summary>
    /// Identifier for Metrics Provider
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// One of "Pingdom", "NewRelic", "Librato", "Datadog", or "Self"
    /// </summary>
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("disabled")]
    public bool? Disabled { get; set; }

    [JsonProperty("metric_base_uri")]
    public string MetricBaseUri { get; set; }

    [JsonProperty("last_revalidated_at")]
    public DateTimeOffset? LastRevalidatedAt { get; set; }

    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }

    [JsonProperty("page_id")]
    public int? PageId { get; set; }

    /// <summary>
    /// Required by the Librato metrics provider.
    /// </summary>
    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("password")]
    public string Password { get; set; }

    /// <summary>
    /// Required by the Datadog and NewRelic type metrics providers.
    /// </summary>
    [JsonProperty("api_key")]
    public string ApiKey { get; set; }

    /// <summary>
    /// Required by the Librato, Datadog and Pingdom type metrics providers.
    /// </summary>
    [JsonProperty("api_token")]
    public string ApiToken { get; set; }

    /// <summary>
    /// Required by the Pingdom-type metrics provider.
    /// </summary>
    [JsonProperty("application_key")]
    public string ApplicationKey { get; set; }
  }
}