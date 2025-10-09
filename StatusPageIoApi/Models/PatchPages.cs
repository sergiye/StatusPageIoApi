using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Update a page
  /// </summary>
  public class PatchPages {

    [JsonProperty("page")]
    public Page Page { get; set; }
  }
}