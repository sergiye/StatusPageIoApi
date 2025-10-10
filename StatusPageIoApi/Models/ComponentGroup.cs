using System;
using Newtonsoft.Json;

namespace StatusPageIoApi {

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
  
  /// <summary>
  /// Create a component group
  /// </summary>
  public class PostComponentGroup {

    /// <summary>
    /// Description of the component group.
    /// </summary>
    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("component_group")]
    public EditComponentGroup ComponentGroup { get; set; }
  }
  
  /// <summary>
  /// Partially update a component group
  /// </summary>
  public class PatchComponentGroup {

    /// <summary>
    /// Updated description of the component group.
    /// </summary>
    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("component_group")]
    public EditComponentGroup ComponentGroup { get; set; }
  }
  
  /// <summary>
  /// Update a component group
  /// </summary>
  public class PutComponentGroup {

    /// <summary>
    /// Updated description of the component group.
    /// </summary>
    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("component_group")]
    public EditComponentGroup ComponentGroup { get; set; }
  }
}