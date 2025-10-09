using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Reactivate a list of quarantined subscribers
  /// </summary>
  public class PostSubscribersReactivate {

    /// <summary>
    /// The array of quarantined subscriber codes to reactivate, or "all" to reactivate all quarantined subscribers.
    /// </summary>
    [JsonProperty("subscribers", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Subscribers { get; set; }

    /// <summary>
    /// If this is present, only reactivate subscribers of this type.
    /// </summary>
    [JsonProperty("type")]
    public PostSubscribersReactivateType Type { get; set; }
  }
}