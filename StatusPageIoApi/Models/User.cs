using System;
using Newtonsoft.Json;

namespace StatusPageIoApi {

  public class User {

    /// <summary>
    /// User identifier
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// Organization identifier
    /// </summary>
    [JsonProperty("organization_id")]
    public string OrganizationId { get; set; }

    /// <summary>
    /// Email address for the team member
    /// </summary>
    [JsonProperty("email")]
    public string Email { get; set; }

    /// <summary>
    /// Password the team member uses to access the site
    /// </summary>
    [JsonProperty("password")]
    public string Password { get; set; }

    [JsonProperty("first_name")]
    public string FirstName { get; set; }

    [JsonProperty("last_name")]
    public string LastName { get; set; }

    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }
  }

  public class CreateUser {

    /// <summary>
    /// Email address for the team member
    /// </summary>
    [JsonProperty("email")]
    public string Email { get; set; }

    /// <summary>
    /// Password the team member uses to access the site
    /// </summary>
    [JsonProperty("password")]
    public string Password { get; set; }

    [JsonProperty("first_name")]
    public string FirstName { get; set; }

    [JsonProperty("last_name")]
    public string LastName { get; set; }
  }
  
  public class PostOrganizationUser {

    [JsonProperty("user")]
    public CreateUser User { get; set; }
  }
}