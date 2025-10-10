using Newtonsoft.Json;

namespace StatusPageIoApi {
  public class PostComponentPageAccessUsers {

    /// <summary>
    /// List of page access users to add to component
    /// </summary>
    [JsonProperty("page_access_user_ids", Required = Required.Always)]
    public string[] PageAccessUserIds { get; set; }
  }
}