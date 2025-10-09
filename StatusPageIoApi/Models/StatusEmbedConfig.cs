using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {

  public class StatusEmbedConfig {

    /// <summary>
    /// Page identifier
    /// </summary>
    [JsonProperty("page_id")]
    public string PageId { get; set; }

    /// <summary>
    /// Corner where status embed iframe will appear on page
    /// </summary>
    [JsonProperty("position")]
    public string Position { get; set; }

    /// <summary>
    /// Color of status embed iframe background when displaying incident
    /// </summary>
    [JsonProperty("incident_background_color")]
    public string IncidentBackgroundColor { get; set; }

    /// <summary>
    /// Color of status embed iframe text when displaying incident
    /// </summary>
    [JsonProperty("incident_text_color")]
    public string IncidentTextColor { get; set; }

    /// <summary>
    /// Color of status embed iframe background when displaying maintenance
    /// </summary>
    [JsonProperty("maintenance_background_color")]
    public string MaintenanceBackgroundColor { get; set; }

    /// <summary>
    /// Color of status embed iframe text when displaying maintenance
    /// </summary>
    [JsonProperty("maintenance_text_color")]
    public string MaintenanceTextColor { get; set; }
  }
}