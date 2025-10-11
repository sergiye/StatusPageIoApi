using Newtonsoft.Json;

namespace StatusPageIoApi {
  public class Postmortem2 {

    /// <summary>
    /// Body of Postmortem to create.
    /// </summary>
    [JsonProperty("body_draft")]
    public string BodyDraft { get; set; }
  }
  
  /// <summary>
  /// Create Postmortem
  /// </summary>
  public class PutIncidentPostmortem {

    [JsonProperty("postmortem")]
    public Postmortem2 Postmortem { get; set; }
  }
}