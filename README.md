# sergiye.StatusPageIoApi

[![Nuget](https://img.shields.io/nuget/v/SergiyE.StatusPageIoApi?style=for-the-badge)](https://www.nuget.org/packages/SergiyE.StatusPageIoApi/)
[![Nuget](https://img.shields.io/nuget/dt/SergiyE.StatusPageIoApi?label=nuget-downloads&style=for-the-badge)](https://www.nuget.org/packages/SergiyE.StatusPageIoApi/)

*SergiyE.StatusPageIoApi is an implementation of StatusPage.io's API in C# / .NET*

## Developer information
**Integrate the library in own application**
1. Add the [SergiyE.StatusPageIoApi](https://www.nuget.org/packages/SergiyE.StatusPageIoApi/) NuGet package to your application.
2. Use the sample code below or the test console application from [here](https://github.com/SergiyE/StatusPageIoApi/tree/master/LibTest)


**Sample code**
```c#
string apiKey = <your-api-key>;
StatusPageIoApi apiClient = new StatusPageIoApi(apiKey);
Page[] pages = await apiClient.GetPages(cancellationToken);
foreach (Page page in pages) {
  Component[] components = await apiClient.GetPagesComponents(page.Id, cancellationToken: cancellationToken);
  foreach(Component component in components) {
    if (component.Status == ComponentStatus.Operational) continue;
    component.Status = ComponentStatus.Operational;
    await apiClient.PatchComponent(page.Id, component.Id, new PatchComponent {
      Component = new Component {
        Status = ComponentStatus.Operational,
      }
    }, cancellationToken: cancellationToken);

  }
  Incident[] incidents = await apiClient.GetIncidents(pageId, cancellationToken: cancellationToken);
  foreach (Incident incident in incidents) {
    switch (incident.Status) {
      case IncidentStatus.Resolved:
      case IncidentStatus.Completed:
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
    }

    await apiClient.PatchIncident(page.Id, incident.Id, new PatchIncident {
      Incident = new Incident {
        Status = incident.Status,
      }
    }, cancellationToken: cancellationToken);
  }
}
```


> [!CAUTION]
> This package is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
