using System.Collections.Generic;
using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Replace components for page access user
  /// </summary>
  public class PostPageAccessUserComponents {

    /// <summary>
    /// List of component codes to allow access to
    /// </summary>
    [JsonProperty("component_ids", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public ICollection<string> ComponentIds { get; set; } =
      new System.Collections.ObjectModel.Collection<string>();
  }
}