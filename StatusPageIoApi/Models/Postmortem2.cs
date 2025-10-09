using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  public class Postmortem2 {

    /// <summary>
    /// Body of Postmortem to create.
    /// </summary>
    [JsonProperty("body_draft", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string BodyDraft { get; set; }
  }
}