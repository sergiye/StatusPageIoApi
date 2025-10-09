using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Get a list of templates
  /// </summary>
  public class IncidentTemplate {

    /// <summary>
    /// Incident Template Identifier
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// Affected components
    /// </summary>
    [JsonProperty("components")]
    public Component[] Components { get; set; }

    /// <summary>
    /// Name of the template, as shown in the list on the "Templates" tab of the "Incidents" page
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Title to be applied to the incident or maintenance when selecting this template
    /// </summary>
    [JsonProperty("title")]
    public string Title { get; set; }

    /// <summary>
    /// Body of the incident or maintenance update to be applied when selecting this template
    /// </summary>
    [JsonProperty("body")]
    public string Body { get; set; }

    /// <summary>
    /// Identifier of Template Group this template belongs to
    /// </summary>
    [JsonProperty("group_id")]
    public string GroupId { get; set; }

    /// <summary>
    /// The status the incident or maintenance should transition to when selecting this template
    /// </summary>
    [JsonProperty("update_status")]
    [JsonConverter(typeof(StringEnumConverter))]
    public IncidentTemplateUpdateStatus UpdateStatus { get; set; }

    /// <summary>
    /// Whether the "tweet update" checkbox should be selected when selecting this template
    /// </summary>
    [JsonProperty("should_tweet")]
    public bool ShouldTweet { get; set; }

    /// <summary>
    /// Whether the "deliver notifications" checkbox should be selected when selecting this template
    /// </summary>
    [JsonProperty("should_send_notifications")]
    public bool ShouldSendNotifications { get; set; }
  }
}