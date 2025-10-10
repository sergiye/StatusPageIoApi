using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  public class ComponentGroup {

    [JsonProperty("components", Required = Required.Always)]
    public string[] Components { get; set; }

    [JsonProperty("name", Required = Required.Always)]
    public string Name { get; set; }
  }
}