using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    /// Create a component
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="body"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Create a component</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Component> PostComponents(string pageId, PostPagesPageIdComponents body,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/pages/{page_id}/components");
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
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int) response.StatusCode;
          if (status == 201) {
            var objectResponse = await ReadObjectResponseAsync<Component>(response, headers, cancellationToken)
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
            throw new ApiException<ErrorEntity>("Could not authenticate", status, objectResponse.Text,
              headers, objectResponse.Object, null);
          }
          else if (status == 422) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
              .ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers,
                null);
            throw new ApiException<ErrorEntity>("Unprocessable entity", status, objectResponse.Text, headers,
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
    /// Get a list of components
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="page">Page offset to fetch. Beginning February 28, 2023, this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <param name="perPage">Number of results to return per page. Beginning February 28, 2023, a default and maximum limit of 100 will be imposed and this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Get a list of components</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Component[]> GetPagesComponents(string pageId, int? page = null, int? perPage = null,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/pages/{page_id}/components?");
      urlBuilder.Replace("{page_id}", Uri.EscapeDataString(ConvertToString(pageId, CultureInfo.InvariantCulture)));
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

        var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int) response.StatusCode;
          if (status == 200) {
            var objectResponse = await ReadObjectResponseAsync<Component[]>(response, headers, cancellationToken)
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
    /// Update a component
    /// </summary>
    /// <remarks>
    /// If group_id is "null" then the component will be removed from a group.
    /// </remarks>
    /// <param name="pageId">Page identifier</param>
    /// <param name="componentId">Component identifier</param>
    /// <param name="body"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Update a component</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Component> PatchComponent(string pageId, string componentId,
      PatchPagesPageIdComponents body,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (componentId == null)
        throw new ArgumentNullException(nameof(componentId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/components/{component_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{component_id}",
        Uri.EscapeDataString(ConvertToString(componentId,
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
              await ReadObjectResponseAsync<Component>(response, headers, cancellationToken)
                .ConfigureAwait(false);
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
    /// Update a component
    /// </summary>
    /// <remarks>
    /// If group_id is "null" then the component will be removed from a group.
    /// </remarks>
    /// <param name="pageId">Page identifier</param>
    /// <param name="componentId">Component identifier</param>
    /// <param name="body"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Update a component</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Component> PutComponent(string pageId, string componentId, PutPagesPageIdComponents body,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (componentId == null)
        throw new ArgumentNullException(nameof(componentId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/components/{component_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{component_id}",
        Uri.EscapeDataString(ConvertToString(componentId,
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
            var objectResponse = await ReadObjectResponseAsync<Component>(response, headers, cancellationToken)
              .ConfigureAwait(false);
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
    /// Delete a component
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="componentId">Component identifier</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Delete a component</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task DeleteComponent(string pageId, string componentId,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (componentId == null)
        throw new ArgumentNullException(nameof(componentId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/components/{component_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{component_id}",
        Uri.EscapeDataString(ConvertToString(componentId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        request.Method = new HttpMethod("DELETE");

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
          if (status == 204) {
          }
          else if (status == 401) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
              .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers,
                null);
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
    /// Get a component
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="componentId">Component identifier</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Get a component</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Component> GetComponent(string pageId, string componentId,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (componentId == null)
        throw new ArgumentNullException(nameof(componentId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/components/{component_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{component_id}",
        Uri.EscapeDataString(ConvertToString(componentId,
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
            var objectResponse =
              await ReadObjectResponseAsync<Component>(response, headers, cancellationToken)
                .ConfigureAwait(false);
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
    /// Get uptime data for a component
    /// </summary>
    /// <remarks>
    /// Get uptime data for a component that has uptime showcase enabled
    /// </remarks>
    /// <param name="pageId">Page identifier</param>
    /// <param name="componentId">Component identifier</param>
    /// <param name="skipRelatedEvents">Skips supplying the related events data along with the component uptime data.</param>
    /// <param name="start">The start date for uptime calculation (defaults to the component's start_date field or 90 days ago, whichever is more recent).
    /// <br/>The maximum supported date range is six calendar months. If the year is given, the date defaults to the first day of the year.
    /// <br/>If the year and month are given, the start date defaults to the first day of that month.
    /// <br/>The earliest supported date is January 1, 1970.</param>
    /// <param name="end">The end date for uptime calculation (defaults to today in the page's time zone). The maximum supported date range is six calendar months.
    /// <br/>If the year is given, the date defaults to the last day of the year. If the year and month are given, the date defaults to the last day of that month.
    /// <br/>The earliest supported date is January 1, 1970.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Get uptime data for a component that has uptime showcase enabled</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<ComponentUptime> GetComponentUptime(string pageId, string componentId,
      bool? skipRelatedEvents, DateTimeOffset? start = null, DateTimeOffset? end = null,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (componentId == null)
        throw new ArgumentNullException(nameof(componentId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/components/{component_id}/uptime?");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{component_id}",
        Uri.EscapeDataString(ConvertToString(componentId,
          CultureInfo.InvariantCulture)));
      if (skipRelatedEvents != null) {
        urlBuilder.Append(Uri.EscapeDataString("skip_related_events") + "=")
          .Append(Uri.EscapeDataString(ConvertToString(skipRelatedEvents,
            CultureInfo.InvariantCulture))).Append("&");
      }

      if (start != null) {
        urlBuilder.Append(Uri.EscapeDataString("start") + "=")
          .Append(Uri.EscapeDataString(ConvertToString(start,
            CultureInfo.InvariantCulture))).Append("&");
      }

      if (end != null) {
        urlBuilder.Append(Uri.EscapeDataString("end") + "=")
          .Append(Uri.EscapeDataString(ConvertToString(end,
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
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int) response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<ComponentUptime>(response, headers, cancellationToken)
                .ConfigureAwait(false);
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
    /// Remove page access users from component
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="componentId">Component identifier</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Remove page access users from component</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Component> DeleteComponentPageAccessUsers(string pageId, string componentId,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (componentId == null)
        throw new ArgumentNullException(nameof(componentId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/components/{component_id}/page_access_users");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{component_id}",
        Uri.EscapeDataString(ConvertToString(componentId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        request.Method = new HttpMethod("DELETE");

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
            var objectResponse = await ReadObjectResponseAsync<Component>(response, headers, cancellationToken)
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
    /// Add page access users to a component
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="componentId">Component identifier</param>
    /// <param name="body"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Add page access users to a component</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Component> PostComponentPageAccessUsers(string pageId, string componentId, Body body,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (componentId == null)
        throw new ArgumentNullException(nameof(componentId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/components/{component_id}/page_access_users");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{component_id}",
        Uri.EscapeDataString(ConvertToString(componentId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        var json = JsonConvert.SerializeObject(body, settings.Value);
        var dictionary =
          JsonConvert
            .DeserializeObject<Dictionary<string, string>>(json,
              settings.Value);
        var content = new FormUrlEncodedContent(dictionary);
        content.Headers.ContentType =
          MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");
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
          if (status == 201) {
            var objectResponse = await ReadObjectResponseAsync<Component>(response, headers, cancellationToken)
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
          else if (status == 422) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
              .ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers,
                null);
            throw new ApiException<ErrorEntity>("Unprocessable entity", status, objectResponse.Text, headers,
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
    /// Remove page access groups from a component
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="componentId">Component identifier</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Remove page access groups from a component</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Component> DeleteComponentPageAccessGroups(string pageId, string componentId,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (componentId == null)
        throw new ArgumentNullException(nameof(componentId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/components/{component_id}/page_access_groups");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{component_id}",
        Uri.EscapeDataString(ConvertToString(componentId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        request.Method = new HttpMethod("DELETE");

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
            var objectResponse = await ReadObjectResponseAsync<Component>(response, headers, cancellationToken)
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
    /// Add page access groups to a component
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="componentId">Component identifier</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Add page access groups to a component</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Component> PostComponentPageAccessGroups(string pageId, string componentId,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (componentId == null)
        throw new ArgumentNullException(nameof(componentId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/components/{component_id}/page_access_groups");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{component_id}",
        Uri.EscapeDataString(ConvertToString(componentId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        request.Content =
          new StringContent(string.Empty, Encoding.UTF8, "application/json");
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
              await ReadObjectResponseAsync<Component>(response, headers, cancellationToken)
                .ConfigureAwait(false);
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
    /// Create a component group
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="body"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Create a component group</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<GroupComponent> PostComponentGroups(string pageId, PostPagesPageIdComponentGroups body,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/pages/{page_id}/component-groups");
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
              await ReadObjectResponseAsync<GroupComponent>(response, headers, cancellationToken)
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
    /// Get a list of component groups
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="page">Page offset to fetch. Beginning February 28, 2023, this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <param name="perPage">Number of results to return per page. Beginning February 28, 2023, a default and maximum limit of 100 will be imposed and this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Get a list of component groups</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<GroupComponent[]> GetComponentGroups(string pageId, int? page = null, int? perPage = null,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/component-groups?");
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

      var disposeClient = false;
      try {
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
                await ReadObjectResponseAsync<GroupComponent[]>(
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
      finally {
        if (disposeClient)
          httpClient.Dispose();
      }
    }

    /// <summary>
    /// Update a component group
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="id">Component group identifier</param>
    /// <param name="body"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Update a component group</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<GroupComponent> PatchComponentGroup(string pageId, string id,
      PatchPagesPageIdComponentGroups body, CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (id == null)
        throw new ArgumentNullException(nameof(id));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/component-groups/{id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{id}",
        Uri.EscapeDataString(ConvertToString(id, CultureInfo.InvariantCulture)));

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
              await ReadObjectResponseAsync<GroupComponent>(response, headers, cancellationToken)
                .ConfigureAwait(false);
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
    /// Update a component group
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="id">Component group identifier</param>
    /// <param name="body"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Update a component group</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<GroupComponent> PutComponentGroup(string pageId, string id,
      PutPagesPageIdComponentGroups body, CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (id == null)
        throw new ArgumentNullException(nameof(id));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/component-groups/{id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{id}",
        Uri.EscapeDataString(ConvertToString(id, CultureInfo.InvariantCulture)));

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
          if (response.Content?.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int) response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<GroupComponent>(response, headers, cancellationToken)
                .ConfigureAwait(false);
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
    /// Delete a component group
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="id">Component group identifier</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Delete a component group</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<GroupComponent> DeleteComponentGroup(string pageId, string id,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (id == null)
        throw new ArgumentNullException(nameof(id));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/component-groups/{id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{id}",
        Uri.EscapeDataString(ConvertToString(id, CultureInfo.InvariantCulture)));

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
              await ReadObjectResponseAsync<GroupComponent>(response, headers, cancellationToken)
                .ConfigureAwait(false);
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
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Get a component group
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="id">Component group identifier</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Get a component group</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<GroupComponent> GetComponentGroup(string pageId, string id,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (id == null)
        throw new ArgumentNullException(nameof(id));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/component-groups/{id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{id}",
        Uri.EscapeDataString(ConvertToString(id, CultureInfo.InvariantCulture)));

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
              await ReadObjectResponseAsync<GroupComponent>(response, headers, cancellationToken)
                .ConfigureAwait(false);
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
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Get uptime data for a component group
    /// </summary>
    /// <remarks>
    /// Get uptime data for a component group that has uptime showcase enabled for at least one component.
    /// </remarks>
    /// <param name="pageId">Page identifier</param>
    /// <param name="id">Component group identifier</param>
    /// <param name="skipRelatedEvents">Skips supplying the related events data along with the component uptime data.</param>
    /// <param name="start">The start date for uptime calculation (defaults to the date of the component in the group with the earliest start_date, or 90 days ago, whichever is more recent).
    /// <br/>The maximum supported date range is six calendar months. If the year is given, the date defaults to the first day of the year.
    /// <br/>If the year and month are given, the start date defaults to the first day of that month.
    /// <br/>The earliest supported date is January 1, 1970.</param>
    /// <param name="end">The end date for uptime calculation (defaults to today in the page's time zone). The maximum supported date range is six calendar months.
    /// <br/>If the year is given, the date defaults to the last day of the year. If the year and month are given, the date defaults to the last day of that month.
    /// <br/>The earliest supported date is January 1, 1970.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Get uptime data for a component group that has uptime showcase enabled for at least one component.</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<ComponentGroupUptime> GetComponentGroupUptime(string pageId, string id,
      bool? skipRelatedEvents, DateTimeOffset? start = null, DateTimeOffset? end = null,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (id == null)
        throw new ArgumentNullException(nameof(id));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/component-groups/{id}/uptime?");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{id}",
        Uri.EscapeDataString(ConvertToString(id, CultureInfo.InvariantCulture)));
      if (skipRelatedEvents != null) {
        urlBuilder.Append(Uri.EscapeDataString("skip_related_events") + "=")
          .Append(Uri.EscapeDataString(ConvertToString(skipRelatedEvents,
            CultureInfo.InvariantCulture))).Append("&");
      }

      if (start != null) {
        urlBuilder.Append(Uri.EscapeDataString("start") + "=")
          .Append(Uri.EscapeDataString(ConvertToString(start,
            CultureInfo.InvariantCulture))).Append("&");
      }

      if (end != null) {
        urlBuilder.Append(Uri.EscapeDataString("end") + "=")
          .Append(Uri.EscapeDataString(ConvertToString(end,
            CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder.Length--;

      var disposeClient = false;
      try {
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
                await ReadObjectResponseAsync<ComponentGroupUptime>(response, headers,
                  cancellationToken).ConfigureAwait(false);
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
            if (disposeResponse)
              response.Dispose();
          }
        }
      }
      finally {
        if (disposeClient)
          httpClient.Dispose();
      }
    }
  }
}