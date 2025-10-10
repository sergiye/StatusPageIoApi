using System;
using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {

  public class ComponentGroup {

    /// <summary>
    /// Component Group Identifier
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("page_id")]
    public string PageId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("components")]
    public string[] Components { get; set; }

    [JsonProperty("position")]
    public string Position { get; set; }

    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }
  }
  
  public class EditComponentGroup {

    [JsonProperty("components", Required = Required.Always)]
    public string[] Components { get; set; }

    [JsonProperty("name", Required = Required.Always)]
    public string Name { get; set; }
  }
}