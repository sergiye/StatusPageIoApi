using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Create a user
  /// </summary>
  public class PostOrganizationsOrganizationIdUsers {

    [JsonProperty("user", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public User User { get; set; } = new User();
  }
}