# StatusPageIoApi

[![Nuget](https://img.shields.io/nuget/v/SergiyE.StatusPageIoApi?style=for-the-badge)](https://www.nuget.org/packages/SergiyE.StatusPageIoApi/)
[![Nuget](https://img.shields.io/nuget/dt/SergiyE.StatusPageIoApi?label=nuget-downloads&style=for-the-badge)](https://www.nuget.org/packages/SergiyE.StatusPageIoApi/)

*StatusPageIoApi is an implementation of [StatusPage.io](https://statuspage.io)'s [API](https://developer.statuspage.io) client in C# / .NET*

## Developer information
**Integrate the library in own application**
1. Add the [StatusPageIoApi](https://www.nuget.org/packages/SergiyE.StatusPageIoApi/) NuGet package to your application.
2. Use the sample code below or the test console application from [here](https://github.com/SergiyE/StatusPageIoApi/tree/master/LibTest)


**Sample code**
```c#
string apiKey = <your-api-key>;
StatusPageIoApi.ApiClient apiClient = new StatusPageIoApi.ApiClient(apiKey);

Page[] pages = await apiClient.GetPages(cancellationToken);
foreach (Page page in pages) {

  //get all components on page
  Component[] components = await apiClient.GetPagesComponents(page.Id, cancellationToken: cancellationToken);

  //create new incident
  Incident newIncident = await apiClient.PostIncidents(pageId, new PostIncident { 
    Incident = new CreateIncident {
      Name = "The system has failed.",
      Status = IncidentStatus.Identified,
      DeliverNotifications = false,
      Components = components.ToDictionary(c => c.Id, c => ComponentStatus.UnderMaintenance),
      ComponentIds = components.Select(c => c.Id).ToArray(),
      ImpactOverride = IncidentImpactOverride.Critical,
    },
  }, cancellationToken: cancellationToken);

  //close all page incidents
  Incident[] incidents = await apiClient.GetIncidents(pageId, cancellationToken: cancellationToken);
  foreach (Incident incident in incidents) {
    switch (incident.Status) {
      case IncidentStatus.Resolved:
      case IncidentStatus.Completed:
        //remove old resolved incident
        if (incident.ResolvedAt.HasValue && incident.ResolvedAt.Value.AddHours(1) < DateTime.UtcNow) {
          await apiClient.DeleteIncident(page.Id, incident.Id, cancellationToken);
        }
        continue;
      case IncidentStatus.Investigating:
      case IncidentStatus.Identified:
      case IncidentStatus.Monitoring:
        incident.Status = IncidentStatus.Resolved;
        break;
      case IncidentStatus.Scheduled:
      case IncidentStatus.InProgress:
      case IncidentStatus.Verifying:
        incident.Status = IncidentStatus.Completed;
        break;
      case null:
        continue;
    }

    await apiClient.PatchIncident(page.Id, incident.Id, new PatchIncident {
      Incident = new Incident {
        Status = incident.Status,
      }
    }, cancellationToken: cancellationToken);
  }

  //patch component statuses
  components = await apiClient.GetPagesComponents(page.Id, cancellationToken: cancellationToken);
  foreach (Component component in components) {
    if (component.Status == ComponentStatus.Operational) continue;
    component.Status = ComponentStatus.Operational;
    await apiClient.PatchComponent(page.Id, component.Id, new PatchComponent {
      Component = new Component {
        Status = ComponentStatus.Operational,
      }
    }, cancellationToken: cancellationToken);
  }

}
```


> [!CAUTION]
> This package is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
