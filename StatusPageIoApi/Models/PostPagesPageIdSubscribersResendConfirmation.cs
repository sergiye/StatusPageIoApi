using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Resend confirmations to a list of subscribers
  /// </summary>
  public class PostPagesPageIdSubscribersResendConfirmation {

    /// <summary>
    /// The array of subscriber codes to resend confirmations for, or "all" to resend confirmations to all subscribers. Only unconfirmed email subscribers will receive this notification.
    /// </summary>
    [JsonProperty("subscribers", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Subscribers { get; set; }
  }
}