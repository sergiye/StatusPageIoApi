using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SergiyE.StatusPageIoApi {
  public class Template {

    /// <summary>
    /// Name of the template, as shown in the list on the "Templates" tab of the "Incidents" page
    /// </summary>
    [JsonProperty("name", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Name { get; set; }

    /// <summary>
    /// Title to be applied to the incident or maintenance when selecting this template
    /// </summary>
    [JsonProperty("title", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Title { get; set; }

    /// <summary>
    /// The initial message, created as the first incident or maintenance update.
    /// </summary>
    [JsonProperty("body", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
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
    public TemplateUpdateStatus? UpdateStatus { get; set; }

    /// <summary>
    /// Whether the "tweet update" checkbox should be selected when selecting this template
    /// </summary>
    [JsonProperty("should_tweet")]
    public bool? ShouldTweet { get; set; }

    /// <summary>
    /// Whether the "deliver notifications" checkbox should be selected when selecting this template
    /// </summary>
    [JsonProperty("should_send_notifications")]
    public bool? ShouldSendNotifications { get; set; }

    /// <summary>
    /// List of component_ids affected by this incident
    /// </summary>
    [JsonProperty("component_ids")]
    public string[] ComponentIds { get; set; }
  }
}