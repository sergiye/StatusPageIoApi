using System;
using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Delete metric for page access user
  /// </summary>
  public class PageAccessUser {

    /// <summary>
    /// Page Access User Identifier
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("page_id")]
    public string PageId { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    /// <summary>
    /// IDP login user id. Key is typically "uid".
    /// </summary>
    [JsonProperty("external_login")]
    public string ExternalLogin { get; set; }

    [JsonProperty("page_access_group_id")]
    public string PageAccessGroupId { get; set; }

    [JsonProperty("page_access_group_ids")]
    public string[] PageAccessGroupIds { get; set; }

    [JsonProperty("subscribe_to_components")]
    public bool SubscribeToComponents { get; set; }

    [JsonProperty("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }
  }
}