using System.Collections.Generic;
using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  public class ComponentGroup {

    [JsonProperty("components", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public ICollection<string> Components { get; set; } =
      new System.Collections.ObjectModel.Collection<string>();

    [JsonProperty("name", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Name { get; set; }
  }
}