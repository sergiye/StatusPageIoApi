using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Update a subscriber
  /// </summary>
  public class PatchSubscriber {

    /// <summary>
    /// A list of component ids for which the subscriber should recieve updates for. Components must be an array with at least one element if it is passed at all. Each component must belong to the page indicated in the path. To set the subscriber to be subscribed to all components on the page, exclude this parameter.
    /// </summary>
    [JsonProperty("component_ids")]
    public string[] ComponentIds { get; set; }
  }
}