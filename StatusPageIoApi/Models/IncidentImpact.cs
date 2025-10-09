using System;
using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  public class IncidentImpact {

    /// <summary>
    /// Incident Impact Identifier
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// The tenant ID associated with the impact.
    /// </summary>
    [JsonProperty("tenant_id")]
    public string TenantId { get; set; }

    /// <summary>
    /// The Atlassian organization ID associated with the impact.
    /// </summary>
    [JsonProperty("atlassian_organization_id")]
    public string AtlassianOrganizationId { get; set; }

    /// <summary>
    /// The product name associated with the impact.
    /// </summary>
    [JsonProperty("product_name")]
    public string ProductName { get; set; }

    /// <summary>
    /// The list of experiences impacted.
    /// </summary>
    [JsonProperty("experiences")]
    public string[] Experiences { get; set; }

    /// <summary>
    /// The timestamp when the impact was created.
    /// </summary>
    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }
  }
}