using System.Collections.Generic;
using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  public class Body {

    /// <summary>
    /// List of page access users to add to component
    /// </summary>
    [JsonProperty("page_access_user_ids", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public ICollection<string> PageAccessUserIds { get; set; } =
      new System.Collections.ObjectModel.Collection<string>();
  }
}