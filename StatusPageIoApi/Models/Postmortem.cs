using System;
using Newtonsoft.Json;

namespace StatusPageIoApi {

  public class Postmortem {

    /// <summary>
    /// Preview Key
    /// </summary>
    [JsonProperty("preview_key")]
    public string PreviewKey { get; set; }

    /// <summary>
    /// Postmortem body
    /// </summary>
    [JsonProperty("body")]
    public string Body { get; set; }

    [JsonProperty("body_updated_at")]
    public DateTimeOffset BodyUpdatedAt { get; set; }

    /// <summary>
    /// Body draft
    /// </summary>
    [JsonProperty("body_draft")]
    public string BodyDraft { get; set; }

    [JsonProperty("body_draft_updated_at")]
    public DateTimeOffset BodyDraftUpdatedAt { get; set; }

    [JsonProperty("published_at")]
    public DateTimeOffset PublishedAt { get; set; }

    /// <summary>
    /// Should email subscribers be notified.
    /// </summary>
    [JsonProperty("notify_subscribers")]
    public bool NotifySubscribers { get; set; }

    /// <summary>
    /// Should Twitter followers be notified.
    /// </summary>
    [JsonProperty("notify_twitter")]
    public bool NotifyTwitter { get; set; }

    /// <summary>
    /// Custom tweet for Incident Postmortem
    /// </summary>
    [JsonProperty("custom_tweet")]
    public string CustomTweet { get; set; }

    [JsonProperty("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }
  }
}