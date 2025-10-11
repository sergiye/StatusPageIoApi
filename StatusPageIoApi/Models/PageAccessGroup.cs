using System;
using Newtonsoft.Json;

namespace StatusPageIoApi {

  public class PageAccessGroup {

    /// <summary>
    /// Page Access Group Identifier
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// Page Identifier.
    /// </summary>
    [JsonProperty("page_id")]
    public string PageId { get; set; }

    /// <summary>
    /// Name for this Group.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("page_access_user_ids")]
    public string[] PageAccessUserIds { get; set; }

    /// <summary>
    /// Associates group with external group.
    /// </summary>
    [JsonProperty("external_identifier")]
    public string ExternalIdentifier { get; set; }

    [JsonProperty("metric_ids")]
    public string[] MetricIds { get; set; }

    [JsonProperty("component_ids")]
    public string[] ComponentIds { get; set; }

    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }
  }
  
  /// <summary>
  /// Create a page access group
  /// </summary>
  public class PostPageAccessGroup {

    [JsonProperty("page_access_group")]
    public PageAccessGroup PageAccessGroup { get; set; }
  }

  /// <summary>
  /// Update a page access group
  /// </summary>
  public class PatchPageAccessGroup {

    [JsonProperty("page_access_group")]
    public PageAccessGroup PageAccessGroup { get; set; }
  }

  /// <summary>
  /// Update a page access group
  /// </summary>
  public class PutPageAccessGroup {

    [JsonProperty("page_access_group")]
    public PageAccessGroup PageAccessGroup { get; set; }
  }
}