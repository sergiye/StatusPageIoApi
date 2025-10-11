using System;
using Newtonsoft.Json;

namespace StatusPageIoApi {

  public class Subscriber {

    /// <summary>
    /// Subscriber Identifier
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// If this is true, do not notify the user with changes to their subscription.
    /// </summary>
    [JsonProperty("skip_confirmation_notification")]
    public bool? SkipConfirmationNotification { get; set; }

    /// <summary>
    /// The communication mode of the subscriber.
    /// </summary>
    [JsonProperty("mode")]
    public string Mode { get; set; }

    /// <summary>
    /// The email address to use to contact the subscriber. Used for Email and Webhook subscribers.
    /// </summary>
    [JsonProperty("email")]
    public string Email { get; set; }

    /// <summary>
    /// The URL where a webhook subscriber elects to receive updates.
    /// </summary>
    [JsonProperty("endpoint")]
    public string Endpoint { get; set; }

    /// <summary>
    /// The phone number used to contact an SMS subscriber
    /// </summary>
    [JsonProperty("phone_number")]
    public string PhoneNumber { get; set; }

    /// <summary>
    /// The two-character country code representing the country of which the phone_number is a part.
    /// </summary>
    [JsonProperty("phone_country")]
    public string PhoneCountry { get; set; }

    /// <summary>
    /// A formatted version of the phone_number and phone_country pair, nicely formatted for display.
    /// </summary>
    [JsonProperty("display_phone_number")]
    public string DisplayPhoneNumber { get; set; }

    /// <summary>
    /// Obfuscated slack channel name
    /// </summary>
    [JsonProperty("obfuscated_channel_name")]
    public string ObfuscatedChannelName { get; set; }

    /// <summary>
    /// The workspace name of the slack subscriber.
    /// </summary>
    [JsonProperty("workspace_name")]
    public string WorkspaceName { get; set; }

    /// <summary>
    /// The timestamp when the subscriber was quarantined due to an issue reaching them.
    /// </summary>
    [JsonProperty("quarantined_at")]
    public DateTimeOffset? QuarantinedAt { get; set; }

    /// <summary>
    /// The timestamp when a quarantined subscriber will be purged (unsubscribed).
    /// </summary>
    [JsonProperty("purge_at")]
    public DateTimeOffset? PurgeAt { get; set; }

    /// <summary>
    /// The components for which the subscriber has elected to receive updates.
    /// </summary>
    [JsonProperty("components")]
    public string[] Components { get; set; }

    /// <summary>
    /// The Page Access user this subscriber belongs to (only for audience-specific pages).
    /// </summary>
    [JsonProperty("page_access_user_id")]
    public string PageAccessUserId { get; set; }

    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }
  }
  
  public class CreateSubscriber {

    /// <summary>
    /// The email address to use to contact the subscriber. Used for Email and Webhook subscribers.
    /// </summary>
    [JsonProperty("email")]
    public string Email { get; set; }

    /// <summary>
    /// The URL where a webhook subscriber elects to receive updates.
    /// </summary>
    [JsonProperty("endpoint")]
    public string Endpoint { get; set; }

    /// <summary>
    /// The two-character country code representing the country of which the phone_number is a part.
    /// </summary>
    [JsonProperty("phone_country")]
    public string PhoneCountry { get; set; }

    /// <summary>
    /// The phone number used to contact an SMS subscriber
    /// </summary>
    [JsonProperty("phone_number")]
    public string PhoneNumber { get; set; }

    /// <summary>
    /// If this is true, do not notify the user with changes to their subscription.
    /// </summary>
    [JsonProperty("skip_confirmation_notification")]
    public bool? SkipConfirmationNotification { get; set; }

    /// <summary>
    /// The code of the page access user to which the subscriber belongs.
    /// </summary>
    [JsonProperty("page_access_user")]
    public string PageAccessUser { get; set; }

    /// <summary>
    /// A list of component ids for which the subscriber should recieve updates for. Components must be an array with at least one element if it is passed at all. Each component must belong to the page indicated in the path.
    /// </summary>
    [JsonProperty("component_ids")]
    public string[] ComponentIds { get; set; }
  }
  
  /// <summary>
  /// Create a subscriber. Not applicable for Slack subscribers.
  /// </summary>
  public class PostSubscriber {

    [JsonProperty("subscriber")]
    public CreateSubscriber Subscriber { get; set; }
  }
}