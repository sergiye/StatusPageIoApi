using System;
using Newtonsoft.Json;

namespace SergiyE.StatusPageIoApi {

  public class Page {
    /// <summary>
    /// Page identifier
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// Timestamp the record was created
    /// </summary>
    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    /// Timestamp the record was last updated
    /// </summary>
    [JsonProperty("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// Name of your page to be displayed
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("page_description")]
    public string PageDescription { get; set; }

    [JsonProperty("headline")]
    public string Headline { get; set; }

    /// <summary>
    /// The main template your statuspage will use
    /// </summary>
    [JsonProperty("branding")]
    public PageBranding? Branding { get; set; }

    /// <summary>
    /// Subdomain at which to access your status page
    /// </summary>
    [JsonProperty("subdomain")]
    public string Subdomain { get; set; }

    /// <summary>
    /// CNAME alias for your status page
    /// </summary>
    [JsonProperty("domain")]
    public string Domain { get; set; }

    /// <summary>
    /// Website of your page.  Clicking on your statuspage image will link here.
    /// </summary>
    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("support_url")]
    public string SupportUrl { get; set; }

    /// <summary>
    /// Should your page hide itself from search engines
    /// </summary>
    [JsonProperty("hidden_from_search")]
    public bool? HiddenFromSearch { get; set; }

    /// <summary>
    /// Can your users subscribe to all notifications on the page
    /// </summary>
    [JsonProperty("allow_page_subscribers")]
    public bool? AllowPageSubscribers { get; set; }

    /// <summary>
    /// Can your users subscribe to notifications for a single incident
    /// </summary>
    [JsonProperty("allow_incident_subscribers")]
    public bool? AllowIncidentSubscribers { get; set; }

    /// <summary>
    /// Can your users choose to receive notifications via email
    /// </summary>
    [JsonProperty("allow_email_subscribers")]
    public bool? AllowEmailSubscribers { get; set; }

    /// <summary>
    /// Can your users choose to receive notifications via SMS
    /// </summary>
    [JsonProperty("allow_sms_subscribers")]
    public bool? AllowSmsSubscribers { get; set; }

    /// <summary>
    /// Can your users choose to access incident feeds via RSS/Atom (not functional on Audience-Specific pages)
    /// </summary>
    [JsonProperty("allow_rss_atom_feeds")]
    public bool? AllowRssAtomFeeds { get; set; }

    /// <summary>
    /// Can your users choose to receive notifications via Webhooks
    /// </summary>
    [JsonProperty("allow_webhook_subscribers")]
    public bool? AllowWebhookSubscribers { get; set; }

    /// <summary>
    /// Allows you to customize the email address your page notifications come from
    /// </summary>
    [JsonProperty("notifications_from_email")]
    public string NotificationsFromEmail { get; set; }

    /// <summary>
    /// Allows you to customize the footer appearing on your notification emails.  Accepts Markdown for formatting
    /// </summary>
    [JsonProperty("notifications_email_footer")]
    public string NotificationsEmailFooter { get; set; }

    [JsonProperty("activity_score")]
    public float? ActivityScore { get; set; }

    [JsonProperty("twitter_username")]
    public string TwitterUsername { get; set; }

    [JsonProperty("viewers_must_be_team_members")]
    public bool? ViewersMustBeTeamMembers { get; set; }

    [JsonProperty("ip_restrictions")]
    public string IpRestrictions { get; set; }

    [JsonProperty("city")]
    public string City { get; set; }

    [JsonProperty("state")]
    public string State { get; set; }

    [JsonProperty("country")]
    public string Country { get; set; }

    /// <summary>
    /// Timezone configured for your page
    /// </summary>
    [JsonProperty("time_zone")]
    public string TimeZone { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_body_background_color")]
    public string CssBodyBackgroundColor { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_font_color")]
    public string CssFontColor { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_light_font_color")]
    public string CssLightFontColor { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_greens")]
    public string CssGreens { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_yellows")]
    public string CssYellows { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_oranges")]
    public string CssOranges { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_blues")]
    public string CssBlues { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_reds")]
    public string CssReds { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_border_color")]
    public string CssBorderColor { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_graph_color")]
    public string CssGraphColor { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_link_color")]
    public string CssLinkColor { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_no_data")]
    public string CssNoData { get; set; }

    [JsonProperty("favicon_logo")]
    public PageLogoItem FaviconLogo { get; set; }

    [JsonProperty("transactional_logo")]
    public PageLogoItem TransactionalLogo { get; set; }

    [JsonProperty("hero_cover")]
    public PageLogoItem HeroCover { get; set; }

    [JsonProperty("email_logo")]
    public PageLogoItem EmailLogo { get; set; }

    [JsonProperty("twitter_logo")]
    public PageLogoItem TwitterLogo { get; set; }
  }
}