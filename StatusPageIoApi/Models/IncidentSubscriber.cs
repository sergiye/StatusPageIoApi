using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  public class IncidentSubscriber {

    /// <summary>
    /// The email address for creating Email subscribers.
    /// </summary>
    [JsonProperty("email")]
    public string Email { get; set; }

    /// <summary>
    /// The two-character country where the phone number is located to use for the new SMS subscriber.
    /// </summary>
    [JsonProperty("phone_country")]
    public string PhoneCountry { get; set; }

    /// <summary>
    /// The phone number (as you would dial from the phone_country) to use for the new SMS subscriber.
    /// </summary>
    [JsonProperty("phone_number")]
    public string PhoneNumber { get; set; }

    /// <summary>
    /// If skip_confirmation_notification is true, the subscriber does not receive any notifications when their subscription changes. Email subscribers will be automatically opted in. This option is only available for paid pages. This option has no effect for trial customers.
    /// </summary>
    [JsonProperty("skip_confirmation_notification")]
    public bool? SkipConfirmationNotification { get; set; }
  }
}