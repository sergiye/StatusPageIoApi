using System;
using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Add page access groups to a component
  /// </summary>
  public class Component {

    /// <summary>
    /// Identifier for component
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// Page identifier
    /// </summary>
    [JsonProperty("page_id")]
    public string PageId { get; set; }

    /// <summary>
    /// Component Group identifier
    /// </summary>
    [JsonProperty("group_id")]
    public string GroupId { get; set; }

    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// Is this component a group
    /// </summary>
    [JsonProperty("group")]
    public bool? Group { get; set; }

    /// <summary>
    /// Display name for component
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// More detailed description for component
    /// </summary>
    [JsonProperty("description")]
    public string Description { get; set; }

    /// <summary>
    /// Order the component will appear on the page
    /// </summary>
    [JsonProperty("position")]
    public int? Position { get; set; }

    /// <summary>
    /// Status of component
    /// </summary>
    [JsonProperty("status")]
    public ComponentStatus? Status { get; set; }

    /// <summary>
    /// Should this component be showcased
    /// </summary>
    [JsonProperty("showcase")]
    public bool? Showcase { get; set; }

    /// <summary>
    /// Requires a special feature flag to be enabled
    /// </summary>
    [JsonProperty("only_show_if_degraded")]
    public bool? OnlyShowIfDegraded { get; set; }

    /// <summary>
    /// Requires a special feature flag to be enabled
    /// </summary>
    [JsonProperty("automation_email")]
    public string AutomationEmail { get; set; }

    /// <summary>
    /// The date this component started being used
    /// </summary>
    [JsonProperty("start_date")]
    public DateTimeOffset? StartDate { get; set; }
  }
}