using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace StatusPageIoApi {

  public class Incident {

    /// <summary>
    /// Incident Identifier
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// Map of status changes to apply to affected components
    /// </summary>
    [JsonProperty("components")]
    public Component[] Components { get; set; }

    /// <summary>
    /// The timestamp when the incident was created at.
    /// </summary>
    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    /// The impact of the incident.
    /// </summary>
    [JsonProperty("impact")]
    [JsonConverter(typeof(StringEnumConverter))]
    public IncidentImpactOverride? Impact { get; set; }

    /// <summary>
    /// value to override calculated impact value
    /// </summary>
    [JsonProperty("impact_override")]
    [JsonConverter(typeof(StringEnumConverter))]
    public IncidentImpactOverride? ImpactOverride { get; set; }

    /// <summary>
    /// The incident updates for incident.
    /// </summary>
    [JsonProperty("incident_updates")]
    public IncidentUpdate[] IncidentUpdates { get; set; }

    /// <summary>
    /// The incident impacts for the incident.
    /// </summary>
    [JsonProperty("incident_impacts")]
    public IncidentImpact[] IncidentImpacts { get; set; }

    /// <summary>
    /// Metadata attached to the incident. Top level values must be objects.
    /// </summary>
    [JsonProperty("metadata")]
    public object Metadata { get; set; }

    /// <summary>
    /// The timestamp when incident entered monitoring state.
    /// </summary>
    [JsonProperty("monitoring_at")]
    public DateTimeOffset? MonitoringAt { get; set; }

    /// <summary>
    /// Incident Name. There is a maximum limit of 255 characters.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Incident Page Identifier
    /// </summary>
    [JsonProperty("page_id")]
    public string PageId { get; set; }

    /// <summary>
    /// Body of the Postmortem.
    /// </summary>
    [JsonProperty("postmortem_body")]
    public string PostmortemBody { get; set; }

    /// <summary>
    /// The timestamp when the incident postmortem body was last updated at.
    /// </summary>
    [JsonProperty("postmortem_body_last_updated_at")]
    public DateTimeOffset? PostmortemBodyLastUpdatedAt { get; set; }

    /// <summary>
    /// Controls whether the incident will have postmortem.
    /// </summary>
    [JsonProperty("postmortem_ignored")]
    public bool? PostmortemIgnored { get; set; }

    /// <summary>
    /// Indicates whether subscribers are already notificed about postmortem.
    /// </summary>
    [JsonProperty("postmortem_notified_subscribers")]
    public bool? PostmortemNotifiedSubscribers { get; set; }

    /// <summary>
    /// Controls whether to decide if notify postmortem on twitter.
    /// </summary>
    [JsonProperty("postmortem_notified_twitter")]
    public bool? PostmortemNotifiedTwitter { get; set; }

    /// <summary>
    /// The timestamp when the postmortem was published.
    /// </summary>
    [JsonProperty("postmortem_published_at")]
    public bool? PostmortemPublishedAt { get; set; }

    /// <summary>
    /// The timestamp when incident was resolved.
    /// </summary>
    [JsonProperty("resolved_at")]
    public DateTimeOffset? ResolvedAt { get; set; }

    /// <summary>
    /// Controls whether the incident is scheduled to automatically change to complete.
    /// </summary>
    [JsonProperty("scheduled_auto_completed")]
    public bool? ScheduledAutoCompleted { get; set; }

    /// <summary>
    /// Controls whether the incident is scheduled to automatically change to in progress.
    /// </summary>
    [JsonProperty("scheduled_auto_in_progress")]
    public bool? ScheduledAutoInProgress { get; set; }

    /// <summary>
    /// The timestamp the incident is scheduled for.
    /// </summary>
    [JsonProperty("scheduled_for")]
    public DateTimeOffset? ScheduledFor { get; set; }

    /// <summary>
    /// Controls whether send notification when scheduled maintenances auto transition to completed.
    /// </summary>
    [JsonProperty("auto_transition_deliver_notifications_at_end")]
    public bool? AutoTransitionDeliverNotificationsAtEnd { get; set; }

    /// <summary>
    /// Controls whether send notification when scheduled maintenances auto transition to started.
    /// </summary>
    [JsonProperty("auto_transition_deliver_notifications_at_start")]
    public bool? AutoTransitionDeliverNotificationsAtStart { get; set; }

    /// <summary>
    /// Controls whether change components status to under_maintenance once scheduled maintenance is in progress.
    /// </summary>
    [JsonProperty("auto_transition_to_maintenance_state")]
    public bool? AutoTransitionToMaintenanceState { get; set; }

    /// <summary>
    /// Controls whether change components status to operational once scheduled maintenance completes.
    /// </summary>
    [JsonProperty("auto_transition_to_operational_state")]
    public bool? AutoTransitionToOperationalState { get; set; }

    /// <summary>
    /// Controls whether to remind subscribers prior to scheduled incidents.
    /// </summary>
    [JsonProperty("scheduled_remind_prior")]
    public bool? ScheduledRemindPrior { get; set; }

    /// <summary>
    /// The timestamp when the scheduled incident reminder was sent at.
    /// </summary>
    [JsonProperty("scheduled_reminded_at")]
    public DateTimeOffset? ScheduledRemindedAt { get; set; }

    /// <summary>
    /// The timestamp the incident is scheduled until.
    /// </summary>
    [JsonProperty("scheduled_until")]
    public DateTimeOffset? ScheduledUntil { get; set; }

    /// <summary>
    /// Incident Shortlink.
    /// </summary>
    [JsonProperty("shortlink")]
    public string Shortlink { get; set; }

    /// <summary>
    /// The incident status. For realtime incidents, valid values are investigating, identified, monitoring, and resolved. For scheduled incidents, valid values are scheduled, in_progress, verifying, and completed.
    /// </summary>
    [JsonProperty("status")]
    [JsonConverter(typeof(StringEnumConverter))]
    public IncidentStatus? Status { get; set; }

    /// <summary>
    /// The timestamp when the incident was updated at.
    /// </summary>
    [JsonProperty("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// Custom reminder intervals for unresolved/open incidents. Not applicable for &lt;strong&gt;Scheduled maintenance&lt;/strong&gt;&lt;br&gt;There are 4 possible states for reminder_intervals:&lt;br&gt;&lt;strong&gt;DEFAULT:&lt;/strong&gt; NULL, representing a default behavior with intervals [3, 6, 12, 24].&lt;br&gt;&lt;strong&gt;AFTER:&lt;/strong&gt; A serialized array of strictly increasing intervals, each integer ranges from [1-24] (inclusive). Ex "[1, 5, 7, 10]"&lt;br&gt;&lt;strong&gt;EVERY:&lt;/strong&gt; An integer in the range [1-24] as a string, representing equal intervals. Ex "4" for [4, 8, 12, 16, 20, 24]&lt;br&gt;&lt;strong&gt;OFF:&lt;/strong&gt; A serialized empty array, for example, "[]", meaning no reminder notifications will be sent.
    /// </summary>
    [JsonProperty("reminder_intervals")]
    public string ReminderIntervals { get; set; }
  }

  public class EditIncident {

    /// <summary>
    /// Incident Name. There is a maximum limit of 255 characters.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// The incident status. For realtime incidents, valid values are investigating, identified, monitoring, and resolved. For scheduled incidents, valid values are scheduled, in_progress, verifying, and completed.
    /// </summary>
    [JsonProperty("status")]
    [JsonConverter(typeof(StringEnumConverter))]
    public IncidentStatus? Status { get; set; }

    /// <summary>
    /// value to override calculated impact value
    /// </summary>
    [JsonProperty("impact_override")]
    [JsonConverter(typeof(StringEnumConverter))]
    public IncidentImpactOverride? ImpactOverride { get; set; }

    /// <summary>
    /// The timestamp the incident is scheduled for.
    /// </summary>
    [JsonProperty("scheduled_for")]
    public DateTimeOffset? ScheduledFor { get; set; }

    /// <summary>
    /// The timestamp the incident is scheduled until.
    /// </summary>
    [JsonProperty("scheduled_until")]
    public DateTimeOffset? ScheduledUntil { get; set; }

    /// <summary>
    /// Controls whether change components status to under_maintenance once scheduled maintenance is in progress.
    /// </summary>
    [JsonProperty("auto_transition_to_maintenance_state")]
    public bool? AutoTransitionToMaintenanceState { get; set; }

    /// <summary>
    /// Controls whether change components status to operational once scheduled maintenance completes.
    /// </summary>
    [JsonProperty("auto_transition_to_operational_state")]
    public bool? AutoTransitionToOperationalState { get; set; }

    /// <summary>
    /// Controls whether the incident is scheduled to automatically change to in progress.
    /// </summary>
    [JsonProperty("scheduled_auto_in_progress")]
    public bool? ScheduledAutoInProgress { get; set; }

    /// <summary>
    /// Controls whether the incident is scheduled to automatically change to complete.
    /// </summary>
    [JsonProperty("scheduled_auto_completed")]
    public bool? ScheduledAutoCompleted { get; set; }

    /// <summary>
    /// Controls whether send notification when scheduled maintenances auto transition to started.
    /// </summary>
    [JsonProperty("auto_transition_deliver_notifications_at_start")]
    public bool? AutoTransitionDeliverNotificationsAtStart { get; set; }

    /// <summary>
    /// Controls whether send notification when scheduled maintenances auto transition to completed.
    /// </summary>
    [JsonProperty("auto_transition_deliver_notifications_at_end")]
    public bool? AutoTransitionDeliverNotificationsAtEnd { get; set; }

    /// <summary>
    /// Custom reminder intervals for unresolved/open incidents. Not applicable for &lt;strong&gt;Scheduled maintenance&lt;/strong&gt;&lt;br&gt;There are 4 possible states for reminder_intervals:&lt;br&gt;&lt;strong&gt;DEFAULT:&lt;/strong&gt; NULL, representing a default behavior with intervals [3, 6, 12, 24].&lt;br&gt;&lt;strong&gt;AFTER:&lt;/strong&gt; A serialized array of strictly increasing intervals, each integer ranges from [1-24] (inclusive). Ex "[1, 5, 7, 10]"&lt;br&gt;&lt;strong&gt;EVERY:&lt;/strong&gt; An integer in the range [1-24] as a string, representing equal intervals. Ex "4" for [4, 8, 12, 16, 20, 24]&lt;br&gt;&lt;strong&gt;OFF:&lt;/strong&gt; A serialized empty array, for example, "[]", meaning no reminder notifications will be sent.
    /// </summary>
    [JsonProperty("reminder_intervals")]
    public string ReminderIntervals { get; set; }
    
    /// <summary>
    /// Metadata attached to the incident. Top level values must be objects.
    /// </summary>
    [JsonProperty("metadata")]
    public object Metadata { get; set; }

    /// <summary>
    /// Deliver notifications to subscribers if this is true. If this is false, create an incident without notifying customers.
    /// </summary>
    [JsonProperty("deliver_notifications")]
    public bool? DeliverNotifications { get; set; } = true;

    /// <summary>
    /// Controls whether tweet automatically when scheduled maintenance starts.
    /// </summary>
    [JsonProperty("auto_tweet_at_beginning")]
    public bool? AutoTweetAtBeginning { get; set; }

    /// <summary>
    /// Controls whether tweet automatically when scheduled maintenance completes.
    /// </summary>
    [JsonProperty("auto_tweet_on_completion")]
    public bool? AutoTweetOnCompletion { get; set; }

    /// <summary>
    /// Controls whether tweet automatically when scheduled maintenance is created.
    /// </summary>
    [JsonProperty("auto_tweet_on_creation")]
    public bool? AutoTweetOnCreation { get; set; }

    /// <summary>
    /// Controls whether tweet automatically one hour before scheduled maintenance starts.
    /// </summary>
    [JsonProperty("auto_tweet_one_hour_before")]
    public bool? AutoTweetOneHourBefore { get; set; }

    /// <summary>
    /// TimeStamp when incident was backfilled.
    /// </summary>
    [JsonProperty("backfill_date")]
    public string BackfillDate { get; set; }

    /// <summary>
    /// Controls whether incident is backfilled. If true, components cannot be specified.
    /// </summary>
    [JsonProperty("backfilled")]
    public bool? Backfilled { get; set; }

    /// <summary>
    /// The initial message, created as the first incident update. There is a maximum limit of 25000 characters
    /// </summary>
    [JsonProperty("body")]
    public string Body { get; set; }
    
    /// <summary>
    /// Map of status changes to apply to affected components
    /// </summary>
    [JsonProperty("components")]
    public Dictionary<string, ComponentStatus> Components { get; set; }

    /// <summary>
    /// List of component_ids affected by this incident
    /// </summary>
    [JsonProperty("component_ids")]
    public string[] ComponentIds { get; set; }

    /// <summary>
    /// Same as :scheduled_auto_transition_in_progress. Controls whether the incident is scheduled to automatically change to in progress.
    /// </summary>
    [JsonProperty("scheduled_auto_transition")]
    public bool? ScheduledAutoTransition { get; set; }
  }
  
  /// <summary>
  /// Create an incident
  /// </summary>
  public class PostIncident {

    [JsonProperty("incident")]
    public EditIncident Incident { get; set; }
  }
  
  /// <summary>
  /// Update an incident
  /// </summary>
  public class PatchIncident {

    [JsonProperty("incident")]
    public EditIncident Incident { get; set; }
  }
  
  /// <summary>
  /// Update an incident
  /// </summary>
  public class PutIncident {

    [JsonProperty("incident")]
    public EditIncident Incident { get; set; }
  }
}