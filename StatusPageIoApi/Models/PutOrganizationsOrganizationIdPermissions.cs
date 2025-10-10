using Newtonsoft.Json;

namespace StatusPageIoApi {
  /// <summary>
  /// Update a user's role permissions. Payload should contain a mapping of pages to a set of the desired roles,
  /// <br/>                  if the page has Role Based Access Control. Otherwise, the pages should map to an empty hash.
  /// <br/>                  User will lose access to any pages omitted from the payload.
  /// </summary>
  public class PutOrganizationsOrganizationIdPermissions {

    [JsonProperty("pages")]
    public Pages Pages { get; set; }
  }
}