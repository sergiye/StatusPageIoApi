using Newtonsoft.Json;

namespace StatusPageIoApi {
  /// <summary>
  /// Resend confirmations to a list of subscribers
  /// </summary>
  public class PostSubscribersResendConfirmation {

    /// <summary>
    /// The array of subscriber codes to resend confirmations for, or "all" to resend confirmations to all subscribers. Only unconfirmed email subscribers will receive this notification.
    /// </summary>
    [JsonProperty("subscribers", Required = Required.Always)]
    public string Subscribers { get; set; }
  }
}