using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace StatusPageIoApi {

  public class IncidentUpdate {

    /// <summary>
    /// Incident Update Identifier.
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// Incident Identifier.
    /// </summary>
    [JsonProperty("incident_id")]
    public string IncidentId { get; set; }

    /// <summary>
    /// Affected components associated with the incident update.
    /// </summary>
    [JsonProperty("affected_components")]
    public object[] AffectedComponents { get; set; }

    /// <summary>
    /// Incident update body.
    /// </summary>
    [JsonProperty("body")]
    public string Body { get; set; }

    /// <summary>
    /// The timestamp when the incident update was created at.
    /// </summary>
    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    /// An optional customized tweet message for incident postmortem.
    /// </summary>
    [JsonProperty("custom_tweet")]
    public string CustomTweet { get; set; }

    /// <summary>
    /// Controls whether to delivery notifications.
    /// </summary>
    [JsonProperty("deliver_notifications")]
    public bool? DeliverNotifications { get; set; }

    /// <summary>
    /// Timestamp when incident update is happened.
    /// </summary>
    [JsonProperty("display_at")]
    public DateTimeOffset? DisplayAt { get; set; }

    /// <summary>
    /// The incident status. For realtime incidents, valid values are investigating, identified, monitoring, and resolved. For scheduled incidents, valid values are scheduled, in_progress, verifying, and completed.
    /// </summary>
    [JsonProperty("status")]
    [JsonConverter(typeof(StringEnumConverter))]
    public IncidentStatus? Status { get; set; }

    /// <summary>
    /// Tweet identifier associated to this incident update.
    /// </summary>
    [JsonProperty("tweet_id")]
    public string TweetId { get; set; }

    /// <summary>
    /// The timestamp when twitter updated at.
    /// </summary>
    [JsonProperty("twitter_updated_at")]
    public DateTimeOffset? TwitterUpdatedAt { get; set; }

    /// <summary>
    /// The timestamp when the incident update is updated.
    /// </summary>
    [JsonProperty("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// Controls whether to create twitter update.
    /// </summary>
    [JsonProperty("wants_twitter_update")]
    public bool? WantsTwitterUpdate { get; set; }
  }
  
  public class EditIncidentUpdate {

    /// <summary>
    /// Controls whether to create twitter update.
    /// </summary>
    [JsonProperty("wants_twitter_update")]
    public bool? WantsTwitterUpdate { get; set; }

    /// <summary>
    /// Incident update body.
    /// </summary>
    [JsonProperty("body")]
    public string Body { get; set; }

    /// <summary>
    /// Timestamp when incident update is happened.
    /// </summary>
    [JsonProperty("display_at")]
    public DateTimeOffset? DisplayAt { get; set; }

    /// <summary>
    /// Controls whether to delivery notifications.
    /// </summary>
    [JsonProperty("deliver_notifications")]
    public bool? DeliverNotifications { get; set; }
  }
  
  public class EditIncidentUpdateBody {

    [JsonProperty("incident_update")]
    public EditIncidentUpdate IncidentUpdate { get; set; }
  }
}