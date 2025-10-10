using Newtonsoft.Json;

namespace StatusPageIoApi {
  /// <summary>
  /// Update status embed config settings
  /// </summary>
  public class PutStatusEmbedConfig {

    [JsonProperty("status_embed_config")]
    public StatusEmbedConfig StatusEmbedConfig { get; set; }
  }
}