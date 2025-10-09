using System.Collections.Generic;
using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Replace components for a page access group
  /// </summary>
  public class PostPagesPageIdPageAccessGroupsPageAccessGroupIdComponents {

    /// <summary>
    /// List of components codes to set on the page access group
    /// </summary>
    [JsonProperty("component_ids", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public ICollection<string> ComponentIds { get; set; } =
      new System.Collections.ObjectModel.Collection<string>();
  }
}