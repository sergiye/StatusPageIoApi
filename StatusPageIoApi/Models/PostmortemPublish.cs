using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {
  public class PostmortemPublish {

    /// <summary>
    /// Whether to notify Twitter followers
    /// </summary>
    [JsonProperty("notify_twitter")]
    public bool NotifyTwitter { get; set; }

    /// <summary>
    /// Whether to notify e-mail subscribers
    /// </summary>
    [JsonProperty("notify_subscribers")]
    public bool NotifySubscribers { get; set; }

    /// <summary>
    /// Custom postmortem tweet to publish
    /// </summary>
    [JsonProperty("custom_tweet")]
    public string CustomTweet { get; set; }
  }
}