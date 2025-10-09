using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  public class Pages {

    /// <summary>
    /// Page identifier
    /// </summary>
    [JsonProperty("page_id")]
    public string PageId { get; set; }

    /// <summary>
    /// User has page configuration role. This field will only be present if the organization has Role Based Access Control.
    /// </summary>
    [JsonProperty("page_configuration")]
    public bool PageConfiguration { get; set; }

    /// <summary>
    /// User has incident manager role. This field will only be present if the organization has Role Based Access Control.
    /// </summary>
    [JsonProperty("incident_manager")]
    public bool IncidentManager { get; set; }

    /// <summary>
    /// User has maintenance manager role. This field will only be present if the organization has Role Based Access Control.
    /// </summary>
    [JsonProperty("maintenance_manager")]
    public bool MaintenanceManager { get; set; }
  }
}