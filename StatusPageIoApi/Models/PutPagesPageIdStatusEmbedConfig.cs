using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  /// <summary>
  /// Update status embed config settings
  /// </summary>
  public class PutPagesPageIdStatusEmbedConfig {

    [JsonProperty("status_embed_config")]
    public StatusEmbedConfig StatusEmbedConfig { get; set; }
  }
}