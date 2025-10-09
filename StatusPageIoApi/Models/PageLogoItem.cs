using System;
using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  public class PageLogoItem {

    [JsonProperty("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }
    [JsonProperty("original_url")]
    public string OriginalUrl { get; set; }
    [JsonProperty("size")]
    public int? Size { get; set; }
    [JsonProperty("normal_url")]
    public string NormalUrl { get; set; }
    [JsonProperty("retina_url")]
    public string RetinaUrl { get; set; }
  }
}