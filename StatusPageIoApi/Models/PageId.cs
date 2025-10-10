using Newtonsoft.Json;

namespace StatusPageIoApi {
  public class PageId {

    /// <summary>
    /// Whether or not user should have page configuration role. This field will only be present for pages with Role Based Access Control.
    /// </summary>
    [JsonProperty("page_configuration")]
    public bool? PageConfiguration { get; set; }

    /// <summary>
    /// Whether or not user should have incident manager role. This field will only be present for pages with Role Based Access Control.
    /// </summary>
    [JsonProperty("incident_manager")]
    public bool? IncidentManager { get; set; }

    /// <summary>
    /// Whether or not user should have maintenance manager role. This field will only be present for pages with Role Based Access Control.
    /// </summary>
    [JsonProperty("maintenance_manager")]
    public bool? MaintenanceManager { get; set; }
  }
}