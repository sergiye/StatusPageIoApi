using Newtonsoft.Json;

namespace StatusPageIoApi {
  /// <summary>
  /// Create a user
  /// </summary>
  public class PostOrganizationUser {

    [JsonProperty("user", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public User User { get; set; } = new User();
  }
}