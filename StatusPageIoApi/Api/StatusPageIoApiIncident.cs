using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SergiyE.StatusPageIoApi {

  public partial class StatusPageIoApi {

    /// <summary>
    /// Create a template
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="body"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Create a template</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<IncidentTemplate> PostIncidentTemplates(string pageId,
      PostPagesPageIdIncidentTemplates body,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/incident_templates");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        var json = JsonConvert.SerializeObject(body, settings.Value);
        var content = new StringContent(json);
        content.Headers.ContentType =
          MediaTypeHeaderValue.Parse("application/json");
        request.Content = content;
        request.Method = new HttpMethod("POST");

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content?.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int) response.StatusCode;
          if (status == 201) {
            var objectResponse =
              await ReadObjectResponseAsync<IncidentTemplate>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            return objectResponse.Object;
          }
          else if (status == 400) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("Bad request", status, objectResponse.Text, headers,
              objectResponse.Object, null);
          }
          else if (status == 401) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("Could not authenticate", status, objectResponse.Text,
              headers, objectResponse.Object, null);
          }
          else if (status == 404) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("The requested resource could not be found.", status,
              objectResponse.Text, headers, objectResponse.Object, null);
          }
          else if (status == 422) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("Unprocessable entity", status, objectResponse.Text,
              headers, objectResponse.Object, null);
          }
          else {
            var responseData = response.Content == null
              ? null
              : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            throw new ApiException(
              "The HTTP status code of the response was not expected (" + status + ").", status,
              responseData, headers, null);
          }
        }
        finally {
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Get a list of templates
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="page">Page offset to fetch.</param>
    /// <param name="perPage">Number of results to return per page.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Get a list of templates</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<IncidentTemplate[]> GetIncidentTemplates(string pageId, int? page = null,
      int? perPage = null, CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/incident_templates?");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      if (page != null) {
        urlBuilder.Append(Uri.EscapeDataString("page") + "=")
          .Append(Uri.EscapeDataString(ConvertToString(page,
            CultureInfo.InvariantCulture))).Append("&");
      }

      if (perPage != null) {
        urlBuilder.Append(Uri.EscapeDataString("per_page") + "=")
          .Append(Uri.EscapeDataString(ConvertToString(perPage,
            CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder.Length--;
      using (var request = new HttpRequestMessage()) {
        request.Method = new HttpMethod("GET");

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content?.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int) response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<IncidentTemplate[]>(
                response, headers, cancellationToken).ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            return objectResponse.Object;
          }
          else if (status == 401) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("Could not authenticate", status, objectResponse.Text,
              headers, objectResponse.Object, null);
          }
          else {
            var responseData = response.Content == null
              ? null
              : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            throw new ApiException(
              "The HTTP status code of the response was not expected (" + status + ").", status,
              responseData, headers, null);
          }
        }
        finally {
          if (disposeResponse)
            response.Dispose();
        }
      }
    }

    /// <summary>
    /// Create an incident
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="body"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Create an incident</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Incident> PostIncidents(string pageId, PostPagesPageIdIncidents body,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/pages/{page_id}/incidents");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        var json = JsonConvert.SerializeObject(body, settings.Value);
        var content = new StringContent(json);
        content.Headers.ContentType =
          MediaTypeHeaderValue.Parse("application/json");
        request.Content = content;
        request.Method = new HttpMethod("POST");

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content?.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int) response.StatusCode;
          if (status == 201) {
            var objectResponse =
              await ReadObjectResponseAsync<Incident>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            return objectResponse.Object;
          }
          else if (status == 400) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("Bad request", status, objectResponse.Text, headers,
              objectResponse.Object, null);
          }
          else if (status == 401) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("Could not authenticate", status, objectResponse.Text,
              headers, objectResponse.Object, null);
          }
          else if (status == 404) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("The requested resource could not be found.", status,
              objectResponse.Text, headers, objectResponse.Object, null);
          }
          else if (status == 422) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("Unprocessable entity", status, objectResponse.Text,
              headers, objectResponse.Object, null);
          }
          else {
            var responseData = response.Content == null
              ? null
              : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            throw new ApiException(
              "The HTTP status code of the response was not expected (" + status + ").", status,
              responseData, headers, null);
          }
        }
        finally {
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Get a list of incidents
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="q">If this is specified, search for the text query string in the incidents' name, status, postmortem_body, and incident_updates fields.</param>
    /// <param name="limit">The maximum number of rows to return per page. The default and maximum limit is 100.</param>
    /// <param name="page">Page offset to fetch.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Get a list of incidents</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Incident[]> GetIncidents(string pageId, string q = null, int? limit = null,
      int? page = null,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/pages/{page_id}/incidents?");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      if (q != null) {
        urlBuilder.Append(Uri.EscapeDataString("q") + "=")
          .Append(Uri.EscapeDataString(ConvertToString(q,
            CultureInfo.InvariantCulture))).Append("&");
      }

      if (limit != null) {
        urlBuilder.Append(Uri.EscapeDataString("limit") + "=")
          .Append(Uri.EscapeDataString(ConvertToString(limit,
            CultureInfo.InvariantCulture))).Append("&");
      }

      if (page != null) {
        urlBuilder.Append(Uri.EscapeDataString("page") + "=")
          .Append(Uri.EscapeDataString(ConvertToString(page,
            CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder.Length--;

      using (var request = new HttpRequestMessage()) {
        request.Method = new HttpMethod("GET");

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content?.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int) response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<Incident[]>(
                response, headers, cancellationToken).ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            return objectResponse.Object;
          }
          else if (status == 400) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("Bad request", status, objectResponse.Text, headers,
              objectResponse.Object, null);
          }
          else if (status == 401) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("Could not authenticate", status, objectResponse.Text,
              headers, objectResponse.Object, null);
          }
          else if (status == 404) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("The requested resource could not be found.", status,
              objectResponse.Text, headers, objectResponse.Object, null);
          }
          else {
            var responseData = response.Content == null
              ? null
              : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            throw new ApiException(
              "The HTTP status code of the response was not expected (" + status + ").", status,
              responseData, headers, null);
          }
        }
        finally {
          if (disposeResponse)
            response.Dispose();
        }
      }
    }

    /// <summary>
    /// Get a list of active maintenances
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="page">Page offset to fetch.</param>
    /// <param name="perPage">Number of results to return per page.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Get a list of active maintenances</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Incident[]> GetIncidentsActiveMaintenance(string pageId, int? page = null,
      int? perPage = null, CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/incidents/active_maintenance?");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      if (page != null) {
        urlBuilder.Append(Uri.EscapeDataString("page") + "=")
          .Append(Uri.EscapeDataString(ConvertToString(page,
            CultureInfo.InvariantCulture))).Append("&");
      }

      if (perPage != null) {
        urlBuilder.Append(Uri.EscapeDataString("per_page") + "=")
          .Append(Uri.EscapeDataString(ConvertToString(perPage,
            CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder.Length--;

      using (var request = new HttpRequestMessage()) {
        request.Method = new HttpMethod("GET");

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content?.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int) response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<Incident[]>(
                response, headers, cancellationToken).ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            return objectResponse.Object;
          }
          else if (status == 401) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("Could not authenticate", status, objectResponse.Text,
              headers, objectResponse.Object, null);
          }
          else if (status == 404) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("The requested resource could not be found.", status,
              objectResponse.Text, headers, objectResponse.Object, null);
          }
          else {
            var responseData = response.Content == null
              ? null
              : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            throw new ApiException(
              "The HTTP status code of the response was not expected (" + status + ").", status,
              responseData, headers, null);
          }
        }
        finally {
          if (disposeResponse)
            response.Dispose();
        }
      }
    }

    /// <summary>
    /// Get a list of upcoming incidents
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="page">Page offset to fetch.</param>
    /// <param name="perPage">Number of results to return per page.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Get a list of upcoming incidents</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Incident[]> GetIncidentsUpcoming(string pageId, int? page = null, int? perPage = null,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/incidents/upcoming?");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      if (page != null) {
        urlBuilder.Append(Uri.EscapeDataString("page") + "=")
          .Append(Uri.EscapeDataString(ConvertToString(page,
            CultureInfo.InvariantCulture))).Append("&");
      }

      if (perPage != null) {
        urlBuilder.Append(Uri.EscapeDataString("per_page") + "=")
          .Append(Uri.EscapeDataString(ConvertToString(perPage,
            CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder.Length--;

      using (var request = new HttpRequestMessage()) {
        request.Method = new HttpMethod("GET");

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content?.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int) response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<Incident[]>(
                response, headers, cancellationToken).ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            return objectResponse.Object;
          }
          else if (status == 401) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("Could not authenticate", status, objectResponse.Text,
              headers, objectResponse.Object, null);
          }
          else if (status == 404) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("The requested resource could not be found.", status,
              objectResponse.Text, headers, objectResponse.Object, null);
          }
          else {
            var responseData = response.Content == null
              ? null
              : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            throw new ApiException(
              "The HTTP status code of the response was not expected (" + status + ").", status,
              responseData, headers, null);
          }
        }
        finally {
          if (disposeResponse)
            response.Dispose();
        }
      }
    }

    /// <summary>
    /// Get a list of scheduled incidents
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="page">Page offset to fetch.</param>
    /// <param name="perPage">Number of results to return per page.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Get a list of scheduled incidents</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Incident[]> GetIncidentsScheduled(string pageId, int? page = null, int? perPage = null,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/incidents/scheduled?");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      if (page != null) {
        urlBuilder.Append(Uri.EscapeDataString("page") + "=")
          .Append(Uri.EscapeDataString(ConvertToString(page,
            CultureInfo.InvariantCulture))).Append("&");
      }

      if (perPage != null) {
        urlBuilder.Append(Uri.EscapeDataString("per_page") + "=")
          .Append(Uri.EscapeDataString(ConvertToString(perPage,
            CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder.Length--;

      using (var request = new HttpRequestMessage()) {
        request.Method = new HttpMethod("GET");

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content?.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int) response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<Incident[]>(
                response, headers, cancellationToken).ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            return objectResponse.Object;
          }
          else if (status == 401) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("Could not authenticate", status, objectResponse.Text,
              headers, objectResponse.Object, null);
          }
          else if (status == 404) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("The requested resource could not be found.", status,
              objectResponse.Text, headers, objectResponse.Object, null);
          }
          else {
            var responseData = response.Content == null
              ? null
              : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            throw new ApiException(
              "The HTTP status code of the response was not expected (" + status + ").", status,
              responseData, headers, null);
          }
        }
        finally {
          if (disposeResponse)
            response.Dispose();
        }
      }
    }

    /// <summary>
    /// Get a list of unresolved incidents
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="page">Page offset to fetch.</param>
    /// <param name="perPage">Number of results to return per page.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Get a list of unresolved incidents</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Incident[]> GetIncidentsUnresolved(string pageId, int? page = null, int? perPage = null,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/incidents/unresolved?");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      if (page != null) {
        urlBuilder.Append(Uri.EscapeDataString("page") + "=")
          .Append(Uri.EscapeDataString(ConvertToString(page,
            CultureInfo.InvariantCulture))).Append("&");
      }

      if (perPage != null) {
        urlBuilder.Append(Uri.EscapeDataString("per_page") + "=")
          .Append(Uri.EscapeDataString(ConvertToString(perPage,
            CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder.Length--;

      using (var request = new HttpRequestMessage()) {
        request.Method = new HttpMethod("GET");

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content?.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int) response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<Incident[]>(
                response, headers, cancellationToken).ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            return objectResponse.Object;
          }
          else if (status == 401) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("Could not authenticate", status, objectResponse.Text,
              headers, objectResponse.Object, null);
          }
          else if (status == 404) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("The requested resource could not be found.", status,
              objectResponse.Text, headers, objectResponse.Object, null);
          }
          else {
            var responseData = response.Content == null
              ? null
              : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            throw new ApiException(
              "The HTTP status code of the response was not expected (" + status + ").", status,
              responseData, headers, null);
          }
        }
        finally {
          if (disposeResponse)
            response.Dispose();
        }
      }
    }

    /// <summary>
    /// Delete an incident
    /// </summary>
    /// <remarks>
    /// Delete an incident
    /// </remarks>
    /// <param name="pageId">Page identifier</param>
    /// <param name="incidentId">Incident Identifier</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Delete an incident</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Incident> DeleteIncident(string pageId, string incidentId,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (incidentId == null)
        throw new ArgumentNullException(nameof(incidentId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/incidents/{incident_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{incident_id}",
        Uri.EscapeDataString(ConvertToString(incidentId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        request.Method = new HttpMethod("DELETE");

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content?.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int) response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<Incident>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            return objectResponse.Object;
          }
          else {
            var responseData = response.Content == null
              ? null
              : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            throw new ApiException(
              "The HTTP status code of the response was not expected (" + status + ").", status,
              responseData, headers, null);
          }
        }
        finally {
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Update an incident
    /// </summary>
    /// <remarks>
    /// Update an incident
    /// </remarks>
    /// <param name="pageId">Page identifier</param>
    /// <param name="incidentId">Incident Identifier</param>
    /// <param name="body"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Update an incident</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Incident> PatchIncident(string pageId, string incidentId, PatchPagesPageIdIncidents body,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (incidentId == null)
        throw new ArgumentNullException(nameof(incidentId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/incidents/{incident_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{incident_id}",
        Uri.EscapeDataString(ConvertToString(incidentId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        var json = JsonConvert.SerializeObject(body, settings.Value);
        var content = new StringContent(json);
        content.Headers.ContentType =
          MediaTypeHeaderValue.Parse("application/json");
        request.Content = content;
        request.Method = new HttpMethod("PATCH");

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content?.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int) response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<Incident>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            return objectResponse.Object;
          }
          else {
            var responseData = response.Content == null
              ? null
              : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            throw new ApiException(
              "The HTTP status code of the response was not expected (" + status + ").", status,
              responseData, headers, null);
          }
        }
        finally {
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Update an incident
    /// </summary>
    /// <remarks>
    /// Update an incident
    /// </remarks>
    /// <param name="pageId">Page identifier</param>
    /// <param name="incidentId">Incident Identifier</param>
    /// <param name="body"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Update an incident</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Incident> PutIncident(string pageId, string incidentId, PutPagesPageIdIncidents body,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (incidentId == null)
        throw new ArgumentNullException(nameof(incidentId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/incidents/{incident_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{incident_id}",
        Uri.EscapeDataString(ConvertToString(incidentId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        var json = JsonConvert.SerializeObject(body, settings.Value);
        var content = new StringContent(json);
        content.Headers.ContentType =
          MediaTypeHeaderValue.Parse("application/json");
        request.Content = content;
        request.Method = new HttpMethod("PUT");

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int) response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<Incident>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            return objectResponse.Object;
          }
          else {
            var responseData = response.Content == null
              ? null
              : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            throw new ApiException(
              "The HTTP status code of the response was not expected (" + status + ").", status,
              responseData, headers, null);
          }
        }
        finally {
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Get an incident
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="incidentId">Incident Identifier</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Get an incident</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Incident> GetIncident(string pageId, string incidentId,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (incidentId == null)
        throw new ArgumentNullException(nameof(incidentId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/incidents/{incident_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{incident_id}",
        Uri.EscapeDataString(ConvertToString(incidentId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        request.Method = new HttpMethod("GET");

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content?.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int) response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<Incident>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            return objectResponse.Object;
          }
          else {
            var responseData = response.Content == null
              ? null
              : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            throw new ApiException(
              "The HTTP status code of the response was not expected (" + status + ").", status,
              responseData, headers, null);
          }
        }
        finally {
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Update a previous incident update
    /// </summary>
    /// <remarks>
    /// Update a previous incident update
    /// </remarks>
    /// <param name="pageId">Page identifier</param>
    /// <param name="incidentId">Incident Identifier</param>
    /// <param name="incidentUpdateId">Incident Update Identifier</param>
    /// <param name="body"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Update a previous incident update</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<IncidentUpdate> PatchIncidentUpdate(string pageId, string incidentId,
      string incidentUpdateId, PatchPagesPageIdIncidentsIncidentIdIncidentUpdates body,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (incidentId == null)
        throw new ArgumentNullException(nameof(incidentId));

      if (incidentUpdateId == null)
        throw new ArgumentNullException(nameof(incidentUpdateId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/incidents/{incident_id}/incident_updates/{incident_update_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{incident_id}",
        Uri.EscapeDataString(ConvertToString(incidentId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{incident_update_id}",
        Uri.EscapeDataString(ConvertToString(incidentUpdateId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        var json = JsonConvert.SerializeObject(body, settings.Value);
        var content = new StringContent(json);
        content.Headers.ContentType =
          MediaTypeHeaderValue.Parse("application/json");
        request.Content = content;
        request.Method = new HttpMethod("PATCH");

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int) response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<IncidentUpdate>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            return objectResponse.Object;
          }
          else {
            var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            throw new ApiException(
              "The HTTP status code of the response was not expected (" + status + ").", status,
              responseData, headers, null);
          }
        }
        finally {
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Update a previous incident update
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="incidentId">Incident Identifier</param>
    /// <param name="incidentUpdateId">Incident Update Identifier</param>
    /// <param name="body"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Update a previous incident update</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<IncidentUpdate> PutIncidentUpdate(string pageId, string incidentId,
      string incidentUpdateId, PutPagesPageIdIncidentsIncidentIdIncidentUpdates body,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (incidentId == null)
        throw new ArgumentNullException(nameof(incidentId));

      if (incidentUpdateId == null)
        throw new ArgumentNullException(nameof(incidentUpdateId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/incidents/{incident_id}/incident_updates/{incident_update_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{incident_id}",
        Uri.EscapeDataString(ConvertToString(incidentId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{incident_update_id}",
        Uri.EscapeDataString(ConvertToString(incidentUpdateId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        var json = JsonConvert.SerializeObject(body, settings.Value);
        var content = new StringContent(json);
        content.Headers.ContentType =
          MediaTypeHeaderValue.Parse("application/json");
        request.Content = content;
        request.Method = new HttpMethod("PUT");

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int) response.StatusCode;
          if (status == 200) {
            var objectResponse = await ReadObjectResponseAsync<IncidentUpdate>(response, headers, cancellationToken)
              .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            return objectResponse.Object;
          }
          else {
            var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            throw new ApiException(
              "The HTTP status code of the response was not expected (" + status + ").", status,
              responseData, headers, null);
          }
        }
        finally {
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Create an incident subscriber
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="incidentId">Incident Identifier</param>
    /// <param name="body"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Create an incident subscriber</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Subscriber> PostIncidentSubscribers(string pageId, string incidentId,
      PostPagesPageIdIncidentsIncidentIdSubscribers body,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (incidentId == null)
        throw new ArgumentNullException(nameof(incidentId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/incidents/{incident_id}/subscribers");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{incident_id}",
        Uri.EscapeDataString(ConvertToString(incidentId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        var json = JsonConvert.SerializeObject(body, settings.Value);
        var content = new StringContent(json);
        content.Headers.ContentType =
          MediaTypeHeaderValue.Parse("application/json");
        request.Content = content;
        request.Method = new HttpMethod("POST");

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int) response.StatusCode;
          switch (status) {
            case 201: {
              var objectResponse = await ReadObjectResponseAsync<Subscriber>(response, headers, cancellationToken)
                .ConfigureAwait(false);
              if (objectResponse.Object == null) {
                throw new ApiException("Response was null which was not expected.", status,
                  objectResponse.Text, headers, null);
              }

              return objectResponse.Object;
            }
            case 400: {
              var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
              if (objectResponse.Object == null)
                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text,
                  headers, null);
              throw new ApiException<ErrorEntity>("Bad request", status, objectResponse.Text, headers,
                objectResponse.Object, null);
            }
            case 401: {
              var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
              if (objectResponse.Object == null)
                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text,
                  headers, null);
              throw new ApiException<ErrorEntity>("Could not authenticate", status, objectResponse.Text, headers,
                objectResponse.Object, null);
            }
            case 404: {
              var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
              if (objectResponse.Object == null)
                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text,
                  headers, null);
              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status,
                objectResponse.Text, headers, objectResponse.Object, null);
            }
            default: {
              var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status,
                responseData, headers, null);
            }
          }
        }
        finally {
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Get a list of incident subscribers
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="incidentId">Incident Identifier</param>
    /// <param name="page">Page offset to fetch. Beginning February 28, 2023, this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <param name="perPage">Number of results to return per page. Beginning February 28, 2023, a default and maximum limit of 100 will be imposed and this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Get a list of incident subscribers</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Subscriber[]> GetIncidentSubscribers(string pageId, string incidentId, int? page = null,
      int? perPage = null, CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (incidentId == null)
        throw new ArgumentNullException(nameof(incidentId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/incidents/{incident_id}/subscribers?");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{incident_id}",
        Uri.EscapeDataString(ConvertToString(incidentId,
          CultureInfo.InvariantCulture)));
      if (page != null) {
        urlBuilder.Append(Uri.EscapeDataString("page") + "=")
          .Append(Uri.EscapeDataString(ConvertToString(page,
            CultureInfo.InvariantCulture))).Append("&");
      }

      if (perPage != null) {
        urlBuilder.Append(Uri.EscapeDataString("per_page") + "=")
          .Append(Uri.EscapeDataString(ConvertToString(perPage,
            CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder.Length--;

      using (var request = new HttpRequestMessage()) {
        request.Method = new HttpMethod("GET");

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int) response.StatusCode;
          switch (status) {
            case 200: {
              var objectResponse = await ReadObjectResponseAsync<Subscriber[]>(response, headers, cancellationToken)
                .ConfigureAwait(false);
              return objectResponse.Object ?? throw new ApiException("Response was null which was not expected.",
                status, objectResponse.Text, headers, null);
            }
            case 401: {
              var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
              if (objectResponse.Object == null)
                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text,
                  headers, null);
              throw new ApiException<ErrorEntity>("Could not authenticate", status, objectResponse.Text, headers,
                objectResponse.Object, null);
            }
            case 404: {
              var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
              if (objectResponse.Object == null)
                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text,
                  headers, null);
              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status,
                objectResponse.Text, headers, objectResponse.Object, null);
            }
            default: {
              var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status,
                responseData, headers, null);
            }
          }
        }
        finally {
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Unsubscribe an incident subscriber
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="incidentId">Incident Identifier</param>
    /// <param name="subscriberId">Subscriber Identifier</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Unsubscribe an incident subscriber</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Subscriber> DeleteIncidentSubscriber(string pageId, string incidentId,
      string subscriberId, CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (incidentId == null)
        throw new ArgumentNullException(nameof(incidentId));

      if (subscriberId == null)
        throw new ArgumentNullException(nameof(subscriberId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/incidents/{incident_id}/subscribers/{subscriber_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{incident_id}",
        Uri.EscapeDataString(ConvertToString(incidentId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{subscriber_id}",
        Uri.EscapeDataString(ConvertToString(subscriberId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        request.Method = new HttpMethod("DELETE");

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int) response.StatusCode;
          if (status == 200) {
            var objectResponse = await ReadObjectResponseAsync<Subscriber>(response, headers, cancellationToken)
              .ConfigureAwait(false);
            return objectResponse.Object ?? throw new ApiException("Response was null which was not expected.", status,
              objectResponse.Text, headers, null);
          }
          else {
            var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status,
              responseData, headers, null);
          }
        }
        finally {
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Get an incident subscriber
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="incidentId">Incident Identifier</param>
    /// <param name="subscriberId">Subscriber Identifier</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Get an incident subscriber</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Subscriber> GetIncidentSubscriber(string pageId, string incidentId, string subscriberId,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (incidentId == null)
        throw new ArgumentNullException(nameof(incidentId));

      if (subscriberId == null)
        throw new ArgumentNullException(nameof(subscriberId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/incidents/{incident_id}/subscribers/{subscriber_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{incident_id}",
        Uri.EscapeDataString(ConvertToString(incidentId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{subscriber_id}",
        Uri.EscapeDataString(ConvertToString(subscriberId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        request.Method = new HttpMethod("GET");

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int) response.StatusCode;
          if (status == 200) {
            var objectResponse = await ReadObjectResponseAsync<Subscriber>(response, headers, cancellationToken)
              .ConfigureAwait(false);
            return objectResponse.Object ?? throw new ApiException("Response was null which was not expected.", status,
              objectResponse.Text, headers, null);
          }
          else {
            var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status,
              responseData, headers, null);
          }
        }
        finally {
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Resend confirmation to an incident subscriber
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="incidentId">Incident Identifier</param>
    /// <param name="subscriberId">Subscriber Identifier</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Resend confirmation to an incident subscriber</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task PostIncidentSubscriberResendConfirmation(string pageId, string incidentId,
      string subscriberId, CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (incidentId == null)
        throw new ArgumentNullException(nameof(incidentId));

      if (subscriberId == null)
        throw new ArgumentNullException(nameof(subscriberId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append(
        "/pages/{page_id}/incidents/{incident_id}/subscribers/{subscriber_id}/resend_confirmation");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{incident_id}",
        Uri.EscapeDataString(ConvertToString(incidentId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{subscriber_id}",
        Uri.EscapeDataString(ConvertToString(subscriberId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        request.Content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
        request.Method = new HttpMethod("POST");

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int) response.StatusCode;
          if (status != 201) {
            var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status,
              responseData, headers, null);
          }
        }
        finally {
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Get Postmortem
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="incidentId">Incident Identifier</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Get Postmortem</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Postmortem> GetIncidentPostmortem(string pageId, string incidentId,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (incidentId == null)
        throw new ArgumentNullException(nameof(incidentId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/incidents/{incident_id}/postmortem");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{incident_id}",
        Uri.EscapeDataString(ConvertToString(incidentId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        request.Method = new HttpMethod("GET");

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int) response.StatusCode;
          if (status == 200) {
            var objectResponse = await ReadObjectResponseAsync<Postmortem>(response, headers, cancellationToken)
              .ConfigureAwait(false);
            return objectResponse.Object ?? throw new ApiException("Response was null which was not expected.", status,
              objectResponse.Text, headers, null);
          }
          else if (status == 401) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
              .ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers,
                null);
            throw new ApiException<ErrorEntity>("Could not authenticate", status, objectResponse.Text, headers,
              objectResponse.Object, null);
          }
          else if (status == 404) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
              .ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers,
                null);
            throw new ApiException<ErrorEntity>("The requested resource could not be found.", status,
              objectResponse.Text, headers, objectResponse.Object, null);
          }
          else {
            var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status,
              responseData, headers, null);
          }
        }
        finally {
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Create Postmortem
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="incidentId">Incident Identifier</param>
    /// <param name="body"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Create Postmortem</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Postmortem> PutIncidentPostmortem(
      string pageId, string incidentId, PutPagesPageIdIncidentsIncidentIdPostmortem body,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (incidentId == null)
        throw new ArgumentNullException(nameof(incidentId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/incidents/{incident_id}/postmortem");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{incident_id}",
        Uri.EscapeDataString(ConvertToString(incidentId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        var json = JsonConvert.SerializeObject(body, settings.Value);
        var content = new StringContent(json);
        content.Headers.ContentType =
          MediaTypeHeaderValue.Parse("application/json");
        request.Content = content;
        request.Method = new HttpMethod("PUT");

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int) response.StatusCode;
          if (status == 200) {
            var objectResponse = await ReadObjectResponseAsync<Postmortem>(response, headers, cancellationToken)
              .ConfigureAwait(false);
            return objectResponse.Object ?? throw new ApiException("Response was null which was not expected.", status,
              objectResponse.Text, headers, null);
          }
          else if (status == 400) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
              .ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers,
                null);
            throw new ApiException<ErrorEntity>("Bad request", status, objectResponse.Text, headers,
              objectResponse.Object, null);
          }
          else if (status == 401) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
              .ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers,
                null);
            throw new ApiException<ErrorEntity>("Could not authenticate", status, objectResponse.Text, headers,
              objectResponse.Object, null);
          }
          else {
            var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status,
              responseData, headers, null);
          }
        }
        finally {
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Delete Postmortem
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="incidentId">Incident Identifier</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Delete Postmortem</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task DeleteIncidentPostmortem(
      string pageId, string incidentId, CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (incidentId == null)
        throw new ArgumentNullException(nameof(incidentId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/incidents/{incident_id}/postmortem");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{incident_id}",
        Uri.EscapeDataString(ConvertToString(incidentId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        request.Method = new HttpMethod("DELETE");

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int) response.StatusCode;
          if (status == 204) {
          }
          else if (status == 401) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
              .ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers,
                null);
            throw new ApiException<ErrorEntity>("Could not authenticate", status, objectResponse.Text, headers,
              objectResponse.Object, null);
          }
          else if (status == 404) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
              .ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers,
                null);
            throw new ApiException<ErrorEntity>("The requested resource could not be found.", status,
              objectResponse.Text, headers, objectResponse.Object, null);
          }
          else {
            var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status,
              responseData, headers, null);
          }
        }
        finally {
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Publish Postmortem
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="incidentId">Incident Identifier</param>
    /// <param name="body"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Publish Postmortem</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Postmortem> PutIncidentPostmortemPublish(string pageId, string incidentId,
      PutPagesPageIdIncidentsIncidentIdPostmortemPublish body,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (incidentId == null)
        throw new ArgumentNullException(nameof(incidentId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/incidents/{incident_id}/postmortem/publish");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{incident_id}",
        Uri.EscapeDataString(ConvertToString(incidentId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        var json = JsonConvert.SerializeObject(body, settings.Value);
        var content = new StringContent(json);
        content.Headers.ContentType =
          MediaTypeHeaderValue.Parse("application/json");
        request.Content = content;
        request.Method = new HttpMethod("PUT");

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int) response.StatusCode;
          if (status == 200) {
            var objectResponse = await ReadObjectResponseAsync<Postmortem>(response, headers, cancellationToken)
              .ConfigureAwait(false);
            return objectResponse.Object ?? throw new ApiException("Response was null which was not expected.", status,
              objectResponse.Text, headers, null);
          }
          else if (status == 400) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
              .ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers,
                null);
            throw new ApiException<ErrorEntity>("Bad request", status, objectResponse.Text, headers,
              objectResponse.Object, null);
          }
          else if (status == 401) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
              .ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers,
                null);
            throw new ApiException<ErrorEntity>("Could not authenticate", status, objectResponse.Text, headers,
              objectResponse.Object, null);
          }
          else if (status == 404) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
              .ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers,
                null);
            throw new ApiException<ErrorEntity>("The requested resource could not be found.", status,
              objectResponse.Text, headers, objectResponse.Object, null);
          }
          else {
            var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status,
              responseData, headers, null);
          }
        }
        finally {
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Revert Postmortem
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="incidentId">Incident Identifier</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Revert Postmortem</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Postmortem> PutIncidentPostmortemRevert(string pageId, string incidentId,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (incidentId == null)
        throw new ArgumentNullException(nameof(incidentId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/incidents/{incident_id}/postmortem/revert");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{incident_id}",
        Uri.EscapeDataString(ConvertToString(incidentId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        request.Content =
          new StringContent(string.Empty, Encoding.UTF8, "application/json");
        request.Method = new HttpMethod("PUT");

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int) response.StatusCode;
          if (status == 200) {
            var objectResponse = await ReadObjectResponseAsync<Postmortem>(response, headers, cancellationToken)
              .ConfigureAwait(false);
            return objectResponse.Object ?? throw new ApiException("Response was null which was not expected.", status,
              objectResponse.Text, headers, null);
          }
          else if (status == 400) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
              .ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers,
                null);
            throw new ApiException<ErrorEntity>("Bad request", status, objectResponse.Text, headers,
              objectResponse.Object, null);
          }
          else if (status == 401) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
              .ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers,
                null);
            throw new ApiException<ErrorEntity>("Could not authenticate", status, objectResponse.Text, headers,
              objectResponse.Object, null);
          }
          else if (status == 404) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
              .ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers,
                null);
            throw new ApiException<ErrorEntity>("The requested resource could not be found.", status,
              objectResponse.Text, headers, objectResponse.Object, null);
          }
          else {
            var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status,
              responseData, headers, null);
          }
        }
        finally {
          response.Dispose();
        }
      }
    }
  }
}