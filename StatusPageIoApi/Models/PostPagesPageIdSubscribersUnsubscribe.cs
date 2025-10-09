using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Unsubscribe a list of subscribers
  /// </summary>
  public class PostPagesPageIdSubscribersUnsubscribe {

    /// <summary>
    /// The array of subscriber codes to unsubscribe (limited to 100), or "all" to unsubscribe all subscribers if the number of subscribers is less than 100.
    /// </summary>
    [JsonProperty("subscribers", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Subscribers { get; set; }

    /// <summary>
    /// If this is present, only unsubscribe subscribers of this type.
    /// </summary>
    [JsonProperty("type")]
    [JsonConverter(typeof(StringEnumConverter))]
    public PostPagesPageIdSubscribersUnsubscribeType Type { get; set; }

    /// <summary>
    /// If this is present, only unsubscribe subscribers in this state. Specify state "all" to unsubscribe subscribers in any states.
    /// </summary>
    [JsonProperty("state")]
    [JsonConverter(typeof(StringEnumConverter))]
    public PostPagesPageIdSubscribersUnsubscribeState State { get; set; } =
      PostPagesPageIdSubscribersUnsubscribeState.Active;

    /// <summary>
    /// If skip_unsubscription_notification is true, the subscribers do not receive any notifications when they are unsubscribed.
    /// </summary>
    [JsonProperty("skip_unsubscription_notification")]
    public bool SkipUnsubscriptionNotification { get; set; }
  }
}