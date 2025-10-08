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
  public class StatusPageIoApi {
    private readonly HttpClient httpClient;
    private readonly Lazy<JsonSerializerSettings> settings;

    public StatusPageIoApi(string apiKey) {
      httpClient = new HttpClient();
      httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", apiKey);
      httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      settings = new Lazy<JsonSerializerSettings>(CreateSerializerSettings);
    }

    public string BaseUrl { get; set; } = "https://api.statuspage.io/v1";

    protected JsonSerializerSettings JsonSerializerSettings => settings.Value;

    public bool ReadResponseAsString { get; set; }

    private JsonSerializerSettings CreateSerializerSettings() {
      var settings = new JsonSerializerSettings();
      return settings;
    }

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get a list of pages
    /// </summary>
    /// <remarks>
    /// Get a list of pages
    /// </remarks>
    /// <returns>Get a list of pages</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<ICollection<Page>> GetPages(CancellationToken cancellationToken) {
      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/pages");

      using (var request = new HttpRequestMessage()) {
        request.Method = new HttpMethod("GET");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<ICollection<Page>>(response,
                headers, cancellationToken).ConfigureAwait(false);
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

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Update a page
    /// </summary>
    /// <remarks>
    /// Update a page
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Update a page</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Page> PatchPages(string pageId, PatchPages body,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/pages/{page_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        var json = JsonConvert.SerializeObject(body, settings.Value);
        var content = new StringContent(json);
        content.Headers.ContentType =
          MediaTypeHeaderValue.Parse("application/json");
        request.Content = content;
        request.Method = new HttpMethod("PATCH");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<Page>(response, headers, cancellationToken)
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Update a page
    /// </summary>
    /// <remarks>
    /// Update a page
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Update a page</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Page> PutPages(string pageId, PutPages body,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/pages/{page_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        var json = JsonConvert.SerializeObject(body, settings.Value);
        var content = new StringContent(json);
        content.Headers.ContentType =
          MediaTypeHeaderValue.Parse("application/json");
        request.Content = content;
        request.Method = new HttpMethod("PUT");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<Page>(response, headers, cancellationToken)
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get a page
    /// </summary>
    /// <remarks>
    /// Get a page
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Get a page</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Page> GetPages(string pageId,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/pages/{page_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        request.Method = new HttpMethod("GET");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<Page>(response, headers, cancellationToken)
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Add a page access user
    /// </summary>
    /// <remarks>
    /// Add a page access user
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Add a page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessUser> PostPageAccessUsers(
      string pageId, PostPagesPageIdPageAccessUsers body, CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/page_access_users");
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<PageAccessUser>(response, headers, cancellationToken)
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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
          else if (status == 409) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>(
              "The request could not be processed due to a conflict in resource state.", status,
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

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get a list of page access users
    /// </summary>
    /// <remarks>
    /// Get a list of page access users
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="email">Email address to search for</param>
    /// <param name="page">Page offset to fetch. Beginning February 28, 2023, this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <param name="per_page">Number of results to return per page. Beginning February 28, 2023, a default and maximum limit of 100 will be imposed and this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <returns>Get a list of page access users</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<ICollection<PageAccessUser>>
      GetPageAccessUsers(string pageId, string email, int? page, int? perPage,
        CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/page_access_users?");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      if (email != null) {
        urlBuilder.Append(Uri.EscapeDataString("email") + "=")
          .Append(Uri.EscapeDataString(ConvertToString(email,
            CultureInfo.InvariantCulture))).Append("&");
      }

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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<ICollection<PageAccessUser>>(
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Update page access user
    /// </summary>
    /// <remarks>
    /// Update page access user
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page_access_user_id">Page Access User Identifier</param>
    /// <returns>Update page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessUser>
      PatchPageAccessUser(string pageId, string pageAccessUserId,
        CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (pageAccessUserId == null)
        throw new ArgumentNullException(nameof(pageAccessUserId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/page_access_users/{page_access_user_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{page_access_user_id}",
        Uri.EscapeDataString(ConvertToString(pageAccessUserId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        request.Content =
          new StringContent(string.Empty, Encoding.UTF8, "application/json");
        request.Method = new HttpMethod("PATCH");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<PageAccessUser>(response, headers, cancellationToken)
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Update page access user
    /// </summary>
    /// <remarks>
    /// Update page access user
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page_access_user_id">Page Access User Identifier</param>
    /// <returns>Update page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessUser>
      PutPageAccessUser(string pageId, string pageAccessUserId,
        CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (pageAccessUserId == null)
        throw new ArgumentNullException(nameof(pageAccessUserId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/page_access_users/{page_access_user_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{page_access_user_id}",
        Uri.EscapeDataString(ConvertToString(pageAccessUserId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        request.Content =
          new StringContent(string.Empty, Encoding.UTF8, "application/json");
        request.Method = new HttpMethod("PUT");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<PageAccessUser>(response, headers, cancellationToken)
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Delete page access user
    /// </summary>
    /// <remarks>
    /// Delete page access user
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page_access_user_id">Page Access User Identifier</param>
    /// <returns>Delete page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task DeletePageAccessUser(
      string pageId, string pageAccessUserId, CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (pageAccessUserId == null)
        throw new ArgumentNullException(nameof(pageAccessUserId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/page_access_users/{page_access_user_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{page_access_user_id}",
        Uri.EscapeDataString(ConvertToString(pageAccessUserId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        request.Method = new HttpMethod("DELETE");

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 204) {
            return;
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get page access user
    /// </summary>
    /// <remarks>
    /// Get page access user
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page_access_user_id">Page Access User Identifier</param>
    /// <returns>Get page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessUser>
      GetPageAccessUser(string pageId, string pageAccessUserId,
        CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (pageAccessUserId == null)
        throw new ArgumentNullException(nameof(pageAccessUserId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/page_access_users/{page_access_user_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{page_access_user_id}",
        Uri.EscapeDataString(ConvertToString(pageAccessUserId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        request.Method = new HttpMethod("GET");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<PageAccessUser>(response, headers, cancellationToken)
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Add components for page access user
    /// </summary>
    /// <remarks>
    /// Add components for page access user
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page_access_user_id">Page Access User Identifier</param>
    /// <returns>Add components for page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessUser>
      PatchPageAccessUserComponents(string pageId, string pageAccessUserId,
        PatchPagesPageIdPageAccessUsersPageAccessUserIdComponents body,
        CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (pageAccessUserId == null)
        throw new ArgumentNullException(nameof(pageAccessUserId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/page_access_users/{page_access_user_id}/components");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{page_access_user_id}",
        Uri.EscapeDataString(ConvertToString(pageAccessUserId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        var json = JsonConvert.SerializeObject(body, settings.Value);
        var content = new StringContent(json);
        content.Headers.ContentType =
          MediaTypeHeaderValue.Parse("application/json");
        request.Content = content;
        request.Method = new HttpMethod("PATCH");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<PageAccessUser>(response, headers, cancellationToken)
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Add components for page access user
    /// </summary>
    /// <remarks>
    /// Add components for page access user
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page_access_user_id">Page Access User Identifier</param>
    /// <returns>Add components for page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessUser>
      PutPageAccessUserComponents(string pageId, string pageAccessUserId,
        PutPagesPageIdPageAccessUsersPageAccessUserIdComponents body,
        CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (pageAccessUserId == null)
        throw new ArgumentNullException(nameof(pageAccessUserId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/page_access_users/{page_access_user_id}/components");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{page_access_user_id}",
        Uri.EscapeDataString(ConvertToString(pageAccessUserId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        var json = JsonConvert.SerializeObject(body, settings.Value);
        var content = new StringContent(json);
        content.Headers.ContentType =
          MediaTypeHeaderValue.Parse("application/json");
        request.Content = content;
        request.Method = new HttpMethod("PUT");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<PageAccessUser>(response, headers, cancellationToken)
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Replace components for page access user
    /// </summary>
    /// <remarks>
    /// Replace components for page access user
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page_access_user_id">Page Access User Identifier</param>
    /// <returns>Replace components for page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessUser>
      PostPageAccessUserComponents(string pageId, string pageAccessUserId,
        PostPagesPageIdPageAccessUsersPageAccessUserIdComponents body,
        CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (pageAccessUserId == null)
        throw new ArgumentNullException(nameof(pageAccessUserId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/page_access_users/{page_access_user_id}/components");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{page_access_user_id}",
        Uri.EscapeDataString(ConvertToString(pageAccessUserId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        var json = JsonConvert.SerializeObject(body, settings.Value);
        var content = new StringContent(json);
        content.Headers.ContentType =
          MediaTypeHeaderValue.Parse("application/json");
        request.Content = content;
        request.Method = new HttpMethod("POST");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 201) {
            var objectResponse =
              await ReadObjectResponseAsync<PageAccessUser>(response, headers, cancellationToken)
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Remove components for page access user
    /// </summary>
    /// <remarks>
    /// Remove components for page access user
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page_access_user_id">Page Access User Identifier</param>
    /// <returns>Remove components for page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessUser>
      DeletePageAccessUserComponents(string pageId, string pageAccessUserId,
        DeletePagesPageIdPageAccessUsersPageAccessUserIdComponents body,
        CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (pageAccessUserId == null)
        throw new ArgumentNullException(nameof(pageAccessUserId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/page_access_users/{page_access_user_id}/components");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{page_access_user_id}",
        Uri.EscapeDataString(ConvertToString(pageAccessUserId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        var json = JsonConvert.SerializeObject(body, settings.Value);
        var content = new StringContent(json);
        content.Headers.ContentType =
          MediaTypeHeaderValue.Parse("application/json");
        request.Content = content;
        request.Method = new HttpMethod("DELETE");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<PageAccessUser>(response, headers, cancellationToken)
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get components for page access user
    /// </summary>
    /// <remarks>
    /// Get components for page access user
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page_access_user_id">Page Access User Identifier</param>
    /// <param name="page">Page offset to fetch. Beginning February 28, 2023, this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <param name="per_page">Number of results to return per page. Beginning February 28, 2023, a default and maximum limit of 100 will be imposed and this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <returns>Get components for page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<ICollection<Component>>
      GetPageAccessUserComponents(string pageId, string pageAccessUserId,
        int? page, int? perPage, CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (pageAccessUserId == null)
        throw new ArgumentNullException(nameof(pageAccessUserId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/page_access_users/{page_access_user_id}/components?");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{page_access_user_id}",
        Uri.EscapeDataString(ConvertToString(pageAccessUserId,
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<ICollection<Component>>(
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Remove component for page access user
    /// </summary>
    /// <remarks>
    /// Remove component for page access user
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page_access_user_id">Page Access User Identifier</param>
    /// <param name="component_id">Component identifier</param>
    /// <returns>Remove component for page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessUser>
      DeletePageAccessUserComponent(string pageId,
        string pageAccessUserId, string componentId, CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (pageAccessUserId == null)
        throw new ArgumentNullException(nameof(pageAccessUserId));

      if (componentId == null)
        throw new ArgumentNullException(nameof(componentId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/page_access_users/{page_access_user_id}/components/{component_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{page_access_user_id}",
        Uri.EscapeDataString(ConvertToString(pageAccessUserId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{component_id}",
        Uri.EscapeDataString(ConvertToString(componentId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        request.Method = new HttpMethod("DELETE");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<PageAccessUser>(response, headers, cancellationToken)
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Add metrics for page access user
    /// </summary>
    /// <remarks>
    /// Add metrics for page access user
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page_access_user_id">Page Access User Identifier</param>
    /// <returns>Add metrics for page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessUser>
      PatchPageAccessUserMetrics(string pageId, string pageAccessUserId,
        PatchPagesPageIdPageAccessUsersPageAccessUserIdMetrics body,
        CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (pageAccessUserId == null)
        throw new ArgumentNullException(nameof(pageAccessUserId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/page_access_users/{page_access_user_id}/metrics");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{page_access_user_id}",
        Uri.EscapeDataString(ConvertToString(pageAccessUserId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        var json = JsonConvert.SerializeObject(body, settings.Value);
        var content = new StringContent(json);
        content.Headers.ContentType =
          MediaTypeHeaderValue.Parse("application/json");
        request.Content = content;
        request.Method = new HttpMethod("PATCH");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<PageAccessUser>(response, headers, cancellationToken)
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Add metrics for page access user
    /// </summary>
    /// <remarks>
    /// Add metrics for page access user
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page_access_user_id">Page Access User Identifier</param>
    /// <returns>Add metrics for page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessUser>
      PutPageAccessUserMetrics(string pageId, string pageAccessUserId,
        PutPagesPageIdPageAccessUsersPageAccessUserIdMetrics body,
        CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (pageAccessUserId == null)
        throw new ArgumentNullException(nameof(pageAccessUserId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/page_access_users/{page_access_user_id}/metrics");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{page_access_user_id}",
        Uri.EscapeDataString(ConvertToString(pageAccessUserId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        var json = JsonConvert.SerializeObject(body, settings.Value);
        var content = new StringContent(json);
        content.Headers.ContentType =
          MediaTypeHeaderValue.Parse("application/json");
        request.Content = content;
        request.Method = new HttpMethod("PUT");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<PageAccessUser>(response, headers, cancellationToken)
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Replace metrics for page access user
    /// </summary>
    /// <remarks>
    /// Replace metrics for page access user
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page_access_user_id">Page Access User Identifier</param>
    /// <returns>Replace metrics for page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessUser>
      PostPageAccessUserMetrics(string pageId, string pageAccessUserId,
        PostPagesPageIdPageAccessUsersPageAccessUserIdMetrics body,
        CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (pageAccessUserId == null)
        throw new ArgumentNullException(nameof(pageAccessUserId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/page_access_users/{page_access_user_id}/metrics");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{page_access_user_id}",
        Uri.EscapeDataString(ConvertToString(pageAccessUserId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        var json = JsonConvert.SerializeObject(body, settings.Value);
        var content = new StringContent(json);
        content.Headers.ContentType =
          MediaTypeHeaderValue.Parse("application/json");
        request.Content = content;
        request.Method = new HttpMethod("POST");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 201) {
            var objectResponse =
              await ReadObjectResponseAsync<PageAccessUser>(response, headers, cancellationToken)
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Delete metrics for page access user
    /// </summary>
    /// <remarks>
    /// Delete metrics for page access user
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page_access_user_id">Page Access User Identifier</param>
    /// <returns>Delete metrics for page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessUser>
      DeletePageAccessUserMetrics(string pageId, string pageAccessUserId,
        DeletePagesPageIdPageAccessUsersPageAccessUserIdMetrics body,
        CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (pageAccessUserId == null)
        throw new ArgumentNullException(nameof(pageAccessUserId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/page_access_users/{page_access_user_id}/metrics");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{page_access_user_id}",
        Uri.EscapeDataString(ConvertToString(pageAccessUserId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        var json = JsonConvert.SerializeObject(body, settings.Value);
        var content = new StringContent(json);
        content.Headers.ContentType =
          MediaTypeHeaderValue.Parse("application/json");
        request.Content = content;
        request.Method = new HttpMethod("DELETE");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<PageAccessUser>(response, headers, cancellationToken)
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get metrics for page access user
    /// </summary>
    /// <remarks>
    /// Get metrics for page access user
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page_access_user_id">Page Access User Identifier</param>
    /// <param name="page">Page offset to fetch. Beginning February 28, 2023, this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <param name="per_page">Number of results to return per page. Beginning February 28, 2023, a default and maximum limit of 100 will be imposed and this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <returns>Get metrics for page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<ICollection<Metric>>
      GetPageAccessUserMetrics(string pageId, string pageAccessUserId, int? page, int? perPage,
        CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (pageAccessUserId == null)
        throw new ArgumentNullException(nameof(pageAccessUserId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/page_access_users/{page_access_user_id}/metrics?");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{page_access_user_id}",
        Uri.EscapeDataString(ConvertToString(pageAccessUserId,
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<ICollection<Metric>>(response,
                headers, cancellationToken).ConfigureAwait(false);
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Delete metric for page access user
    /// </summary>
    /// <remarks>
    /// Delete metric for page access user
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page_access_user_id">Page Access User Identifier</param>
    /// <param name="metric_id">Identifier of metric requested</param>
    /// <returns>Delete metric for page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessUser>
      DeletePageAccessUserMetric(string pageId,
        string pageAccessUserId,
        string metricId, CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (pageAccessUserId == null)
        throw new ArgumentNullException(nameof(pageAccessUserId));

      if (metricId == null)
        throw new ArgumentNullException(nameof(metricId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/page_access_users/{page_access_user_id}/metrics/{metric_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{page_access_user_id}",
        Uri.EscapeDataString(ConvertToString(pageAccessUserId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{metric_id}",
        Uri.EscapeDataString(ConvertToString(metricId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        request.Method = new HttpMethod("DELETE");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<PageAccessUser>(response, headers, cancellationToken)
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
          if (disposeResponse)
            response.Dispose();
        }
      }
    }


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get a list of page access groups
    /// </summary>
    /// <remarks>
    /// Get a list of page access groups
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page">Page offset to fetch. Beginning February 28, 2023, this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <param name="per_page">Number of results to return per page. Beginning February 28, 2023, a default and maximum limit of 100 will be imposed and this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <returns>Get a list of page access groups</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<ICollection<PageAccessGroup>>
      GetPageAccessGroups(string pageId, int? page, int? perPage, CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/page_access_groups?");
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<ICollection<PageAccessGroup>>(
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Create a page access group
    /// </summary>
    /// <remarks>
    /// Create a page access group
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Create a page access group</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessGroup> PostPageAccessGroups(string pageId, PostPagesPageIdPageAccessGroups body,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/page_access_groups");
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 201) {
            var objectResponse =
              await ReadObjectResponseAsync<PageAccessGroup>(response, headers, cancellationToken)
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get a page access group
    /// </summary>
    /// <remarks>
    /// Get a page access group
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page_access_group_id">Page Access Group Identifier</param>
    /// <returns>Get a page access group</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessGroup> GetPageAccessGroup(string pageId, string pageAccessGroupId,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (pageAccessGroupId == null)
        throw new ArgumentNullException(nameof(pageAccessGroupId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/page_access_groups/{page_access_group_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{page_access_group_id}",
        Uri.EscapeDataString(ConvertToString(pageAccessGroupId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        request.Method = new HttpMethod("GET");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<PageAccessGroup>(response, headers, cancellationToken)
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Update a page access group
    /// </summary>
    /// <remarks>
    /// Update a page access group
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page_access_group_id">Page Access Group Identifier</param>
    /// <returns>Update a page access group</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessGroup>
      PatchPageAccessGroup(string pageId, string pageAccessGroupId, PatchPagesPageIdPageAccessGroups body,
        CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (pageAccessGroupId == null)
        throw new ArgumentNullException(nameof(pageAccessGroupId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/page_access_groups/{page_access_group_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{page_access_group_id}",
        Uri.EscapeDataString(ConvertToString(pageAccessGroupId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        var json = JsonConvert.SerializeObject(body, settings.Value);
        var content = new StringContent(json);
        content.Headers.ContentType =
          MediaTypeHeaderValue.Parse("application/json");
        request.Content = content;
        request.Method = new HttpMethod("PATCH");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<PageAccessGroup>(response, headers, cancellationToken)
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Update a page access group
    /// </summary>
    /// <remarks>
    /// Update a page access group
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page_access_group_id">Page Access Group Identifier</param>
    /// <returns>Update a page access group</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessGroup> PutPageAccessGroup(string pageId, string pageAccessGroupId,
      PutPagesPageIdPageAccessGroups body, CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (pageAccessGroupId == null)
        throw new ArgumentNullException(nameof(pageAccessGroupId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/page_access_groups/{page_access_group_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{page_access_group_id}",
        Uri.EscapeDataString(ConvertToString(pageAccessGroupId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        var json = JsonConvert.SerializeObject(body, settings.Value);
        var content = new StringContent(json);
        content.Headers.ContentType =
          MediaTypeHeaderValue.Parse("application/json");
        request.Content = content;
        request.Method = new HttpMethod("PUT");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<PageAccessGroup>(response, headers, cancellationToken)
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Remove a page access group
    /// </summary>
    /// <remarks>
    /// Remove a page access group
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page_access_group_id">Page Access Group Identifier</param>
    /// <returns>Remove a page access group</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessGroup> DeletePageAccessGroup(string pageId, string pageAccessGroupId,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (pageAccessGroupId == null)
        throw new ArgumentNullException(nameof(pageAccessGroupId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/page_access_groups/{page_access_group_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{page_access_group_id}",
        Uri.EscapeDataString(ConvertToString(pageAccessGroupId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        request.Method = new HttpMethod("DELETE");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<PageAccessGroup>(response, headers, cancellationToken)
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Add components to page access group
    /// </summary>
    /// <remarks>
    /// Add components to page access group
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page_access_group_id">Page Access Group Identifier</param>
    /// <returns>Add components to page access group</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessGroup> PatchPageAccessGroupComponents(string pageId, string pageAccessGroupId,
      PatchPagesPageIdPageAccessGroupsPageAccessGroupIdComponents body, CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (pageAccessGroupId == null)
        throw new ArgumentNullException(nameof(pageAccessGroupId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/page_access_groups/{page_access_group_id}/components");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{page_access_group_id}",
        Uri.EscapeDataString(ConvertToString(pageAccessGroupId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        var json = JsonConvert.SerializeObject(body, settings.Value);
        var content = new StringContent(json);
        content.Headers.ContentType =
          MediaTypeHeaderValue.Parse("application/json");
        request.Content = content;
        request.Method = new HttpMethod("PATCH");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<PageAccessGroup>(response, headers, cancellationToken)
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Add components to page access group
    /// </summary>
    /// <remarks>
    /// Add components to page access group
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page_access_group_id">Page Access Group Identifier</param>
    /// <returns>Add components to page access group</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessGroup> PutPageAccessGroupComponents(string pageId, string pageAccessGroupId,
      PutPagesPageIdPageAccessGroupsPageAccessGroupIdComponents body,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (pageAccessGroupId == null)
        throw new ArgumentNullException(nameof(pageAccessGroupId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/page_access_groups/{page_access_group_id}/components");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{page_access_group_id}",
        Uri.EscapeDataString(ConvertToString(pageAccessGroupId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        var json = JsonConvert.SerializeObject(body, settings.Value);
        var content = new StringContent(json);
        content.Headers.ContentType =
          MediaTypeHeaderValue.Parse("application/json");
        request.Content = content;
        request.Method = new HttpMethod("PUT");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<PageAccessGroup>(response, headers, cancellationToken)
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Replace components for a page access group
    /// </summary>
    /// <remarks>
    /// Replace components for a page access group
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page_access_group_id">Page Access Group Identifier</param>
    /// <returns>Replace components for a page access group</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessGroup> PostPageAccessGroupComponents(string pageId, string pageAccessGroupId,
      PostPagesPageIdPageAccessGroupsPageAccessGroupIdComponents body,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (pageAccessGroupId == null)
        throw new ArgumentNullException(nameof(pageAccessGroupId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/page_access_groups/{page_access_group_id}/components");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{page_access_group_id}",
        Uri.EscapeDataString(ConvertToString(pageAccessGroupId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        var json = JsonConvert.SerializeObject(body, settings.Value);
        var content = new StringContent(json);
        content.Headers.ContentType =
          MediaTypeHeaderValue.Parse("application/json");
        request.Content = content;
        request.Method = new HttpMethod("POST");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 201) {
            var objectResponse =
              await ReadObjectResponseAsync<PageAccessGroup>(response, headers, cancellationToken)
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Delete components for a page access group
    /// </summary>
    /// <remarks>
    /// Delete components for a page access group
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page_access_group_id">Page Access Group Identifier</param>
    /// <returns>Delete components for a page access group</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessGroup> DeletePageAccessGroupComponents(string pageId,
      string pageAccessGroupId, DeletePagesPageIdPageAccessGroupsPageAccessGroupIdComponents body,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (pageAccessGroupId == null)
        throw new ArgumentNullException(nameof(pageAccessGroupId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/page_access_groups/{page_access_group_id}/components");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{page_access_group_id}",
        Uri.EscapeDataString(ConvertToString(pageAccessGroupId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        var json = JsonConvert.SerializeObject(body, settings.Value);
        var content = new StringContent(json);
        content.Headers.ContentType =
          MediaTypeHeaderValue.Parse("application/json");
        request.Content = content;
        request.Method = new HttpMethod("DELETE");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<PageAccessGroup>(response, headers, cancellationToken)
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// List components for a page access group
    /// </summary>
    /// <remarks>
    /// List components for a page access group
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page_access_group_id">Page Access Group Identifier</param>
    /// <param name="page">Page offset to fetch. Beginning February 28, 2023, this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <param name="per_page">Number of results to return per page. Beginning February 28, 2023, a default and maximum limit of 100 will be imposed and this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <returns>List components for a page access group</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<ICollection<Component>> GetPageAccessGroupComponents(string pageId,
      string pageAccessGroupId,
      int? page, int? perPage, CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (pageAccessGroupId == null)
        throw new ArgumentNullException(nameof(pageAccessGroupId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/page_access_groups/{page_access_group_id}/components?");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{page_access_group_id}",
        Uri.EscapeDataString(ConvertToString(pageAccessGroupId,
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

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

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<ICollection<Component>>(
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Remove a component from a page access group
    /// </summary>
    /// <remarks>
    /// Remove a component from a page access group
    /// </remarks>
    /// <param name="pageId">Page identifier</param>
    /// <param name="pageAccessGroupId">Page Access Group Identifier</param>
    /// <param name="componentId">Component identifier</param>
    /// <returns>Remove a component from a page access group</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessGroup> DeletePageAccessGroupComponent(string pageId, string pageAccessGroupId,
      string componentId,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (pageAccessGroupId == null)
        throw new ArgumentNullException(nameof(pageAccessGroupId));

      if (componentId == null)
        throw new ArgumentNullException(nameof(componentId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/page_access_groups/{page_access_group_id}/components/{component_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{page_access_group_id}",
        Uri.EscapeDataString(ConvertToString(pageAccessGroupId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{component_id}",
        Uri.EscapeDataString(ConvertToString(componentId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        request.Method = new HttpMethod("DELETE");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<PageAccessGroup>(response, headers, cancellationToken)
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Resend confirmations to a list of subscribers
    /// </summary>
    /// <remarks>
    /// Resend confirmations to a list of subscribers
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Resend confirmations to a list of subscribers</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task PostSubscribersResendConfirmation(string pageId,
      PostPagesPageIdSubscribersResendConfirmation body,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/subscribers/resend_confirmation");
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
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 201) {
            return;
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
          if (disposeResponse)
            response.Dispose();
        }
      }
    }


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Unsubscribe a list of subscribers
    /// </summary>
    /// <remarks>
    /// Unsubscribe a list of subscribers
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Unsubscribe a list of subscribers</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task PostSubscribersUnsubscribe(string pageId, PostPagesPageIdSubscribersUnsubscribe body,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/subscribers/unsubscribe");
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
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 201) {
            return;
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
          if (disposeResponse)
            response.Dispose();
        }
      }
    }


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Reactivate a list of subscribers
    /// </summary>
    /// <remarks>
    /// Reactivate a list of quarantined subscribers
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Reactivate a list of quarantined subscribers</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task PostSubscribersReactivate(string pageId, PostPagesPageIdSubscribersReactivate body,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/subscribers/reactivate");
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
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 201) {
            return;
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
          if (disposeResponse)
            response.Dispose();
        }
      }
    }


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get a histogram of subscribers by type and then state
    /// </summary>
    /// <remarks>
    /// Get a histogram of subscribers by type and then state
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Get a histogram of subscribers by type and then state</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<SubscriberCountByTypeAndState> GetSubscribersHistogramByState(string pageId,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/subscribers/histogram_by_state");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        request.Method = new HttpMethod("GET");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<SubscriberCountByTypeAndState>(response, headers,
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get a count of subscribers by type
    /// </summary>
    /// <remarks>
    /// Get a count of subscribers by type
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="type">If this is present, only count subscribers of this type.</param>
    /// <param name="state">If this is present, only count subscribers in this state. Specify state "all" to count subscribers in any states.</param>
    /// <returns>Get a count of subscribers by type</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<SubscriberCountByType> GetSubscribersCount(string pageId, Type? type, State? state,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/subscribers/count?");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      if (type != null) {
        urlBuilder.Append(Uri.EscapeDataString("type") + "=")
          .Append(Uri.EscapeDataString(ConvertToString(type,
            CultureInfo.InvariantCulture))).Append("&");
      }

      if (state != null) {
        urlBuilder.Append(Uri.EscapeDataString("state") + "=")
          .Append(Uri.EscapeDataString(ConvertToString(state,
            CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder.Length--;

      using (var request = new HttpRequestMessage()) {
        request.Method = new HttpMethod("GET");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<SubscriberCountByType>(response, headers,
                cancellationToken).ConfigureAwait(false);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get a list of unsubscribed subscribers
    /// </summary>
    /// <remarks>
    /// Get a list of unsubscribed subscribers
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page">Page offset to fetch. Beginning February 28, 2023, this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <param name="per_page">Number of results to return per page. Beginning February 28, 2023, a default and maximum limit of 100 will be imposed and this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <returns>Get a list of unsubscribed subscribers</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<ICollection<Subscriber>> GetSubscribersUnsubscribed(string pageId, int? page,
      int? perPage,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/subscribers/unsubscribed?");
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<ICollection<Subscriber>>(
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Create a subscriber
    /// </summary>
    /// <remarks>
    /// Create a subscriber. Not applicable for Slack subscribers.
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Create a subscriber. Not applicable for Slack subscribers.</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Subscriber> PostSubscribers(string pageId, PostPagesPageIdSubscribers body,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/pages/{page_id}/subscribers");
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 201) {
            var objectResponse =
              await ReadObjectResponseAsync<Subscriber>(response, headers, cancellationToken)
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get a list of subscribers
    /// </summary>
    /// <remarks>
    /// Get a list of subscribers
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="q">If this is specified, search the contact information (email, endpoint, or phone number) for the provided value. This parameter doesn’t support searching for Slack subscribers.</param>
    /// <param name="type">If specified, only return subscribers of the indicated type.</param>
    /// <param name="state">If this is present, only return subscribers in this state. Specify state "all" to find subscribers in any states.</param>
    /// <param name="limit">The maximum number of rows to return. If a text query string is specified (q=), the default and maximum limit is 100. If the text query string is not specified, the default and maximum limit are not set, and not providing a limit will return all the subscribers. Beginning February 28, 2023, a default limit of 100 will be imposed and this endpoint will return paginated data (i.e. will no longer return all subscribers) even if this query parameter is not provided.</param>
    /// <param name="page">The page offset of subscribers. The first page is page 0, the second page 1, etc. This skips page * limit subscribers. Beginning February 28, 2023, this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <param name="sort_field">The field on which to sort: 'primary' to indicate sorting by the identifying field, 'created_at' for sorting by creation timestamp, 'quarantined_at' for sorting by quarantine timestamp, and 'relevance' which sorts by the relevancy of the search text. 'relevance' is not a valid parameter if no search text is supplied.</param>
    /// <param name="sort_direction">The sort direction of the listing.</param>
    /// <returns>Get a list of subscribers</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<ICollection<Subscriber>> GetSubscribers(string pageId, string q, Type? type, State? state,
      int? limit, int? page,
      SortField? sortField, SortDirection? sortDirection,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/pages/{page_id}/subscribers?");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      if (q != null) {
        urlBuilder.Append(Uri.EscapeDataString("q") + "=")
          .Append(Uri.EscapeDataString(ConvertToString(q,
            CultureInfo.InvariantCulture))).Append("&");
      }

      if (type != null) {
        urlBuilder.Append(Uri.EscapeDataString("type") + "=")
          .Append(Uri.EscapeDataString(ConvertToString(type,
            CultureInfo.InvariantCulture))).Append("&");
      }

      if (state != null) {
        urlBuilder.Append(Uri.EscapeDataString("state") + "=")
          .Append(Uri.EscapeDataString(ConvertToString(state,
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

      if (sortField != null) {
        urlBuilder.Append(Uri.EscapeDataString("sort_field") + "=")
          .Append(Uri.EscapeDataString(ConvertToString(sortField,
            CultureInfo.InvariantCulture))).Append("&");
      }

      if (sortDirection != null) {
        urlBuilder.Append(Uri.EscapeDataString("sort_direction") + "=")
          .Append(Uri.EscapeDataString(ConvertToString(sortDirection,
            CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder.Length--;

      using (var request = new HttpRequestMessage()) {
        request.Method = new HttpMethod("GET");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<ICollection<Subscriber>>(
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
          else if (status == 403) {
            var objectResponse =
              await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                .ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status,
                objectResponse.Text, headers, null);
            }

            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
              status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Resend confirmation to a subscriber
    /// </summary>
    /// <remarks>
    /// Resend confirmation to a subscriber
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="subscriber_id">Subscriber Identifier</param>
    /// <returns>Resend confirmation to a subscriber</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task PostSubscriberResendConfirmation(string pageId, string subscriberId,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (subscriberId == null)
        throw new ArgumentNullException(nameof(subscriberId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/subscribers/{subscriber_id}/resend_confirmation");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{subscriber_id}",
        Uri.EscapeDataString(ConvertToString(subscriberId,
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
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 201) {
            return;
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Unsubscribe a subscriber
    /// </summary>
    /// <remarks>
    /// Unsubscribe a subscriber
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="subscriber_id">Subscriber Identifier</param>
    /// <param name="skip_unsubscription_notification">If skip_unsubscription_notification is true, the subscriber does not receive any notifications when they are unsubscribed.</param>
    /// <returns>Unsubscribe a subscriber</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Subscriber> DeleteSubscriber(string pageId, string subscriberId,
      bool? skipUnsubscriptionNotification,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (subscriberId == null)
        throw new ArgumentNullException(nameof(subscriberId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/subscribers/{subscriber_id}?");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{subscriber_id}",
        Uri.EscapeDataString(ConvertToString(subscriberId,
          CultureInfo.InvariantCulture)));
      if (skipUnsubscriptionNotification != null) {
        urlBuilder.Append(Uri.EscapeDataString("skip_unsubscription_notification") + "=")
          .Append(Uri.EscapeDataString(ConvertToString(skipUnsubscriptionNotification,
            CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder.Length--;

      using (var request = new HttpRequestMessage()) {
        request.Method = new HttpMethod("DELETE");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<Subscriber>(response, headers, cancellationToken)
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
          if (disposeResponse)
            response.Dispose();
        }
      }
    }


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Update a subscriber
    /// </summary>
    /// <remarks>
    /// Update a subscriber
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="subscriber_id">Subscriber Identifier</param>
    /// <returns>Update a subscriber</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Subscriber> PatchSubscriber(string pageId, string subscriberId,
      PatchPagesPageIdSubscribers body,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (subscriberId == null)
        throw new ArgumentNullException(nameof(subscriberId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/subscribers/{subscriber_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{subscriber_id}",
        Uri.EscapeDataString(ConvertToString(subscriberId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        var json = JsonConvert.SerializeObject(body, settings.Value);
        var content = new StringContent(json);
        content.Headers.ContentType =
          MediaTypeHeaderValue.Parse("application/json");
        request.Content = content;
        request.Method = new HttpMethod("PATCH");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<Subscriber>(response, headers, cancellationToken)
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
          if (disposeResponse)
            response.Dispose();
        }
      }
    }

    /// <summary>
    /// Get a subscriber
    /// </summary>
    /// <remarks>
    /// Get a subscriber
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="subscriber_id">Subscriber Identifier</param>
    /// <returns>Get a subscriber</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual Task<Subscriber> GetSubscriber(string pageId, string subscriberId) {
      return GetSubscriber(pageId, subscriberId,
        CancellationToken.None);
    }

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get a subscriber
    /// </summary>
    /// <remarks>
    /// Get a subscriber
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="subscriber_id">Subscriber Identifier</param>
    /// <returns>Get a subscriber</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Subscriber> GetSubscriber(string pageId, string subscriberId,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (subscriberId == null)
        throw new ArgumentNullException(nameof(subscriberId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/subscribers/{subscriber_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{subscriber_id}",
        Uri.EscapeDataString(ConvertToString(subscriberId,
          CultureInfo.InvariantCulture)));

      using (var request = new HttpRequestMessage()) {
        request.Method = new HttpMethod("GET");
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<Subscriber>(response, headers, cancellationToken)
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
          if (disposeResponse)
            response.Dispose();
        }
      }
    }


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Create a template
    /// </summary>
    /// <remarks>
    /// Create a template
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Create a template</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<IncidentTemplate> PostIncidentTemplates(string pageId,
      PostPagesPageIdIncidentTemplates body,
      CancellationToken cancellationToken) {
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
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
          if (disposeResponse)
            response.Dispose();
        }
      }
    }


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get a list of templates
    /// </summary>
    /// <remarks>
    /// Get a list of templates
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page">Page offset to fetch.</param>
    /// <param name="per_page">Number of results to return per page.</param>
    /// <returns>Get a list of templates</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<ICollection<IncidentTemplate>> GetIncidentTemplates(string pageId, int? page,
      int? perPage,
      CancellationToken cancellationToken) {
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<ICollection<IncidentTemplate>>(
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Create an incident
    /// </summary>
    /// <remarks>
    /// Create an incident
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Create an incident</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Incident> PostIncidents(string pageId, PostPagesPageIdIncidents body,
      CancellationToken cancellationToken) {
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
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
          if (disposeResponse)
            response.Dispose();
        }
      }
    }


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get a list of incidents
    /// </summary>
    /// <remarks>
    /// Get a list of incidents
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="q">If this is specified, search for the text query string in the incidents' name, status, postmortem_body, and incident_updates fields.</param>
    /// <param name="limit">The maximum number of rows to return per page. The default and maximum limit is 100.</param>
    /// <param name="page">Page offset to fetch.</param>
    /// <returns>Get a list of incidents</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<ICollection<Incident>> GetIncidents(string pageId, string q, int? limit, int? page,
      CancellationToken cancellationToken) {
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<ICollection<Incident>>(
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get a list of active maintenances
    /// </summary>
    /// <remarks>
    /// Get a list of active maintenances
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page">Page offset to fetch.</param>
    /// <param name="per_page">Number of results to return per page.</param>
    /// <returns>Get a list of active maintenances</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<ICollection<Incident>> GetIncidentsActiveMaintenance(string pageId, int? page,
      int? perPage,
      CancellationToken cancellationToken) {
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<ICollection<Incident>>(
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get a list of upcoming incidents
    /// </summary>
    /// <remarks>
    /// Get a list of upcoming incidents
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page">Page offset to fetch.</param>
    /// <param name="per_page">Number of results to return per page.</param>
    /// <returns>Get a list of upcoming incidents</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<ICollection<Incident>> GetIncidentsUpcoming(string pageId, int? page, int? perPage,
      CancellationToken cancellationToken) {
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<ICollection<Incident>>(
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get a list of scheduled incidents
    /// </summary>
    /// <remarks>
    /// Get a list of scheduled incidents
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page">Page offset to fetch.</param>
    /// <param name="per_page">Number of results to return per page.</param>
    /// <returns>Get a list of scheduled incidents</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<ICollection<Incident>> GetIncidentsScheduled(string pageId, int? page, int? perPage,
      CancellationToken cancellationToken) {
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<ICollection<Incident>>(
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get a list of unresolved incidents
    /// </summary>
    /// <remarks>
    /// Get a list of unresolved incidents
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page">Page offset to fetch.</param>
    /// <param name="per_page">Number of results to return per page.</param>
    /// <returns>Get a list of unresolved incidents</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<ICollection<Incident>> GetIncidentsUnresolved(string pageId, int? page, int? perPage,
      CancellationToken cancellationToken) {
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<ICollection<Incident>>(
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Delete an incident
    /// </summary>
    /// <remarks>
    /// Delete an incident
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="incident_id">Incident Identifier</param>
    /// <returns>Delete an incident</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Incident> DeleteIncident(string pageId, string incidentId,
      CancellationToken cancellationToken) {
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
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
          if (disposeResponse)
            response.Dispose();
        }
      }
    }


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Update an incident
    /// </summary>
    /// <remarks>
    /// Update an incident
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="incident_id">Incident Identifier</param>
    /// <returns>Update an incident</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Incident> PatchIncident(string pageId, string incidentId, PatchPagesPageIdIncidents body,
      CancellationToken cancellationToken) {
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Update an incident
    /// </summary>
    /// <remarks>
    /// Update an incident
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="incident_id">Incident Identifier</param>
    /// <returns>Update an incident</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Incident> PutIncident(string pageId, string incidentId, PutPagesPageIdIncidents body,
      CancellationToken cancellationToken) {
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

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

          var status = (int)response.StatusCode;
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get an incident
    /// </summary>
    /// <remarks>
    /// Get an incident
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="incident_id">Incident Identifier</param>
    /// <returns>Get an incident</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Incident> GetIncident(string pageId, string incidentId,
      CancellationToken cancellationToken) {
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content != null && response.Content.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int)response.StatusCode;
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Update a previous incident update
    /// </summary>
    /// <remarks>
    /// Update a previous incident update
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="incident_id">Incident Identifier</param>
    /// <param name="incident_update_id">Incident Update Identifier</param>
    /// <returns>Update a previous incident update</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<IncidentUpdate> PatchIncidentUpdate(string pageId, string incidentId,
      string incidentUpdateId, PatchPagesPageIdIncidentsIncidentIdIncidentUpdates body,
      CancellationToken cancellationToken) {
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

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

          var status = (int)response.StatusCode;
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Update a previous incident update
    /// </summary>
    /// <remarks>
    /// Update a previous incident update
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="incident_id">Incident Identifier</param>
    /// <param name="incident_update_id">Incident Update Identifier</param>
    /// <returns>Update a previous incident update</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<IncidentUpdate> PutIncidentUpdate(string pageId, string incidentId,
      string incidentUpdateId, PutPagesPageIdIncidentsIncidentIdIncidentUpdates body,
      CancellationToken cancellationToken) {
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int)response.StatusCode;
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Create an incident subscriber
    /// </summary>
    /// <remarks>
    /// Create an incident subscriber
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="incident_id">Incident Identifier</param>
    /// <returns>Create an incident subscriber</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Subscriber> PostIncidentSubscribers(string pageId, string incidentId,
      PostPagesPageIdIncidentsIncidentIdSubscribers body,
      CancellationToken cancellationToken) {
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int)response.StatusCode;
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
              var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken).ConfigureAwait(false);
              if (objectResponse.Object == null)
                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
              throw new ApiException<ErrorEntity>("Bad request", status, objectResponse.Text, headers, objectResponse.Object, null);
            }
            case 401: {
              var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken).ConfigureAwait(false);
              if (objectResponse.Object == null)
                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
              throw new ApiException<ErrorEntity>("Could not authenticate", status, objectResponse.Text, headers, objectResponse.Object, null);
            }
            case 404: {
              var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken).ConfigureAwait(false);
              if (objectResponse.Object == null)
                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status, objectResponse.Text, headers, objectResponse.Object, null);
            }
            default: {
              var responseData = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
              throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
            }
          }
        }
        finally {
          response.Dispose();
        }
      }
    }


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get a list of incident subscribers
    /// </summary>
    /// <remarks>
    /// Get a list of incident subscribers
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="incident_id">Incident Identifier</param>
    /// <param name="page">Page offset to fetch. Beginning February 28, 2023, this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <param name="per_page">Number of results to return per page. Beginning February 28, 2023, a default and maximum limit of 100 will be imposed and this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <returns>Get a list of incident subscribers</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<ICollection<Subscriber>> GetIncidentSubscribers(string pageId, string incidentId,
      int? page,
      int? perPage,
      CancellationToken cancellationToken) {
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int)response.StatusCode;
          switch (status) {
            case 200: {
              var objectResponse = await ReadObjectResponseAsync<ICollection<Subscriber>>(response, headers, cancellationToken).ConfigureAwait(false);
              return objectResponse.Object ?? throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
            }
            case 401: {
              var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken).ConfigureAwait(false);
              if (objectResponse.Object == null)
                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
              throw new ApiException<ErrorEntity>("Could not authenticate", status, objectResponse.Text, headers, objectResponse.Object, null);
            }
            case 404: {
              var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken).ConfigureAwait(false);
              if (objectResponse.Object == null)
                throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status, objectResponse.Text, headers, objectResponse.Object, null);
            }
            default: {
              var responseData = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
              throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
            }
          }
        }
        finally {
          response.Dispose();
        }
      }
    }


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Unsubscribe an incident subscriber
    /// </summary>
    /// <remarks>
    /// Unsubscribe an incident subscriber
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="incident_id">Incident Identifier</param>
    /// <param name="subscriber_id">Subscriber Identifier</param>
    /// <returns>Unsubscribe an incident subscriber</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Subscriber> DeleteIncidentSubscriber(string pageId, string incidentId,
      string subscriberId, CancellationToken cancellationToken) {
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse = await ReadObjectResponseAsync<Subscriber>(response, headers, cancellationToken).ConfigureAwait(false);
            return objectResponse.Object ?? throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
          }
          else {
            var responseData = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
          }
        }
        finally {
          response.Dispose();
        }
      }
    }

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get an incident subscriber
    /// </summary>
    /// <remarks>
    /// Get an incident subscriber
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="incident_id">Incident Identifier</param>
    /// <param name="subscriber_id">Subscriber Identifier</param>
    /// <returns>Get an incident subscriber</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Subscriber> GetIncidentSubscriber(string pageId, string incidentId,
      string subscriberId, CancellationToken cancellationToken) {
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse = await ReadObjectResponseAsync<Subscriber>(response, headers, cancellationToken).ConfigureAwait(false);
            return objectResponse.Object ?? throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
          }
          else {
            var responseData = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
          }
        }
        finally {
          response.Dispose();
        }
      }
    }

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Resend confirmation to an incident subscriber
    /// </summary>
    /// <remarks>
    /// Resend confirmation to an incident subscriber
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="incident_id">Incident Identifier</param>
    /// <param name="subscriber_id">Subscriber Identifier</param>
    /// <returns>Resend confirmation to an incident subscriber</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task PostIncidentSubscriberResendConfirmation(string pageId, string incidentId,
      string subscriberId, CancellationToken cancellationToken) {
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

        var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int)response.StatusCode;
          if (status != 201) {
            var responseData = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
          }
        }
        finally {
          response.Dispose();
        }
      }
    }


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get Postmortem
    /// </summary>
    /// <remarks>
    /// Get Postmortem
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="incident_id">Incident Identifier</param>
    /// <returns>Get Postmortem</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Postmortem> GetIncidentPostmortem(string pageId, string incidentId,
      CancellationToken cancellationToken) {
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse = await ReadObjectResponseAsync<Postmortem>(response, headers, cancellationToken).ConfigureAwait(false);
            return objectResponse.Object ?? throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
          }
          else if (status == 401) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken).ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
            throw new ApiException<ErrorEntity>("Could not authenticate", status, objectResponse.Text, headers, objectResponse.Object, null);
          }
          else if (status == 404) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken).ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
            throw new ApiException<ErrorEntity>("The requested resource could not be found.", status, objectResponse.Text, headers, objectResponse.Object, null);
          }
          else {
            var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
          }
        }
        finally {
          response.Dispose();
        }
      }
    }


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Create Postmortem
    /// </summary>
    /// <remarks>
    /// Create Postmortem
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="incident_id">Incident Identifier</param>
    /// <returns>Create Postmortem</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Postmortem> PutIncidentPostmortem(
      string pageId, string incidentId, PutPagesPageIdIncidentsIncidentIdPostmortem body,
      CancellationToken cancellationToken) {
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse = await ReadObjectResponseAsync<Postmortem>(response, headers, cancellationToken).ConfigureAwait(false);
            return objectResponse.Object ?? throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
          }
          else if (status == 400) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken).ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
            throw new ApiException<ErrorEntity>("Bad request", status, objectResponse.Text, headers,
              objectResponse.Object, null);
          }
          else if (status == 401) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken).ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
            throw new ApiException<ErrorEntity>("Could not authenticate", status, objectResponse.Text, headers, objectResponse.Object, null);
          }
          else {
            var responseData = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
          }
        }
        finally {
          response.Dispose();
        }
      }
    }

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Delete Postmortem
    /// </summary>
    /// <remarks>
    /// Delete Postmortem
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="incident_id">Incident Identifier</param>
    /// <returns>Delete Postmortem</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task DeleteIncidentPostmortem(
      string pageId, string incidentId, CancellationToken cancellationToken) {
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

        var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int)response.StatusCode;
          if (status == 204) {
            return;
          }
          else if (status == 401) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken).ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
            throw new ApiException<ErrorEntity>("Could not authenticate", status, objectResponse.Text, headers, objectResponse.Object, null);
          }
          else if (status == 404) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken).ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
            throw new ApiException<ErrorEntity>("The requested resource could not be found.", status, objectResponse.Text, headers, objectResponse.Object, null);
          }
          else {
            var responseData = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
          }
        }
        finally {
          response.Dispose();
        }
      }
    }


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Publish Postmortem
    /// </summary>
    /// <remarks>
    /// Publish Postmortem
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="incident_id">Incident Identifier</param>
    /// <returns>Publish Postmortem</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Postmortem> PutIncidentPostmortemPublish(string pageId, string incidentId,
      PutPagesPageIdIncidentsIncidentIdPostmortemPublish body,
      CancellationToken cancellationToken) {
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse = await ReadObjectResponseAsync<Postmortem>(response, headers, cancellationToken).ConfigureAwait(false);
            return objectResponse.Object ?? throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
          }
          else if (status == 400) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken).ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
            throw new ApiException<ErrorEntity>("Bad request", status, objectResponse.Text, headers, objectResponse.Object, null);
          }
          else if (status == 401) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken).ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
            throw new ApiException<ErrorEntity>("Could not authenticate", status, objectResponse.Text, headers, objectResponse.Object, null);
          }
          else if (status == 404) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken).ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
            throw new ApiException<ErrorEntity>("The requested resource could not be found.", status, objectResponse.Text, headers, objectResponse.Object, null);
          }
          else {
            var responseData = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
          }
        }
        finally {
          response.Dispose();
        }
      }
    }

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Revert Postmortem
    /// </summary>
    /// <remarks>
    /// Revert Postmortem
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="incident_id">Incident Identifier</param>
    /// <returns>Revert Postmortem</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Postmortem> PutIncidentPostmortemRevert(string pageId, string incidentId,
      CancellationToken cancellationToken) {
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse = await ReadObjectResponseAsync<Postmortem>(response, headers, cancellationToken).ConfigureAwait(false);
            return objectResponse.Object ?? throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
          }
          else if (status == 400) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken).ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
            throw new ApiException<ErrorEntity>("Bad request", status, objectResponse.Text, headers, objectResponse.Object, null);
          }
          else if (status == 401) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken).ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
            throw new ApiException<ErrorEntity>("Could not authenticate", status, objectResponse.Text, headers, objectResponse.Object, null);
          }
          else if (status == 404) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken).ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
            throw new ApiException<ErrorEntity>("The requested resource could not be found.", status, objectResponse.Text, headers, objectResponse.Object, null);
          }
          else {
            var responseData = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
          }
        }
        finally {
          response.Dispose();
        }
      }
    }

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Create a component
    /// </summary>
    /// <remarks>
    /// Create a component
    /// </remarks>
    /// <param name="pageId">Page identifier</param>
    /// <returns>Create a component</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Component> PostComponents(string pageId, PostPagesPageIdComponents body,
      CancellationToken cancellationToken) {
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int)response.StatusCode;
          if (status == 201) {
            var objectResponse = await ReadObjectResponseAsync<Component>(response, headers, cancellationToken).ConfigureAwait(false);
            return objectResponse.Object ?? throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
          }
          else if (status == 401) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken).ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
            throw new ApiException<ErrorEntity>("Could not authenticate", status, objectResponse.Text,
              headers, objectResponse.Object, null);
          }
          else if (status == 422) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken).ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
            throw new ApiException<ErrorEntity>("Unprocessable entity", status, objectResponse.Text, headers, objectResponse.Object, null);
          }
          else {
            var responseData = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
          }
        }
        finally {
          response.Dispose();
        }
      }
    }

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get a list of components
    /// </summary>
    /// <remarks>
    /// Get a list of components
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page">Page offset to fetch. Beginning February 28, 2023, this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <param name="per_page">Number of results to return per page. Beginning February 28, 2023, a default and maximum limit of 100 will be imposed and this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <returns>Get a list of components</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<ICollection<Component>> GetPagesComponents(string pageId, int? page, int? perPage,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/pages/{page_id}/components?");
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse = await ReadObjectResponseAsync<ICollection<Component>>(response, headers, cancellationToken).ConfigureAwait(false);
            return objectResponse.Object ?? throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
          }
          else if (status == 401) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken).ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
            throw new ApiException<ErrorEntity>("Could not authenticate", status, objectResponse.Text, headers, objectResponse.Object, null);
          }
          else {
            var responseData = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
          }
        }
        finally {
          response.Dispose();
        }
      }
    }

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Update a component
    /// </summary>
    /// <remarks>
    /// If group_id is "null" then the component will be removed from a group.
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="component_id">Component identifier</param>
    /// <returns>Update a component</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Component> PatchComponent(string pageId, string componentId,
      PatchPagesPageIdComponents body,
      CancellationToken cancellationToken) {
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

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

          var status = (int)response.StatusCode;
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
            var responseData = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Update a component
    /// </summary>
    /// <remarks>
    /// If group_id is "null" then the component will be removed from a group.
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="component_id">Component identifier</param>
    /// <returns>Update a component</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Component> PutComponent(
      string pageId, string componentId, PutPagesPageIdComponents body,
      CancellationToken cancellationToken) {
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse = await ReadObjectResponseAsync<Component>(response, headers, cancellationToken).ConfigureAwait(false);
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
            var responseData = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
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

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Delete a component
    /// </summary>
    /// <remarks>
    /// Delete a component
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="component_id">Component identifier</param>
    /// <returns>Delete a component</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task DeleteComponent(string pageId,
      string componentId, CancellationToken cancellationToken) {
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

          var status = (int)response.StatusCode;
          if (status == 204) {
            return;
          }
          else if (status == 401) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken).ConfigureAwait(false);
            if (objectResponse.Object == null) {
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
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
            var responseData = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
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

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get a component
    /// </summary>
    /// <remarks>
    /// Get a component
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="component_id">Component identifier</param>
    /// <returns>Get a component</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Component> GetComponent(
      string pageId, string componentId, CancellationToken cancellationToken) {
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int)response.StatusCode;
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
            var responseData = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
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

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get uptime data for a component
    /// </summary>
    /// <remarks>
    /// Get uptime data for a component that has uptime showcase enabled
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="component_id">Component identifier</param>
    /// <param name="skip_related_events">Skips supplying the related events data along with the component uptime data.</param>
    /// <param name="start">The start date for uptime calculation (defaults to the component's start_date field or 90 days ago, whichever is more recent).
    /// <br/>The maximum supported date range is six calendar months. If the year is given, the date defaults to the first day of the year.
    /// <br/>If the year and month are given, the start date defaults to the first day of that month.
    /// <br/>The earliest supported date is January 1, 1970.</param>
    /// <param name="end">The end date for uptime calculation (defaults to today in the page's time zone). The maximum supported date range is six calendar months.
    /// <br/>If the year is given, the date defaults to the last day of the year. If the year and month are given, the date defaults to the last day of that month.
    /// <br/>The earliest supported date is January 1, 1970.</param>
    /// <returns>Get uptime data for a component that has uptime showcase enabled</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<ComponentUptime> GetComponentUptime(string pageId, string componentId,
      bool? skipRelatedEvents,
      object start, object end, CancellationToken cancellationToken) {
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

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

          var status = (int)response.StatusCode;
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
            var responseData = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
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

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Remove page access users from component
    /// </summary>
    /// <remarks>
    /// Remove page access users from component
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="component_id">Component identifier</param>
    /// <returns>Remove page access users from component</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Component> DeleteComponentPageAccessUsers(string pageId, string componentId,
      CancellationToken cancellationToken) {
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse = await ReadObjectResponseAsync<Component>(response, headers, cancellationToken).ConfigureAwait(false);
            return objectResponse.Object ?? throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
          }
          else if (status == 401) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken).ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
            throw new ApiException<ErrorEntity>("Could not authenticate", status, objectResponse.Text, headers, objectResponse.Object, null);
          }
          else if (status == 404) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken).ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
            throw new ApiException<ErrorEntity>("The requested resource could not be found.", status, objectResponse.Text, headers, objectResponse.Object, null);
          }
          else {
            var responseData = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
          }
        }
        finally {
          response.Dispose();
        }
      }
    }

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Add page access users to a component
    /// </summary>
    /// <remarks>
    /// Add page access users to a component
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="component_id">Component identifier</param>
    /// <returns>Add page access users to a component</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Component> PostComponentPageAccessUsers(string pageId, string componentId, Body body,
      CancellationToken cancellationToken) {
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int)response.StatusCode;
          if (status == 201) {
            var objectResponse = await ReadObjectResponseAsync<Component>(response, headers, cancellationToken).ConfigureAwait(false);
            return objectResponse.Object ?? throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
          }
          else if (status == 401) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken).ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
            throw new ApiException<ErrorEntity>("Could not authenticate", status, objectResponse.Text, headers, objectResponse.Object, null);
          }
          else if (status == 404) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken).ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
            throw new ApiException<ErrorEntity>("The requested resource could not be found.", status, objectResponse.Text, headers, objectResponse.Object, null);
          }
          else if (status == 422) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken).ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
            throw new ApiException<ErrorEntity>("Unprocessable entity", status, objectResponse.Text, headers, objectResponse.Object, null);
          }
          else {
            var responseData = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
          }
        }
        finally {
          response.Dispose();
        }
      }
    }


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Remove page access groups from a component
    /// </summary>
    /// <remarks>
    /// Remove page access groups from a component
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="component_id">Component identifier</param>
    /// <returns>Remove page access groups from a component</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Component> DeleteComponentPageAccessGroups(string pageId, string componentId,
      CancellationToken cancellationToken) {
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
        request.Headers.Accept.Add(
          MediaTypeWithQualityHeaderValue.Parse("application/json"));

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        try {
          var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int)response.StatusCode;
          if (status == 200) {
            var objectResponse = await ReadObjectResponseAsync<Component>(response, headers, cancellationToken).ConfigureAwait(false);
            return objectResponse.Object ?? throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
          }
          else if (status == 401) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken).ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
            throw new ApiException<ErrorEntity>("Could not authenticate", status, objectResponse.Text, headers, objectResponse.Object, null);
          }
          else if (status == 404) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken).ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
            throw new ApiException<ErrorEntity>("The requested resource could not be found.", status, objectResponse.Text, headers, objectResponse.Object, null);
          }
          else {
            var responseData = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
          }
        }
        finally {
          response.Dispose();
        }
      }
    }

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Add page access groups to a component
    /// </summary>
    /// <remarks>
    /// Add page access groups to a component
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="component_id">Component identifier</param>
    /// <returns>Add page access groups to a component</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Component> PostComponentPageAccessGroups(string pageId, string componentId,
      CancellationToken cancellationToken) {
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

      var disposeClient = false;
      try {
        using (var request = new HttpRequestMessage()) {
          request.Content =
            new StringContent(string.Empty, Encoding.UTF8, "application/json");
          request.Method = new HttpMethod("POST");
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
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

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Create a component group
    /// </summary>
    /// <remarks>
    /// Create a component group
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Create a component group</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<GroupComponent> PostComponentGroups(
      string pageId, PostPagesPageIdComponentGroups body, CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/pages/{page_id}/component-groups");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));

      var disposeClient = false;
      try {
        using (var request = new HttpRequestMessage()) {
          var json = JsonConvert.SerializeObject(body, settings.Value);
          var content = new StringContent(json);
          content.Headers.ContentType =
            MediaTypeHeaderValue.Parse("application/json");
          request.Content = content;
          request.Method = new HttpMethod("POST");
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
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

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get a list of component groups
    /// </summary>
    /// <remarks>
    /// Get a list of component groups
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page">Page offset to fetch. Beginning February 28, 2023, this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <param name="per_page">Number of results to return per page. Beginning February 28, 2023, a default and maximum limit of 100 will be imposed and this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <returns>Get a list of component groups</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<ICollection<GroupComponent>> GetComponentGroups(string pageId, int? page, int? perPage,
      CancellationToken cancellationToken) {
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
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
            if (status == 200) {
              var objectResponse =
                await ReadObjectResponseAsync<ICollection<GroupComponent>>(
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

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Update a component group
    /// </summary>
    /// <remarks>
    /// Update a component group
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="id">Component group identifier</param>
    /// <returns>Update a component group</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<GroupComponent> PatchComponentGroup(string pageId, string id,
      PatchPagesPageIdComponentGroups body,
      CancellationToken cancellationToken) {
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

      var disposeClient = false;
      try {
        using (var request = new HttpRequestMessage()) {
          var json = JsonConvert.SerializeObject(body, settings.Value);
          var content = new StringContent(json);
          content.Headers.ContentType =
            MediaTypeHeaderValue.Parse("application/json");
          request.Content = content;
          request.Method = new HttpMethod("PATCH");
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
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

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Update a component group
    /// </summary>
    /// <remarks>
    /// Update a component group
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="id">Component group identifier</param>
    /// <returns>Update a component group</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<GroupComponent> PutComponentGroup(
      string pageId, string id, PutPagesPageIdComponentGroups body,
      CancellationToken cancellationToken) {
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

      var disposeClient = false;
      try {
        using (var request = new HttpRequestMessage()) {
          var json = JsonConvert.SerializeObject(body, settings.Value);
          var content = new StringContent(json);
          content.Headers.ContentType =
            MediaTypeHeaderValue.Parse("application/json");
          request.Content = content;
          request.Method = new HttpMethod("PUT");
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Delete a component group
    /// </summary>
    /// <remarks>
    /// Delete a component group
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="id">Component group identifier</param>
    /// <returns>Delete a component group</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<GroupComponent> DeleteComponentGroup(
      string pageId, string id, CancellationToken cancellationToken) {
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

      var disposeClient = false;
      try {
        using (var request = new HttpRequestMessage()) {
          request.Method = new HttpMethod("DELETE");
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get a component group
    /// </summary>
    /// <remarks>
    /// Get a component group
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="id">Component group identifier</param>
    /// <returns>Get a component group</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<GroupComponent> GetComponentGroup(
      string pageId, string id, CancellationToken cancellationToken) {
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

      var disposeClient = false;
      try {
        using (var request = new HttpRequestMessage()) {
          request.Method = new HttpMethod("GET");
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get uptime data for a component group
    /// </summary>
    /// <remarks>
    /// Get uptime data for a component group that has uptime showcase enabled for at least one component.
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="id">Component group identifier</param>
    /// <param name="skip_related_events">Skips supplying the related events data along with the component uptime data.</param>
    /// <param name="start">The start date for uptime calculation (defaults to the date of the component in the group with the earliest start_date, or 90 days ago, whichever is more recent).
    /// <br/>The maximum supported date range is six calendar months. If the year is given, the date defaults to the first day of the year.
    /// <br/>If the year and month are given, the start date defaults to the first day of that month.
    /// <br/>The earliest supported date is January 1, 1970.</param>
    /// <param name="end">The end date for uptime calculation (defaults to today in the page's time zone). The maximum supported date range is six calendar months.
    /// <br/>If the year is given, the date defaults to the last day of the year. If the year and month are given, the date defaults to the last day of that month.
    /// <br/>The earliest supported date is January 1, 1970.</param>
    /// <returns>Get uptime data for a component group that has uptime showcase enabled for at least one component.</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<ComponentGroupUptime> GetComponentGroupUptime(string pageId, string id,
      bool? skipRelatedEvents,
      object start,
      object end, CancellationToken cancellationToken) {
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
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Add data points to metrics
    /// </summary>
    /// <remarks>
    /// Add data points to metrics
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Data Point is submitted and is currently being added to the metrics</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<MetricAddResponse> PostMetricsData(
      string pageId, PostPagesPageIdMetricsData body, CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/pages/{page_id}/metrics/data");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));

      var disposeClient = false;
      try {
        using (var request = new HttpRequestMessage()) {
          var json = JsonConvert.SerializeObject(body, settings.Value);
          var content = new StringContent(json);
          content.Headers.ContentType =
            MediaTypeHeaderValue.Parse("application/json");
          request.Content = content;
          request.Method = new HttpMethod("POST");
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
            if (status == 202) {
              var objectResponse =
                await ReadObjectResponseAsync<MetricAddResponse>(response, headers, cancellationToken)
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
            else if (status == 403) {
              var objectResponse =
                await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                  .ConfigureAwait(false);
              if (objectResponse.Object == null) {
                throw new ApiException("Response was null which was not expected.", status,
                  objectResponse.Text, headers, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                status, objectResponse.Text, headers, objectResponse.Object, null);
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
            else if (status == 405) {
              var objectResponse =
                await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                  .ConfigureAwait(false);
              if (objectResponse.Object == null) {
                throw new ApiException("Response was null which was not expected.", status,
                  objectResponse.Text, headers, null);
              }

              throw new ApiException<ErrorEntity>("Method not allowed.", status, objectResponse.Text,
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get a list of metrics
    /// </summary>
    /// <remarks>
    /// Get a list of metrics
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page">Page offset to fetch. Beginning February 28, 2023, this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <param name="per_page">Number of results to return per page. Beginning February 28, 2023, a default and maximum limit of 100 will be imposed and this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <returns>Get a list of metrics</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Metric> GetMetrics(string pageId, int? page,
      int? perPage, CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/pages/{page_id}/metrics?");
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
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
            if (status == 200) {
              var objectResponse =
                await ReadObjectResponseAsync<Metric>(response, headers, cancellationToken)
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Update a metric
    /// </summary>
    /// <remarks>
    /// Update a metric
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="metric_id">Metric Identifier</param>
    /// <returns>Update a metric</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Metric> PatchMetric(string pageId,
      string metricId, PatchPagesPageIdMetrics body, CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (metricId == null)
        throw new ArgumentNullException(nameof(metricId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/metrics/{metric_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{metric_id}",
        Uri.EscapeDataString(ConvertToString(metricId,
          CultureInfo.InvariantCulture)));

      var disposeClient = false;
      try {
        using (var request = new HttpRequestMessage()) {
          var json = JsonConvert.SerializeObject(body, settings.Value);
          var content = new StringContent(json);
          content.Headers.ContentType =
            MediaTypeHeaderValue.Parse("application/json");
          request.Content = content;
          request.Method = new HttpMethod("PATCH");
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
            if (status == 200) {
              var objectResponse =
                await ReadObjectResponseAsync<Metric>(response, headers, cancellationToken)
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Update a metric
    /// </summary>
    /// <remarks>
    /// Update a metric
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="metric_id">Metric Identifier</param>
    /// <returns>Update a metric</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Metric> PutMetric(string pageId,
      string metricId, PutPagesPageIdMetrics body, CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (metricId == null)
        throw new ArgumentNullException(nameof(metricId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/metrics/{metric_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{metric_id}",
        Uri.EscapeDataString(ConvertToString(metricId,
          CultureInfo.InvariantCulture)));

      var disposeClient = false;
      try {
        using (var request = new HttpRequestMessage()) {
          var json = JsonConvert.SerializeObject(body, settings.Value);
          var content = new StringContent(json);
          content.Headers.ContentType =
            MediaTypeHeaderValue.Parse("application/json");
          request.Content = content;
          request.Method = new HttpMethod("PUT");
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
            if (status == 200) {
              var objectResponse =
                await ReadObjectResponseAsync<Metric>(response, headers, cancellationToken)
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Delete a metric
    /// </summary>
    /// <remarks>
    /// Delete a metric
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="metric_id">Metric Identifier</param>
    /// <returns>Delete a metric</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Metric> DeleteMetric(string pageId,
      string metricId, CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (metricId == null)
        throw new ArgumentNullException(nameof(metricId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/metrics/{metric_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{metric_id}",
        Uri.EscapeDataString(ConvertToString(metricId,
          CultureInfo.InvariantCulture)));

      var disposeClient = false;
      try {
        using (var request = new HttpRequestMessage()) {
          request.Method = new HttpMethod("DELETE");
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
            if (status == 200) {
              var objectResponse =
                await ReadObjectResponseAsync<Metric>(response, headers, cancellationToken)
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get a metric
    /// </summary>
    /// <remarks>
    /// Get a metric
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="metric_id">Metric Identifier</param>
    /// <returns>Get a metric</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Metric> GetMetric(string pageId,
      string metricId, CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (metricId == null)
        throw new ArgumentNullException(nameof(metricId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/metrics/{metric_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{metric_id}",
        Uri.EscapeDataString(ConvertToString(metricId,
          CultureInfo.InvariantCulture)));

      var disposeClient = false;
      try {
        using (var request = new HttpRequestMessage()) {
          request.Method = new HttpMethod("GET");
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
            if (status == 200) {
              var objectResponse =
                await ReadObjectResponseAsync<Metric>(response, headers, cancellationToken)
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Reset data for a metric
    /// </summary>
    /// <remarks>
    /// Reset data for a metric
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="metric_id">Metric Identifier</param>
    /// <returns>Reset data for a metric</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Metric> DeletePagesPageIdMetricsMetricIdDataAsync(
      string pageId, string metricId, CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (metricId == null)
        throw new ArgumentNullException(nameof(metricId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/metrics/{metric_id}/data");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{metric_id}",
        Uri.EscapeDataString(ConvertToString(metricId,
          CultureInfo.InvariantCulture)));

      var disposeClient = false;
      try {
        using (var request = new HttpRequestMessage()) {
          request.Method = new HttpMethod("DELETE");
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
            if (status == 200) {
              var objectResponse =
                await ReadObjectResponseAsync<Metric>(response, headers, cancellationToken)
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Add data to a metric
    /// </summary>
    /// <remarks>
    /// Add data to a metric
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="metric_id">Metric Identifier</param>
    /// <returns>Add data to a metric</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<SingleMetricAddResponse> PostMetricData(string pageId, string metricId,
      PostPagesPageIdMetricsMetricIdData body, CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (metricId == null)
        throw new ArgumentNullException(nameof(metricId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/metrics/{metric_id}/data");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{metric_id}",
        Uri.EscapeDataString(ConvertToString(metricId,
          CultureInfo.InvariantCulture)));

      var disposeClient = false;
      try {
        using (var request = new HttpRequestMessage()) {
          var json = JsonConvert.SerializeObject(body, settings.Value);
          var content = new StringContent(json);
          content.Headers.ContentType =
            MediaTypeHeaderValue.Parse("application/json");
          request.Content = content;
          request.Method = new HttpMethod("POST");
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
            if (status == 201) {
              var objectResponse =
                await ReadObjectResponseAsync<SingleMetricAddResponse>(response, headers,
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
            else if (status == 405) {
              var objectResponse =
                await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                  .ConfigureAwait(false);
              if (objectResponse.Object == null) {
                throw new ApiException("Response was null which was not expected.", status,
                  objectResponse.Text, headers, null);
              }

              throw new ApiException<ErrorEntity>("Method not allowed.", status, objectResponse.Text,
                headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get a list of metric providers
    /// </summary>
    /// <remarks>
    /// Get a list of metric providers
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Get a list of metric providers</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<ICollection<MetricsProvider>> GetMetricsProviders(string pageId,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/metrics_providers");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));

      var disposeClient = false;
      try {
        using (var request = new HttpRequestMessage()) {
          request.Method = new HttpMethod("GET");
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
            if (status == 200) {
              var objectResponse =
                await ReadObjectResponseAsync<ICollection<MetricsProvider>>(
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
            else if (status == 403) {
              var objectResponse =
                await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                  .ConfigureAwait(false);
              if (objectResponse.Object == null) {
                throw new ApiException("Response was null which was not expected.", status,
                  objectResponse.Text, headers, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Create a metric provider
    /// </summary>
    /// <remarks>
    /// Create a metric provider
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Create a metric provider</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<MetricsProvider> PostMetricsProviders(string pageId, PostPagesPageIdMetricsProviders body,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/metrics_providers");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));

      var disposeClient = false;
      try {
        using (var request = new HttpRequestMessage()) {
          var json = JsonConvert.SerializeObject(body, settings.Value);
          var content = new StringContent(json);
          content.Headers.ContentType =
            MediaTypeHeaderValue.Parse("application/json");
          request.Content = content;
          request.Method = new HttpMethod("POST");
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
            if (status == 201) {
              var objectResponse =
                await ReadObjectResponseAsync<MetricsProvider>(response, headers, cancellationToken)
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
            else if (status == 403) {
              var objectResponse =
                await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                  .ConfigureAwait(false);
              if (objectResponse.Object == null) {
                throw new ApiException("Response was null which was not expected.", status,
                  objectResponse.Text, headers, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get a metric provider
    /// </summary>
    /// <remarks>
    /// Get a metric provider
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="metrics_provider_id">Metric Provider Identifier</param>
    /// <returns>Get a metric provider</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<MetricsProvider> GetMetricsProvider(string pageId, string metricsProviderId,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (metricsProviderId == null)
        throw new ArgumentNullException(nameof(metricsProviderId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/metrics_providers/{metrics_provider_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{metrics_provider_id}",
        Uri.EscapeDataString(ConvertToString(metricsProviderId,
          CultureInfo.InvariantCulture)));

      var disposeClient = false;
      try {
        using (var request = new HttpRequestMessage()) {
          request.Method = new HttpMethod("GET");
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
            if (status == 200) {
              var objectResponse =
                await ReadObjectResponseAsync<MetricsProvider>(response, headers, cancellationToken)
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
            else if (status == 403) {
              var objectResponse =
                await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                  .ConfigureAwait(false);
              if (objectResponse.Object == null) {
                throw new ApiException("Response was null which was not expected.", status,
                  objectResponse.Text, headers, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Update a metric provider
    /// </summary>
    /// <remarks>
    /// Update a metric provider
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="metrics_provider_id">Metric Provider Identifier</param>
    /// <returns>Update a metric provider</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<MetricsProvider> PatchMetricsProvider(string pageId, string metricsProviderId,
      PatchPagesPageIdMetricsProviders body, CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (metricsProviderId == null)
        throw new ArgumentNullException(nameof(metricsProviderId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/metrics_providers/{metrics_provider_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{metrics_provider_id}",
        Uri.EscapeDataString(ConvertToString(metricsProviderId,
          CultureInfo.InvariantCulture)));

      var disposeClient = false;
      try {
        using (var request = new HttpRequestMessage()) {
          var json = JsonConvert.SerializeObject(body, settings.Value);
          var content = new StringContent(json);
          content.Headers.ContentType =
            MediaTypeHeaderValue.Parse("application/json");
          request.Content = content;
          request.Method = new HttpMethod("PATCH");
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
            if (status == 200) {
              var objectResponse =
                await ReadObjectResponseAsync<MetricsProvider>(response, headers, cancellationToken)
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
            else if (status == 403) {
              var objectResponse =
                await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                  .ConfigureAwait(false);
              if (objectResponse.Object == null) {
                throw new ApiException("Response was null which was not expected.", status,
                  objectResponse.Text, headers, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Update a metric provider
    /// </summary>
    /// <remarks>
    /// Update a metric provider
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="metrics_provider_id">Metric Provider Identifier</param>
    /// <returns>Update a metric provider</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<MetricsProvider> PutMetricsProvider(string pageId, string metricsProviderId,
      PutPagesPageIdMetricsProviders body, CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (metricsProviderId == null)
        throw new ArgumentNullException(nameof(metricsProviderId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/metrics_providers/{metrics_provider_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{metrics_provider_id}",
        Uri.EscapeDataString(ConvertToString(metricsProviderId,
          CultureInfo.InvariantCulture)));

      var disposeClient = false;
      try {
        using (var request = new HttpRequestMessage()) {
          var json = JsonConvert.SerializeObject(body, settings.Value);
          var content = new StringContent(json);
          content.Headers.ContentType =
            MediaTypeHeaderValue.Parse("application/json");
          request.Content = content;
          request.Method = new HttpMethod("PUT");
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
            if (status == 200) {
              var objectResponse =
                await ReadObjectResponseAsync<MetricsProvider>(response, headers, cancellationToken)
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
            else if (status == 403) {
              var objectResponse =
                await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                  .ConfigureAwait(false);
              if (objectResponse.Object == null) {
                throw new ApiException("Response was null which was not expected.", status,
                  objectResponse.Text, headers, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Delete a metric provider
    /// </summary>
    /// <remarks>
    /// Delete a metric provider
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="metrics_provider_id">Metric Provider Identifier</param>
    /// <returns>Delete a metric provider</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<MetricsProvider> DeleteMetricsProvider(string pageId, string metricsProviderId,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (metricsProviderId == null)
        throw new ArgumentNullException(nameof(metricsProviderId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/metrics_providers/{metrics_provider_id}");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{metrics_provider_id}",
        Uri.EscapeDataString(ConvertToString(metricsProviderId,
          CultureInfo.InvariantCulture)));

      var disposeClient = false;
      try {
        using (var request = new HttpRequestMessage()) {
          request.Method = new HttpMethod("DELETE");
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
            if (status == 200) {
              var objectResponse =
                await ReadObjectResponseAsync<MetricsProvider>(response, headers, cancellationToken)
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
            else if (status == 403) {
              var objectResponse =
                await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                  .ConfigureAwait(false);
              if (objectResponse.Object == null) {
                throw new ApiException("Response was null which was not expected.", status,
                  objectResponse.Text, headers, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// List metrics for a metric provider
    /// </summary>
    /// <remarks>
    /// List metrics for a metric provider
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="metrics_provider_id">Metric Provider Identifier</param>
    /// <param name="page">Page offset to fetch. Beginning February 28, 2023, this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <param name="per_page">Number of results to return per page. Beginning February 28, 2023, a default and maximum limit of 100 will be imposed and this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <returns>List metrics for a metric provider</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Metric> GetMetricsProviderMetrics(string pageId, string metricsProviderId,
      int? page, int? perPage, CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (metricsProviderId == null)
        throw new ArgumentNullException(nameof(metricsProviderId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/metrics_providers/{metrics_provider_id}/metrics?");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{metrics_provider_id}",
        Uri.EscapeDataString(ConvertToString(metricsProviderId,
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
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
            if (status == 200) {
              var objectResponse =
                await ReadObjectResponseAsync<Metric>(response, headers, cancellationToken)
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
            else if (status == 403) {
              var objectResponse =
                await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                  .ConfigureAwait(false);
              if (objectResponse.Object == null) {
                throw new ApiException("Response was null which was not expected.", status,
                  objectResponse.Text, headers, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Create a metric for a metric provider
    /// </summary>
    /// <remarks>
    /// Create a metric for a metric provider
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="metrics_provider_id">Metric Provider Identifier</param>
    /// <returns>Create a metric for a metric provider</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Metric> PostMetricsProviderMetrics(string pageId, string metricsProviderId,
      PostPagesPageIdMetricsProvidersMetricsProviderIdMetrics body,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (metricsProviderId == null)
        throw new ArgumentNullException(nameof(metricsProviderId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/metrics_providers/{metrics_provider_id}/metrics");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{metrics_provider_id}",
        Uri.EscapeDataString(ConvertToString(metricsProviderId,
          CultureInfo.InvariantCulture)));

      var disposeClient = false;
      try {
        using (var request = new HttpRequestMessage()) {
          var json = JsonConvert.SerializeObject(body, settings.Value);
          var content = new StringContent(json);
          content.Headers.ContentType =
            MediaTypeHeaderValue.Parse("application/json");
          request.Content = content;
          request.Method = new HttpMethod("POST");
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
            if (status == 201) {
              var objectResponse =
                await ReadObjectResponseAsync<Metric>(response, headers, cancellationToken)
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
            else if (status == 403) {
              var objectResponse =
                await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                  .ConfigureAwait(false);
              if (objectResponse.Object == null) {
                throw new ApiException("Response was null which was not expected.", status,
                  objectResponse.Text, headers, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get status embed config settings
    /// </summary>
    /// <remarks>
    /// Get status embed config settings
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Get status embed config settings</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<StatusEmbedConfig> GetStatusEmbedConfig(string pageId,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/status_embed_config");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));

      var disposeClient = false;
      try {
        using (var request = new HttpRequestMessage()) {
          request.Method = new HttpMethod("GET");
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
            if (status == 200) {
              var objectResponse =
                await ReadObjectResponseAsync<StatusEmbedConfig>(response, headers, cancellationToken)
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
            else if (status == 403) {
              var objectResponse =
                await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                  .ConfigureAwait(false);
              if (objectResponse.Object == null) {
                throw new ApiException("Response was null which was not expected.", status,
                  objectResponse.Text, headers, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Update status embed config settings
    /// </summary>
    /// <remarks>
    /// Update status embed config settings
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Update status embed config settings</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<StatusEmbedConfig> PatchStatusEmbedConfig(
      string pageId, PatchPagesPageIdStatusEmbedConfig body,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/status_embed_config");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));

      var disposeClient = false;
      try {
        using (var request = new HttpRequestMessage()) {
          var json = JsonConvert.SerializeObject(body, settings.Value);
          var content = new StringContent(json);
          content.Headers.ContentType =
            MediaTypeHeaderValue.Parse("application/json");
          request.Content = content;
          request.Method = new HttpMethod("PATCH");
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
            if (status == 200) {
              var objectResponse =
                await ReadObjectResponseAsync<StatusEmbedConfig>(response, headers, cancellationToken)
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
            else if (status == 403) {
              var objectResponse =
                await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                  .ConfigureAwait(false);
              if (objectResponse.Object == null) {
                throw new ApiException("Response was null which was not expected.", status,
                  objectResponse.Text, headers, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Update status embed config settings
    /// </summary>
    /// <remarks>
    /// Update status embed config settings
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Update status embed config settings</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<StatusEmbedConfig> PutStatusEmbedConfig(
      string pageId, PutPagesPageIdStatusEmbedConfig body,
      CancellationToken cancellationToken) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/status_embed_config");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
          CultureInfo.InvariantCulture)));

      var disposeClient = false;
      try {
        using (var request = new HttpRequestMessage()) {
          var json = JsonConvert.SerializeObject(body, settings.Value);
          var content = new StringContent(json);
          content.Headers.ContentType =
            MediaTypeHeaderValue.Parse("application/json");
          request.Content = content;
          request.Method = new HttpMethod("PUT");
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
            if (status == 200) {
              var objectResponse =
                await ReadObjectResponseAsync<StatusEmbedConfig>(response, headers, cancellationToken)
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
            else if (status == 403) {
              var objectResponse =
                await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                  .ConfigureAwait(false);
              if (objectResponse.Object == null) {
                throw new ApiException("Response was null which was not expected.", status,
                  objectResponse.Text, headers, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Update a user's role permissions
    /// </summary>
    /// <remarks>
    /// Update a user's role permissions. Payload should contain a mapping of pages to a set of the desired roles,
    /// <br/>                  if the page has Role Based Access Control. Otherwise, the pages should map to an empty hash.
    /// <br/>                  User will lose access to any pages omitted from the payload.
    /// </remarks>
    /// <param name="organization_id">Organization Identifier</param>
    /// <param name="user_id">User identifier</param>
    /// <returns>Update a user's role permissions. Payload should contain a mapping of pages to a set of the desired roles,
    /// <br/>                  if the page has Role Based Access Control. Otherwise, the pages should map to an empty hash.
    /// <br/>                  User will lose access to any pages omitted from the payload.</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Permissions> PutOrganizationPermissionsUser(string organizationId, string userId,
      PutOrganizationsOrganizationIdPermissions body, CancellationToken cancellationToken) {
      if (organizationId == null)
        throw new ArgumentNullException(nameof(organizationId));

      if (userId == null)
        throw new ArgumentNullException(nameof(userId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/organizations/{organization_id}/permissions/{user_id}");
      urlBuilder.Replace("{organization_id}",
        Uri.EscapeDataString(ConvertToString(organizationId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{user_id}",
        Uri.EscapeDataString(ConvertToString(userId,
          CultureInfo.InvariantCulture)));

      var disposeClient = false;
      try {
        using (var request = new HttpRequestMessage()) {
          var json = JsonConvert.SerializeObject(body, settings.Value);
          var content = new StringContent(json);
          content.Headers.ContentType =
            MediaTypeHeaderValue.Parse("application/json");
          request.Content = content;
          request.Method = new HttpMethod("PUT");
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
            if (status == 200) {
              var objectResponse =
                await ReadObjectResponseAsync<Permissions>(response, headers, cancellationToken)
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
            else if (status == 403) {
              var objectResponse =
                await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                  .ConfigureAwait(false);
              if (objectResponse.Object == null) {
                throw new ApiException("Response was null which was not expected.", status,
                  objectResponse.Text, headers, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get a user's permissions
    /// </summary>
    /// <remarks>
    /// Get a user's permissions
    /// </remarks>
    /// <param name="organization_id">Organization Identifier</param>
    /// <param name="user_id">User identifier</param>
    /// <returns>Get a user's permissions</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Permissions> GetOrganizationPermissionsUser(string organizationId, string userId,
      CancellationToken cancellationToken) {
      if (organizationId == null)
        throw new ArgumentNullException(nameof(organizationId));

      if (userId == null)
        throw new ArgumentNullException(nameof(userId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/organizations/{organization_id}/permissions/{user_id}");
      urlBuilder.Replace("{organization_id}",
        Uri.EscapeDataString(ConvertToString(organizationId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{user_id}",
        Uri.EscapeDataString(ConvertToString(userId,
          CultureInfo.InvariantCulture)));

      var disposeClient = false;
      try {
        using (var request = new HttpRequestMessage()) {
          request.Method = new HttpMethod("GET");
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
            if (status == 200) {
              var objectResponse =
                await ReadObjectResponseAsync<Permissions>(response, headers, cancellationToken)
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Delete a user
    /// </summary>
    /// <remarks>
    /// Delete a user
    /// </remarks>
    /// <param name="organization_id">Organization Identifier</param>
    /// <param name="user_id">User Identifier</param>
    /// <returns>Delete a user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<User> DeleteOrganizationUser(
      string organizationId, string userId, CancellationToken cancellationToken) {
      if (organizationId == null)
        throw new ArgumentNullException(nameof(organizationId));

      if (userId == null)
        throw new ArgumentNullException(nameof(userId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/organizations/{organization_id}/users/{user_id}");
      urlBuilder.Replace("{organization_id}",
        Uri.EscapeDataString(ConvertToString(organizationId,
          CultureInfo.InvariantCulture)));
      urlBuilder.Replace("{user_id}",
        Uri.EscapeDataString(ConvertToString(userId,
          CultureInfo.InvariantCulture)));

      var disposeClient = false;
      try {
        using (var request = new HttpRequestMessage()) {
          request.Method = new HttpMethod("DELETE");
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
            if (status == 200) {
              var objectResponse =
                await ReadObjectResponseAsync<User>(response, headers, cancellationToken)
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
            else if (status == 403) {
              var objectResponse =
                await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
                  .ConfigureAwait(false);
              if (objectResponse.Object == null) {
                throw new ApiException("Response was null which was not expected.", status,
                  objectResponse.Text, headers, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                status, objectResponse.Text, headers, objectResponse.Object, null);
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Create a user
    /// </summary>
    /// <remarks>
    /// Create a user
    /// </remarks>
    /// <param name="organization_id">Organization Identifier</param>
    /// <returns>Create a user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<User> PostOrganizationUsers(
      string organizationId, PostOrganizationsOrganizationIdUsers body,
      CancellationToken cancellationToken) {
      if (organizationId == null)
        throw new ArgumentNullException(nameof(organizationId));

      if (body == null)
        throw new ArgumentNullException(nameof(body));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/organizations/{organization_id}/users");
      urlBuilder.Replace("{organization_id}",
        Uri.EscapeDataString(ConvertToString(organizationId,
          CultureInfo.InvariantCulture)));

      var disposeClient = false;
      try {
        using (var request = new HttpRequestMessage()) {
          var json = JsonConvert.SerializeObject(body, settings.Value);
          var content = new StringContent(json);
          content.Headers.ContentType =
            MediaTypeHeaderValue.Parse("application/json");
          request.Content = content;
          request.Method = new HttpMethod("POST");
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
            if (status == 201) {
              var objectResponse =
                await ReadObjectResponseAsync<User>(response, headers, cancellationToken)
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


    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Get a list of users
    /// </summary>
    /// <remarks>
    /// Get a list of users
    /// </remarks>
    /// <param name="organization_id">Organization Identifier</param>
    /// <param name="page">Page offset to fetch. Beginning February 28, 2023, this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <param name="per_page">Number of results to return per page. Beginning February 28, 2023, a default and maximum limit of 100 will be imposed and this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <returns>Get a list of users</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<ICollection<User>> GetOrganizationUsers(string organizationId, int? page, int? perPage,
      CancellationToken cancellationToken) {
      if (organizationId == null)
        throw new ArgumentNullException(nameof(organizationId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/organizations/{organization_id}/users?");
      urlBuilder.Replace("{organization_id}",
        Uri.EscapeDataString(ConvertToString(organizationId,
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
          request.Headers.Accept.Add(
            MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url = urlBuilder.ToString();
          request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

          var response = await httpClient.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
          var disposeResponse = true;
          try {
            var headers =
              response.Headers.ToDictionary(h => h.Key, h => h.Value);
            if (response.Content != null && response.Content.Headers != null) {
              foreach (var item in response.Content.Headers)
                headers[item.Key] = item.Value;
            }

            var status = (int)response.StatusCode;
            if (status == 200) {
              var objectResponse =
                await ReadObjectResponseAsync<ICollection<User>>(response,
                  headers, cancellationToken).ConfigureAwait(false);
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

    protected virtual async Task<ObjectResponseResult<T>> ReadObjectResponseAsync<T>(
      HttpResponseMessage response,
      IReadOnlyDictionary<string, IEnumerable<string>>
        headers, CancellationToken cancellationToken) {
      if (response == null || response.Content == null) {
        return new ObjectResponseResult<T>(default(T), string.Empty);
      }

      if (ReadResponseAsString) {
        var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        try {
          var typedBody =
            JsonConvert.DeserializeObject<T>(responseText, JsonSerializerSettings);
          return new ObjectResponseResult<T>(typedBody, responseText);
        }
        catch (JsonException exception) {
          var message = "Could not deserialize the response body string as " + typeof(T).FullName + ".";
          throw new ApiException(message, (int)response.StatusCode, responseText, headers, exception);
        }
      }
      else {
        try {
          using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
          using (var streamReader = new System.IO.StreamReader(responseStream))
          using (var jsonTextReader = new JsonTextReader(streamReader)) {
            var serializer = JsonSerializer.Create(JsonSerializerSettings);
            var typedBody = serializer.Deserialize<T>(jsonTextReader);
            return new ObjectResponseResult<T>(typedBody, string.Empty);
          }
        }
        catch (JsonException exception) {
          var message = "Could not deserialize the response body stream as " + typeof(T).FullName + ".";
          throw new ApiException(message, (int)response.StatusCode, string.Empty, headers, exception);
        }
      }
    }

    private string ConvertToString(object value, CultureInfo cultureInfo) {
      if (value == null) {
        return "";
      }

      if (value is Enum) {
        var name = Enum.GetName(value.GetType(), value);
        if (name != null) {
          var field = System.Reflection.IntrospectionExtensions.GetTypeInfo(value.GetType())
            .GetDeclaredField(name);
          if (field != null) {
            var attribute = System.Reflection.CustomAttributeExtensions.GetCustomAttribute(field,
                typeof(System.Runtime.Serialization.EnumMemberAttribute))
              as System.Runtime.Serialization.EnumMemberAttribute;
            if (attribute != null) {
              return attribute.Value != null ? attribute.Value : name;
            }
          }

          var converted = Convert.ToString(Convert.ChangeType(value,
            Enum.GetUnderlyingType(value.GetType()), cultureInfo));
          return converted == null ? string.Empty : converted;
        }
      }
      else if (value is bool) {
        return Convert.ToString((bool)value, cultureInfo).ToLowerInvariant();
      }
      else if (value is byte[]) {
        return Convert.ToBase64String((byte[])value);
      }
      else if (value.GetType().IsArray) {
        var array = ((Array)value).OfType<object>();
        return string.Join(",", array.Select(o => ConvertToString(o, cultureInfo)));
      }

      var result = Convert.ToString(value, cultureInfo);
      return result == null ? "" : result;
    }

    protected struct ObjectResponseResult<T> {
      public ObjectResponseResult(T responseObject, string responseText) {
        this.Object = responseObject;
        this.Text = responseText;
      }

      public T Object { get; }

      public string Text { get; }
    }
  }

  /// <summary>
  /// Get a page
  /// </summary>
  public class Page {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Page identifier
    /// </summary>
    [JsonProperty("id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    /// <summary>
    /// Timestamp the record was created
    /// </summary>
    [JsonProperty("created_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Timestamp the record was last updated
    /// </summary>
    [JsonProperty("updated_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    /// Name of your page to be displayed
    /// </summary>
    [JsonProperty("name", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    [JsonProperty("page_description", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string PageDescription { get; set; }

    [JsonProperty("headline", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Headline { get; set; }

    /// <summary>
    /// The main template your statuspage will use
    /// </summary>
    [JsonProperty("branding", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Branding { get; set; }

    /// <summary>
    /// Subdomain at which to access your status page
    /// </summary>
    [JsonProperty("subdomain", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Subdomain { get; set; }

    /// <summary>
    /// CNAME alias for your status page
    /// </summary>
    [JsonProperty("domain", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Domain { get; set; }

    /// <summary>
    /// Website of your page.  Clicking on your statuspage image will link here.
    /// </summary>
    [JsonProperty("url", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Url { get; set; }

    [JsonProperty("support_url", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string SupportUrl { get; set; }

    /// <summary>
    /// Should your page hide itself from search engines
    /// </summary>
    [JsonProperty("hidden_from_search", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool HiddenFromSearch { get; set; }

    /// <summary>
    /// Can your users subscribe to all notifications on the page
    /// </summary>
    [JsonProperty("allow_page_subscribers", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool AllowPageSubscribers { get; set; }

    /// <summary>
    /// Can your users subscribe to notifications for a single incident
    /// </summary>
    [JsonProperty("allow_incident_subscribers", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool AllowIncidentSubscribers { get; set; }

    /// <summary>
    /// Can your users choose to receive notifications via email
    /// </summary>
    [JsonProperty("allow_email_subscribers", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool AllowEmailSubscribers { get; set; }

    /// <summary>
    /// Can your users choose to receive notifications via SMS
    /// </summary>
    [JsonProperty("allow_sms_subscribers", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool AllowSmsSubscribers { get; set; }

    /// <summary>
    /// Can your users choose to access incident feeds via RSS/Atom (not functional on Audience-Specific pages)
    /// </summary>
    [JsonProperty("allow_rss_atom_feeds", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool AllowRssAtomFeeds { get; set; }

    /// <summary>
    /// Can your users choose to receive notifications via Webhooks
    /// </summary>
    [JsonProperty("allow_webhook_subscribers", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool AllowWebhookSubscribers { get; set; }

    /// <summary>
    /// Allows you to customize the email address your page notifications come from
    /// </summary>
    [JsonProperty("notifications_from_email", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string NotificationsFromEmail { get; set; }

    /// <summary>
    /// Allows you to customize the footer appearing on your notification emails.  Accepts Markdown for formatting
    /// </summary>
    [JsonProperty("notifications_email_footer", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string NotificationsEmailFooter { get; set; }

    [JsonProperty("activity_score", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public float ActivityScore { get; set; }

    [JsonProperty("twitter_username", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string TwitterUsername { get; set; }

    [JsonProperty("viewers_must_be_team_members", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool ViewersMustBeTeamMembers { get; set; }

    [JsonProperty("ip_restrictions", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string IpRestrictions { get; set; }

    [JsonProperty("city", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string City { get; set; }

    [JsonProperty("state", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string State { get; set; }

    [JsonProperty("country", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Country { get; set; }

    /// <summary>
    /// Timezone configured for your page
    /// </summary>
    [JsonProperty("time_zone", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string TimeZone { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_body_background_color", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssBodyBackgroundColor { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_font_color", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssFontColor { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_light_font_color", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssLightFontColor { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_greens", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssGreens { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_yellows", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssYellows { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_oranges", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssOranges { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_blues", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssBlues { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_reds", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssReds { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_border_color", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssBorderColor { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_graph_color", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssGraphColor { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_link_color", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssLinkColor { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_no_data", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssNoData { get; set; }

    [JsonProperty("favicon_logo", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string FaviconLogo { get; set; }

    [JsonProperty("transactional_logo", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string TransactionalLogo { get; set; }

    [JsonProperty("hero_cover", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string HeroCover { get; set; }

    [JsonProperty("email_logo", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string EmailLogo { get; set; }

    [JsonProperty("twitter_logo", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string TwitterLogo { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Get a list of users
  /// </summary>
  public class ErrorEntity {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("message", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Message { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a page
  /// </summary>
  public class PatchPages {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("page", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public Page2 Page { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a page
  /// </summary>
  public class PutPages {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("page", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public Page3 Page { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Add a page access user
  /// </summary>
  public class PostPagesPageIdPageAccessUsers {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("page_access_user", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public PageAccessUser PageAccessUser { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Delete metric for page access user
  /// </summary>
  public class PageAccessUser {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Page Access User Identifier
    /// </summary>
    [JsonProperty("id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    [JsonProperty("page_id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string PageId { get; set; }

    [JsonProperty("email", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Email { get; set; }

    /// <summary>
    /// IDP login user id. Key is typically "uid".
    /// </summary>
    [JsonProperty("external_login", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string ExternalLogin { get; set; }

    [JsonProperty("page_access_group_id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string PageAccessGroupId { get; set; }

    [JsonProperty("page_access_group_ids", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> PageAccessGroupIds { get; set; }

    [JsonProperty("subscribe_to_components", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool SubscribeToComponents { get; set; }

    [JsonProperty("created_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonProperty("updated_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset UpdatedAt { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Add components for page access user
  /// </summary>
  public class PatchPagesPageIdPageAccessUsersPageAccessUserIdComponents {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// List of component codes to allow access to
    /// </summary>
    [JsonProperty("component_ids", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public ICollection<string> ComponentIds { get; set; } =
      new System.Collections.ObjectModel.Collection<string>();

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Add components for page access user
  /// </summary>
  public class PutPagesPageIdPageAccessUsersPageAccessUserIdComponents {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// List of component codes to allow access to
    /// </summary>
    [JsonProperty("component_ids", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public ICollection<string> ComponentIds { get; set; } =
      new System.Collections.ObjectModel.Collection<string>();

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Replace components for page access user
  /// </summary>
  public class PostPagesPageIdPageAccessUsersPageAccessUserIdComponents {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// List of component codes to allow access to
    /// </summary>
    [JsonProperty("component_ids", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public ICollection<string> ComponentIds { get; set; } =
      new System.Collections.ObjectModel.Collection<string>();

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Remove components for page access user
  /// </summary>
  public class DeletePagesPageIdPageAccessUsersPageAccessUserIdComponents {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// List of components codes to remove.  If omitted, all components will be removed.
    /// </summary>
    [JsonProperty("component_ids", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> ComponentIds { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Add page access groups to a component
  /// </summary>
  public class Component {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Identifier for component
    /// </summary>
    [JsonProperty("id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    /// <summary>
    /// Page identifier
    /// </summary>
    [JsonProperty("page_id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string PageId { get; set; }

    /// <summary>
    /// Component Group identifier
    /// </summary>
    [JsonProperty("group_id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string GroupId { get; set; }

    [JsonProperty("created_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonProperty("updated_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    /// Is this component a group
    /// </summary>
    [JsonProperty("group", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool Group { get; set; }

    /// <summary>
    /// Display name for component
    /// </summary>
    [JsonProperty("name", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    /// <summary>
    /// More detailed description for component
    /// </summary>
    [JsonProperty("description", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Description { get; set; }

    /// <summary>
    /// Order the component will appear on the page
    /// </summary>
    [JsonProperty("position", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public int Position { get; set; }

    /// <summary>
    /// Status of component
    /// </summary>
    [JsonProperty("status", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public ComponentStatus Status { get; set; }

    /// <summary>
    /// Should this component be showcased
    /// </summary>
    [JsonProperty("showcase", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool Showcase { get; set; }

    /// <summary>
    /// Requires a special feature flag to be enabled
    /// </summary>
    [JsonProperty("only_show_if_degraded", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool OnlyShowIfDegraded { get; set; }

    /// <summary>
    /// Requires a special feature flag to be enabled
    /// </summary>
    [JsonProperty("automation_email", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string AutomationEmail { get; set; }

    /// <summary>
    /// The date this component started being used
    /// </summary>
    [JsonProperty("start_date", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(DateFormatConverter))]
    public DateTimeOffset StartDate { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Add metrics for page access user
  /// </summary>
  public class PatchPagesPageIdPageAccessUsersPageAccessUserIdMetrics {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// List of metrics to add
    /// </summary>
    [JsonProperty("metric_ids", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public ICollection<string> MetricIds { get; set; } =
      new System.Collections.ObjectModel.Collection<string>();

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Add metrics for page access user
  /// </summary>
  public class PutPagesPageIdPageAccessUsersPageAccessUserIdMetrics {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// List of metrics to add
    /// </summary>
    [JsonProperty("metric_ids", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public ICollection<string> MetricIds { get; set; } =
      new System.Collections.ObjectModel.Collection<string>();

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Replace metrics for page access user
  /// </summary>
  public class PostPagesPageIdPageAccessUsersPageAccessUserIdMetrics {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// List of metrics to add
    /// </summary>
    [JsonProperty("metric_ids", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public ICollection<string> MetricIds { get; set; } =
      new System.Collections.ObjectModel.Collection<string>();

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Delete metrics for page access user
  /// </summary>
  public class DeletePagesPageIdPageAccessUsersPageAccessUserIdMetrics {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// List of metrics to remove
    /// </summary>
    [JsonProperty("metric_ids", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> MetricIds { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Create a metric for a metric provider
  /// </summary>
  public class Metric {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Metric identifier
    /// </summary>
    [JsonProperty("id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    /// <summary>
    /// Metric Provider identifier
    /// </summary>
    [JsonProperty("metrics_provider_id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string MetricsProviderId { get; set; }

    /// <summary>
    /// Metric Display identifier used to look up the metric data from the provider
    /// </summary>
    [JsonProperty("metric_identifier", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string MetricIdentifier { get; set; }

    /// <summary>
    /// Name of metric
    /// </summary>
    [JsonProperty("name", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    /// <summary>
    /// Should the metric be displayed
    /// </summary>
    [JsonProperty("display", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool Display { get; set; }

    [JsonProperty("tooltip_description", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string TooltipDescription { get; set; }

    [JsonProperty("backfilled", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool Backfilled { get; set; }

    [JsonProperty("y_axis_min", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public float YAxisMin { get; set; }

    [JsonProperty("y_axis_max", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public float YAxisMax { get; set; }

    /// <summary>
    /// Should the values on the y axis be hidden on render
    /// </summary>
    [JsonProperty("y_axis_hidden", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool YAxisHidden { get; set; }

    /// <summary>
    /// Suffix to describe the units on the graph
    /// </summary>
    [JsonProperty("suffix", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Suffix { get; set; }

    [JsonProperty("decimal_places", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public int DecimalPlaces { get; set; }

    [JsonProperty("most_recent_data_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset MostRecentDataAt { get; set; }

    [JsonProperty("created_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonProperty("updated_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset UpdatedAt { get; set; }

    [JsonProperty("last_fetched_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset LastFetchedAt { get; set; }

    [JsonProperty("backfill_percentage", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public int BackfillPercentage { get; set; }

    [JsonProperty("reference_name", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string ReferenceName { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Remove a component from a page access group
  /// </summary>
  public class PageAccessGroup {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Page Access Group Identifier
    /// </summary>
    [JsonProperty("id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    /// <summary>
    /// Page Identifier.
    /// </summary>
    [JsonProperty("page_id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string PageId { get; set; }

    /// <summary>
    /// Name for this Group.
    /// </summary>
    [JsonProperty("name", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    [JsonProperty("page_access_user_ids", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> PageAccessUserIds { get; set; }

    /// <summary>
    /// Associates group with external group.
    /// </summary>
    [JsonProperty("external_identifier", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string ExternalIdentifier { get; set; }

    [JsonProperty("metric_ids", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> MetricIds { get; set; }

    [JsonProperty("component_ids", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> ComponentIds { get; set; }

    [JsonProperty("created_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonProperty("updated_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset UpdatedAt { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Create a page access group
  /// </summary>
  public class PostPagesPageIdPageAccessGroups {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("page_access_group", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public PageAccessGroup PageAccessGroup { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a page access group
  /// </summary>
  public class PatchPagesPageIdPageAccessGroups {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("page_access_group", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public PageAccessGroup PageAccessGroup { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a page access group
  /// </summary>
  public class PutPagesPageIdPageAccessGroups {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("page_access_group", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public PageAccessGroup PageAccessGroup { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Add components to page access group
  /// </summary>
  public class PatchPagesPageIdPageAccessGroupsPageAccessGroupIdComponents {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// List of Component identifiers
    /// </summary>
    [JsonProperty("component_ids", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> ComponentIds { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Add components to page access group
  /// </summary>
  public class PutPagesPageIdPageAccessGroupsPageAccessGroupIdComponents {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// List of Component identifiers
    /// </summary>
    [JsonProperty("component_ids", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> ComponentIds { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Replace components for a page access group
  /// </summary>
  public class PostPagesPageIdPageAccessGroupsPageAccessGroupIdComponents {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// List of components codes to set on the page access group
    /// </summary>
    [JsonProperty("component_ids", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public ICollection<string> ComponentIds { get; set; } =
      new System.Collections.ObjectModel.Collection<string>();

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Delete components for a page access group
  /// </summary>
  public class DeletePagesPageIdPageAccessGroupsPageAccessGroupIdComponents {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("component_ids", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> ComponentIds { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Resend confirmations to a list of subscribers
  /// </summary>
  public class PostPagesPageIdSubscribersResendConfirmation {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// The array of subscriber codes to resend confirmations for, or "all" to resend confirmations to all subscribers. Only unconfirmed email subscribers will receive this notification.
    /// </summary>
    [JsonProperty("subscribers", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Subscribers { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Unsubscribe a list of subscribers
  /// </summary>
  public class PostPagesPageIdSubscribersUnsubscribe {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// The array of subscriber codes to unsubscribe (limited to 100), or "all" to unsubscribe all subscribers if the number of subscribers is less than 100.
    /// </summary>
    [JsonProperty("subscribers", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Subscribers { get; set; }

    /// <summary>
    /// If this is present, only unsubscribe subscribers of this type.
    /// </summary>
    [JsonProperty("type", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public PostPagesPageIdSubscribersUnsubscribeType Type { get; set; }

    /// <summary>
    /// If this is present, only unsubscribe subscribers in this state. Specify state "all" to unsubscribe subscribers in any states.
    /// </summary>
    [JsonProperty("state", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public PostPagesPageIdSubscribersUnsubscribeState State { get; set; } =
      PostPagesPageIdSubscribersUnsubscribeState.Active;

    /// <summary>
    /// If skip_unsubscription_notification is true, the subscribers do not receive any notifications when they are unsubscribed.
    /// </summary>
    [JsonProperty("skip_unsubscription_notification",
      Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool SkipUnsubscriptionNotification { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Reactivate a list of quarantined subscribers
  /// </summary>
  public class PostPagesPageIdSubscribersReactivate {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// The array of quarantined subscriber codes to reactivate, or "all" to reactivate all quarantined subscribers.
    /// </summary>
    [JsonProperty("subscribers", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Subscribers { get; set; }

    /// <summary>
    /// If this is present, only reactivate subscribers of this type.
    /// </summary>
    [JsonProperty("type", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public PostPagesPageIdSubscribersReactivateType Type { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Get a histogram of subscribers by type and then state
  /// </summary>
  public class SubscriberCountByTypeAndState {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("email", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public SubscriberCountByState Email { get; set; }

    [JsonProperty("sms", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public SubscriberCountByState Sms { get; set; }

    [JsonProperty("webhook", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public SubscriberCountByState Webhook { get; set; }

    [JsonProperty("integration_partner", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public SubscriberCountByState IntegrationPartner { get; set; }

    [JsonProperty("slack", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public SubscriberCountByState Slack { get; set; }

    [JsonProperty("teams", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public SubscriberCountByState Teams { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  public class SubscriberCountByState {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// The number of active subscribers found by the query.
    /// </summary>
    [JsonProperty("active", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public int Active { get; set; }

    /// <summary>
    /// The number of unconfirmed subscribers found by the query.
    /// </summary>
    [JsonProperty("unconfirmed", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public int Unconfirmed { get; set; }

    /// <summary>
    /// The number of quarantined subscribers found by the query.
    /// </summary>
    [JsonProperty("quarantined", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public int Quarantined { get; set; }

    /// <summary>
    /// The total number of subscribers found by the query.
    /// </summary>
    [JsonProperty("total", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public int Total { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Get a count of subscribers by type
  /// </summary>
  public class SubscriberCountByType {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// The number of Email subscribers found by the query.
    /// </summary>
    [JsonProperty("email", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public int Email { get; set; }

    /// <summary>
    /// The number of Webhook subscribers found by the query.
    /// </summary>
    [JsonProperty("sms", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public int Sms { get; set; }

    /// <summary>
    /// The number of SMS subscribers found by the query.
    /// </summary>
    [JsonProperty("webhook", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public int Webhook { get; set; }

    /// <summary>
    /// The number of integration partners found by the query.
    /// </summary>
    [JsonProperty("integration_partner", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public int IntegrationPartner { get; set; }

    /// <summary>
    /// The number of Slack subscribers found by the query.
    /// </summary>
    [JsonProperty("slack", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public int Slack { get; set; }

    /// <summary>
    /// The number of MS teams subscribers found by the query.
    /// </summary>
    [JsonProperty("teams", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public int Teams { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Get an incident subscriber
  /// </summary>
  public class Subscriber {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Subscriber Identifier
    /// </summary>
    [JsonProperty("id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    /// <summary>
    /// If this is true, do not notify the user with changes to their subscription.
    /// </summary>
    [JsonProperty("skip_confirmation_notification",
      Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool SkipConfirmationNotification { get; set; }

    /// <summary>
    /// The communication mode of the subscriber.
    /// </summary>
    [JsonProperty("mode", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Mode { get; set; }

    /// <summary>
    /// The email address to use to contact the subscriber. Used for Email and Webhook subscribers.
    /// </summary>
    [JsonProperty("email", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Email { get; set; }

    /// <summary>
    /// The URL where a webhook subscriber elects to receive updates.
    /// </summary>
    [JsonProperty("endpoint", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Endpoint { get; set; }

    /// <summary>
    /// The phone number used to contact an SMS subscriber
    /// </summary>
    [JsonProperty("phone_number", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string PhoneNumber { get; set; }

    /// <summary>
    /// The two-character country code representing the country of which the phone_number is a part.
    /// </summary>
    [JsonProperty("phone_country", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string PhoneCountry { get; set; }

    /// <summary>
    /// A formatted version of the phone_number and phone_country pair, nicely formatted for display.
    /// </summary>
    [JsonProperty("display_phone_number", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string DisplayPhoneNumber { get; set; }

    /// <summary>
    /// Obfuscated slack channel name
    /// </summary>
    [JsonProperty("obfuscated_channel_name", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string ObfuscatedChannelName { get; set; }

    /// <summary>
    /// The workspace name of the slack subscriber.
    /// </summary>
    [JsonProperty("workspace_name", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string WorkspaceName { get; set; }

    /// <summary>
    /// The timestamp when the subscriber was quarantined due to an issue reaching them.
    /// </summary>
    [JsonProperty("quarantined_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset QuarantinedAt { get; set; }

    /// <summary>
    /// The timestamp when a quarantined subscriber will be purged (unsubscribed).
    /// </summary>
    [JsonProperty("purge_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset PurgeAt { get; set; }

    /// <summary>
    /// The components for which the subscriber has elected to receive updates.
    /// </summary>
    [JsonProperty("components", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Components { get; set; }

    /// <summary>
    /// The Page Access user this subscriber belongs to (only for audience-specific pages).
    /// </summary>
    [JsonProperty("page_access_user_id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string PageAccessUserId { get; set; }

    [JsonProperty("created_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// The code of the page access user to which the subscriber belongs.
    /// </summary>
    [JsonProperty("page_access_user", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string PageAccessUser { get; set; }

    /// <summary>
    /// A list of component ids for which the subscriber should recieve updates for. Components must be an array with at least one element if it is passed at all. Each component must belong to the page indicated in the path.
    /// </summary>
    [JsonProperty("component_ids", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> ComponentIds { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Create a subscriber. Not applicable for Slack subscribers.
  /// </summary>
  public class PostPagesPageIdSubscribers {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("subscriber", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public Subscriber Subscriber { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a subscriber
  /// </summary>
  public class PatchPagesPageIdSubscribers {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// A list of component ids for which the subscriber should recieve updates for. Components must be an array with at least one element if it is passed at all. Each component must belong to the page indicated in the path. To set the subscriber to be subscribed to all components on the page, exclude this parameter.
    /// </summary>
    [JsonProperty("component_ids", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> ComponentIds { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Create a template
  /// </summary>
  public class PostPagesPageIdIncidentTemplates {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("template", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public Template Template { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Get a list of templates
  /// </summary>
  public class IncidentTemplate {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Incident Template Identifier
    /// </summary>
    [JsonProperty("id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    /// <summary>
    /// Affected components
    /// </summary>
    [JsonProperty("components", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<Component> Components { get; set; }

    /// <summary>
    /// Name of the template, as shown in the list on the "Templates" tab of the "Incidents" page
    /// </summary>
    [JsonProperty("name", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    /// <summary>
    /// Title to be applied to the incident or maintenance when selecting this template
    /// </summary>
    [JsonProperty("title", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Title { get; set; }

    /// <summary>
    /// Body of the incident or maintenance update to be applied when selecting this template
    /// </summary>
    [JsonProperty("body", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Body { get; set; }

    /// <summary>
    /// Identifier of Template Group this template belongs to
    /// </summary>
    [JsonProperty("group_id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string GroupId { get; set; }

    /// <summary>
    /// The status the incident or maintenance should transition to when selecting this template
    /// </summary>
    [JsonProperty("update_status", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public IncidentTemplateUpdateStatus UpdateStatus { get; set; }

    /// <summary>
    /// Whether the "tweet update" checkbox should be selected when selecting this template
    /// </summary>
    [JsonProperty("should_tweet", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool ShouldTweet { get; set; }

    /// <summary>
    /// Whether the "deliver notifications" checkbox should be selected when selecting this template
    /// </summary>
    [JsonProperty("should_send_notifications", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool ShouldSendNotifications { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Create an incident
  /// </summary>
  public class PostPagesPageIdIncidents {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("incident", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public Incident Incident { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Get an incident
  /// </summary>
  public class Incident {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Incident Identifier
    /// </summary>
    [JsonProperty("id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    /// <summary>
    /// Incident components
    /// </summary>
    [JsonProperty("components", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<Component> Components { get; set; }

    /// <summary>
    /// The timestamp when the incident was created at.
    /// </summary>
    [JsonProperty("created_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// The impact of the incident.
    /// </summary>
    [JsonProperty("impact", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public IncidentImpactOverride Impact { get; set; }

    /// <summary>
    /// value to override calculated impact value
    /// </summary>
    [JsonProperty("impact_override", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public IncidentImpactOverride ImpactOverride { get; set; }

    /// <summary>
    /// The incident updates for incident.
    /// </summary>
    [JsonProperty("incident_updates", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<IncidentUpdate> IncidentUpdates { get; set; }

    /// <summary>
    /// The incident impacts for the incident.
    /// </summary>
    [JsonProperty("incident_impacts", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<IncidentImpact> IncidentImpacts { get; set; }

    /// <summary>
    /// Metadata attached to the incident. Top level values must be objects.
    /// </summary>
    [JsonProperty("metadata", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public object Metadata { get; set; }

    /// <summary>
    /// The timestamp when incident entered monitoring state.
    /// </summary>
    [JsonProperty("monitoring_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset MonitoringAt { get; set; }

    /// <summary>
    /// Incident Name. There is a maximum limit of 255 characters.
    /// </summary>
    [JsonProperty("name", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    /// <summary>
    /// Incident Page Identifier
    /// </summary>
    [JsonProperty("page_id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string PageId { get; set; }

    /// <summary>
    /// Body of the Postmortem.
    /// </summary>
    [JsonProperty("postmortem_body", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string PostmortemBody { get; set; }

    /// <summary>
    /// The timestamp when the incident postmortem body was last updated at.
    /// </summary>
    [JsonProperty("postmortem_body_last_updated_at",
      Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset PostmortemBodyLastUpdatedAt { get; set; }

    /// <summary>
    /// Controls whether the incident will have postmortem.
    /// </summary>
    [JsonProperty("postmortem_ignored", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool PostmortemIgnored { get; set; }

    /// <summary>
    /// Indicates whether subscribers are already notificed about postmortem.
    /// </summary>
    [JsonProperty("postmortem_notified_subscribers",
      Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool PostmortemNotifiedSubscribers { get; set; }

    /// <summary>
    /// Controls whether to decide if notify postmortem on twitter.
    /// </summary>
    [JsonProperty("postmortem_notified_twitter", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool PostmortemNotifiedTwitter { get; set; }

    /// <summary>
    /// The timestamp when the postmortem was published.
    /// </summary>
    [JsonProperty("postmortem_published_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool PostmortemPublishedAt { get; set; }

    /// <summary>
    /// The timestamp when incident was resolved.
    /// </summary>
    [JsonProperty("resolved_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset ResolvedAt { get; set; }

    /// <summary>
    /// Controls whether the incident is scheduled to automatically change to complete.
    /// </summary>
    [JsonProperty("scheduled_auto_completed", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool ScheduledAutoCompleted { get; set; }

    /// <summary>
    /// Controls whether the incident is scheduled to automatically change to in progress.
    /// </summary>
    [JsonProperty("scheduled_auto_in_progress", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool ScheduledAutoInProgress { get; set; }

    /// <summary>
    /// The timestamp the incident is scheduled for.
    /// </summary>
    [JsonProperty("scheduled_for", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset ScheduledFor { get; set; }

    /// <summary>
    /// Controls whether send notification when scheduled maintenances auto transition to completed.
    /// </summary>
    [JsonProperty("auto_transition_deliver_notifications_at_end",
      Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool AutoTransitionDeliverNotificationsAtEnd { get; set; }

    /// <summary>
    /// Controls whether send notification when scheduled maintenances auto transition to started.
    /// </summary>
    [JsonProperty("auto_transition_deliver_notifications_at_start",
      Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool AutoTransitionDeliverNotificationsAtStart { get; set; }

    /// <summary>
    /// Controls whether change components status to under_maintenance once scheduled maintenance is in progress.
    /// </summary>
    [JsonProperty("auto_transition_to_maintenance_state",
      Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool AutoTransitionToMaintenanceState { get; set; }

    /// <summary>
    /// Controls whether change components status to operational once scheduled maintenance completes.
    /// </summary>
    [JsonProperty("auto_transition_to_operational_state",
      Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool AutoTransitionToOperationalState { get; set; }

    /// <summary>
    /// Controls whether to remind subscribers prior to scheduled incidents.
    /// </summary>
    [JsonProperty("scheduled_remind_prior", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool ScheduledRemindPrior { get; set; }

    /// <summary>
    /// The timestamp when the scheduled incident reminder was sent at.
    /// </summary>
    [JsonProperty("scheduled_reminded_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset ScheduledRemindedAt { get; set; }

    /// <summary>
    /// The timestamp the incident is scheduled until.
    /// </summary>
    [JsonProperty("scheduled_until", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset ScheduledUntil { get; set; }

    /// <summary>
    /// Incident Shortlink.
    /// </summary>
    [JsonProperty("shortlink", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Shortlink { get; set; }

    /// <summary>
    /// The incident status. For realtime incidents, valid values are investigating, identified, monitoring, and resolved. For scheduled incidents, valid values are scheduled, in_progress, verifying, and completed.
    /// </summary>
    [JsonProperty("status", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public IncidentStatus Status { get; set; }

    /// <summary>
    /// The timestamp when the incident was updated at.
    /// </summary>
    [JsonProperty("updated_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    /// Custom reminder intervals for unresolved/open incidents. Not applicable for &lt;strong&gt;Scheduled maintenance&lt;/strong&gt;&lt;br&gt;There are 4 possible states for reminder_intervals:&lt;br&gt;&lt;strong&gt;DEFAULT:&lt;/strong&gt; NULL, representing a default behavior with intervals [3, 6, 12, 24].&lt;br&gt;&lt;strong&gt;AFTER:&lt;/strong&gt; A serialized array of strictly increasing intervals, each integer ranges from [1-24] (inclusive). Ex "[1, 5, 7, 10]"&lt;br&gt;&lt;strong&gt;EVERY:&lt;/strong&gt; An integer in the range [1-24] as a string, representing equal intervals. Ex "4" for [4, 8, 12, 16, 20, 24]&lt;br&gt;&lt;strong&gt;OFF:&lt;/strong&gt; A serialized empty array, for example, "[]", meaning no reminder notifications will be sent.
    /// </summary>
    [JsonProperty("reminder_intervals", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string ReminderIntervals { get; set; }

    /// <summary>
    /// Deliver notifications to subscribers if this is true. If this is false, create an incident without notifying customers.
    /// </summary>
    [JsonProperty("deliver_notifications", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool DeliverNotifications { get; set; } = true;

    /// <summary>
    /// Controls whether tweet automatically when scheduled maintenance starts.
    /// </summary>
    [JsonProperty("auto_tweet_at_beginning", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool AutoTweetAtBeginning { get; set; }

    /// <summary>
    /// Controls whether tweet automatically when scheduled maintenance completes.
    /// </summary>
    [JsonProperty("auto_tweet_on_completion", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool AutoTweetOnCompletion { get; set; }

    /// <summary>
    /// Controls whether tweet automatically when scheduled maintenance is created.
    /// </summary>
    [JsonProperty("auto_tweet_on_creation", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool AutoTweetOnCreation { get; set; }

    /// <summary>
    /// Controls whether tweet automatically one hour before scheduled maintenance starts.
    /// </summary>
    [JsonProperty("auto_tweet_one_hour_before", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool AutoTweetOneHourBefore { get; set; }

    /// <summary>
    /// TimeStamp when incident was backfilled.
    /// </summary>
    [JsonProperty("backfill_date", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string BackfillDate { get; set; }

    /// <summary>
    /// Controls whether incident is backfilled. If true, components cannot be specified.
    /// </summary>
    [JsonProperty("backfilled", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool Backfilled { get; set; }

    /// <summary>
    /// The initial message, created as the first incident update. There is a maximum limit of 25000 characters
    /// </summary>
    [JsonProperty("body", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Body { get; set; }

    /// <summary>
    /// List of component_ids affected by this incident
    /// </summary>
    [JsonProperty("component_ids", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> ComponentIds { get; set; }

    /// <summary>
    /// Same as :scheduled_auto_transition_in_progress. Controls whether the incident is scheduled to automatically change to in progress.
    /// </summary>
    [JsonProperty("scheduled_auto_transition", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool ScheduledAutoTransition { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a previous incident update
  /// </summary>
  public class IncidentUpdate {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Incident Update Identifier.
    /// </summary>
    [JsonProperty("id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    /// <summary>
    /// Incident Identifier.
    /// </summary>
    [JsonProperty("incident_id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string IncidentId { get; set; }

    /// <summary>
    /// Affected components associated with the incident update.
    /// </summary>
    [JsonProperty("affected_components", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<object> AffectedComponents { get; set; }

    /// <summary>
    /// Incident update body.
    /// </summary>
    [JsonProperty("body", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Body { get; set; }

    /// <summary>
    /// The timestamp when the incident update was created at.
    /// </summary>
    [JsonProperty("created_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// An optional customized tweet message for incident postmortem.
    /// </summary>
    [JsonProperty("custom_tweet", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CustomTweet { get; set; }

    /// <summary>
    /// Controls whether to delivery notifications.
    /// </summary>
    [JsonProperty("deliver_notifications", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool DeliverNotifications { get; set; }

    /// <summary>
    /// Timestamp when incident update is happened.
    /// </summary>
    [JsonProperty("display_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset DisplayAt { get; set; }

    /// <summary>
    /// The incident status. For realtime incidents, valid values are investigating, identified, monitoring, and resolved. For scheduled incidents, valid values are scheduled, in_progress, verifying, and completed.
    /// </summary>
    [JsonProperty("status", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public IncidentUpdateStatus Status { get; set; }

    /// <summary>
    /// Tweet identifier associated to this incident update.
    /// </summary>
    [JsonProperty("tweet_id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string TweetId { get; set; }

    /// <summary>
    /// The timestamp when twitter updated at.
    /// </summary>
    [JsonProperty("twitter_updated_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset TwitterUpdatedAt { get; set; }

    /// <summary>
    /// The timestamp when the incident update is updated.
    /// </summary>
    [JsonProperty("updated_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    /// Controls whether to create twitter update.
    /// </summary>
    [JsonProperty("wants_twitter_update", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool WantsTwitterUpdate { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  public class IncidentImpact {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Incident Impact Identifier
    /// </summary>
    [JsonProperty("id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    /// <summary>
    /// The tenant ID associated with the impact.
    /// </summary>
    [JsonProperty("tenant_id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string TenantId { get; set; }

    /// <summary>
    /// The Atlassian organization ID associated with the impact.
    /// </summary>
    [JsonProperty("atlassian_organization_id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string AtlassianOrganizationId { get; set; }

    /// <summary>
    /// The product name associated with the impact.
    /// </summary>
    [JsonProperty("product_name", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string ProductName { get; set; }

    /// <summary>
    /// The list of experiences impacted.
    /// </summary>
    [JsonProperty("experiences", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> Experiences { get; set; }

    /// <summary>
    /// The timestamp when the impact was created.
    /// </summary>
    [JsonProperty("created_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update an incident
  /// </summary>
  public class PatchPagesPageIdIncidents {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("incident", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public Incident Incident { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update an incident
  /// </summary>
  public class PutPagesPageIdIncidents {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("incident", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public Incident Incident { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a previous incident update
  /// </summary>
  public class PatchPagesPageIdIncidentsIncidentIdIncidentUpdates {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("incident_update", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public IncidentUpdate IncidentUpdate { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a previous incident update
  /// </summary>
  public class PutPagesPageIdIncidentsIncidentIdIncidentUpdates {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("incident_update", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public IncidentUpdate IncidentUpdate { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Create an incident subscriber
  /// </summary>
  public class PostPagesPageIdIncidentsIncidentIdSubscribers {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("subscriber", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public Subscriber3 Subscriber { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Revert Postmortem
  /// </summary>
  public class Postmortem {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Preview Key
    /// </summary>
    [JsonProperty("preview_key", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string PreviewKey { get; set; }

    /// <summary>
    /// Postmortem body
    /// </summary>
    [JsonProperty("body", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Body { get; set; }

    [JsonProperty("body_updated_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset BodyUpdatedAt { get; set; }

    /// <summary>
    /// Body draft
    /// </summary>
    [JsonProperty("body_draft", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string BodyDraft { get; set; }

    [JsonProperty("body_draft_updated_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset BodyDraftUpdatedAt { get; set; }

    [JsonProperty("published_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset PublishedAt { get; set; }

    /// <summary>
    /// Should email subscribers be notified.
    /// </summary>
    [JsonProperty("notify_subscribers", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool NotifySubscribers { get; set; }

    /// <summary>
    /// Should Twitter followers be notified.
    /// </summary>
    [JsonProperty("notify_twitter", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool NotifyTwitter { get; set; }

    /// <summary>
    /// Custom tweet for Incident Postmortem
    /// </summary>
    [JsonProperty("custom_tweet", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CustomTweet { get; set; }

    [JsonProperty("created_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonProperty("updated_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset UpdatedAt { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Create Postmortem
  /// </summary>
  public class PutPagesPageIdIncidentsIncidentIdPostmortem {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("postmortem", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public Postmortem2 Postmortem { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Publish Postmortem
  /// </summary>
  public class PutPagesPageIdIncidentsIncidentIdPostmortemPublish {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("postmortem", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public Postmortem3 Postmortem { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Create a component
  /// </summary>
  public class PostPagesPageIdComponents {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("component", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public Component Component { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a component
  /// </summary>
  public class PatchPagesPageIdComponents {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("component", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public Component Component { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a component
  /// </summary>
  public class PutPagesPageIdComponents {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("component", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public Component Component { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Get uptime data for a component that has uptime showcase enabled
  /// </summary>
  public class ComponentUptime {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Start date used for uptime calculation (see the warnings field in the response if this value does not match the start parameter you provided).
    /// </summary>
    [JsonProperty("range_start", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset RangeStart { get; set; }

    /// <summary>
    /// End date used for uptime calculation (see the warnings field in the response if this value does not match the end parameter you provided).
    /// </summary>
    [JsonProperty("range_end", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset RangeEnd { get; set; }

    /// <summary>
    /// Uptime percentage for a component
    /// </summary>
    [JsonProperty("uptime_percentage", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public float UptimePercentage { get; set; }

    /// <summary>
    /// Seconds of major outage
    /// </summary>
    [JsonProperty("major_outage", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public int MajorOutage { get; set; }

    /// <summary>
    /// Seconds of partial outage
    /// </summary>
    [JsonProperty("partial_outage", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public int PartialOutage { get; set; }

    /// <summary>
    /// Warning messages related to the uptime query that may occur
    /// </summary>
    [JsonProperty("warnings", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Warnings { get; set; }

    /// <summary>
    /// Component identifier
    /// </summary>
    [JsonProperty("id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    /// <summary>
    /// Component display name
    /// </summary>
    [JsonProperty("name", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    /// <summary>
    /// Related incidents
    /// </summary>
    [JsonProperty("related_events", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public RelatedEvents RelatedEvents { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Create a component group
  /// </summary>
  public class PostPagesPageIdComponentGroups {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Description of the component group.
    /// </summary>
    [JsonProperty("description", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Description { get; set; }

    [JsonProperty("component_group", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public ComponentGroup ComponentGroup { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Get a component group
  /// </summary>
  public class GroupComponent {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Component Group Identifier
    /// </summary>
    [JsonProperty("id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    [JsonProperty("page_id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string PageId { get; set; }

    [JsonProperty("name", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    [JsonProperty("description", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Description { get; set; }

    [JsonProperty("components", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Components { get; set; }

    [JsonProperty("position", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Position { get; set; }

    [JsonProperty("created_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonProperty("updated_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset UpdatedAt { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a component group
  /// </summary>
  public class PatchPagesPageIdComponentGroups {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Updated description of the component group.
    /// </summary>
    [JsonProperty("description", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Description { get; set; }

    [JsonProperty("component_group", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public ComponentGroup2 ComponentGroup { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a component group
  /// </summary>
  public class PutPagesPageIdComponentGroups {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Updated description of the component group.
    /// </summary>
    [JsonProperty("description", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Description { get; set; }

    [JsonProperty("component_group", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public ComponentGroup3 ComponentGroup { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Get uptime data for a component group that has uptime showcase enabled for at least one component.
  /// </summary>
  public class ComponentGroupUptime {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Start date used for uptime calculation (see the warnings field in the response if this value does not match the start parameter you provided).
    /// </summary>
    [JsonProperty("range_start", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset RangeStart { get; set; }

    /// <summary>
    /// End date used for uptime calculation (see the warnings field in the response if this value does not match the end parameter you provided).
    /// </summary>
    [JsonProperty("range_end", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset RangeEnd { get; set; }

    /// <summary>
    /// Uptime percentage for a component
    /// </summary>
    [JsonProperty("uptime_percentage", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public float UptimePercentage { get; set; }

    /// <summary>
    /// Seconds of major outage
    /// </summary>
    [JsonProperty("major_outage", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public int MajorOutage { get; set; }

    /// <summary>
    /// Seconds of partial outage
    /// </summary>
    [JsonProperty("partial_outage", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public int PartialOutage { get; set; }

    /// <summary>
    /// Warning messages related to the uptime query that may occur
    /// </summary>
    [JsonProperty("warnings", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Warnings { get; set; }

    /// <summary>
    /// Component group identifier
    /// </summary>
    [JsonProperty("id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    /// <summary>
    /// Component group display name
    /// </summary>
    [JsonProperty("name", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    /// <summary>
    /// Related incidents by component
    /// </summary>
    [JsonProperty("related_events", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public RelatedEvents2 RelatedEvents { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Add data points to metrics
  /// </summary>
  public class MetricAddResponse {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Metric identifier to add data to
    /// </summary>
    [JsonProperty("metric_id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<MetricId> MetricId { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Add data points to metrics
  /// </summary>
  public class PostPagesPageIdMetricsData {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("data", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public MetricAddResponse Data { get; set; } = new MetricAddResponse();

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a metric
  /// </summary>
  public class PatchPagesPageIdMetrics {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("metric", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public Metric2 Metric { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a metric
  /// </summary>
  public class PutPagesPageIdMetrics {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("metric", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public Metric3 Metric { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Add data to a metric
  /// </summary>
  public class PostPagesPageIdMetricsMetricIdData {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("data", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public Data Data { get; set; } = new Data();

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Add data to a metric
  /// </summary>
  public class SingleMetricAddResponse {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("data", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public Data2 Data { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Delete a metric provider
  /// </summary>
  public class MetricsProvider {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Identifier for Metrics Provider
    /// </summary>
    [JsonProperty("id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    /// <summary>
    /// One of "Pingdom", "NewRelic", "Librato", "Datadog", or "Self"
    /// </summary>
    [JsonProperty("type", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Type { get; set; }

    [JsonProperty("disabled", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool Disabled { get; set; }

    [JsonProperty("metric_base_uri", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string MetricBaseUri { get; set; }

    [JsonProperty("last_revalidated_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset LastRevalidatedAt { get; set; }

    [JsonProperty("created_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonProperty("updated_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset UpdatedAt { get; set; }

    [JsonProperty("page_id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public int PageId { get; set; }

    /// <summary>
    /// Required by the Librato metrics provider.
    /// </summary>
    [JsonProperty("email", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Email { get; set; }

    [JsonProperty("password", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Password { get; set; }

    /// <summary>
    /// Required by the Datadog and NewRelic type metrics providers.
    /// </summary>
    [JsonProperty("api_key", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string ApiKey { get; set; }

    /// <summary>
    /// Required by the Librato, Datadog and Pingdom type metrics providers.
    /// </summary>
    [JsonProperty("api_token", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string ApiToken { get; set; }

    /// <summary>
    /// Required by the Pingdom-type metrics provider.
    /// </summary>
    [JsonProperty("application_key", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string ApplicationKey { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Create a metric provider
  /// </summary>
  public class PostPagesPageIdMetricsProviders {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("metrics_provider", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public MetricsProvider MetricsProvider { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a metric provider
  /// </summary>
  public class PatchPagesPageIdMetricsProviders {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("metrics_provider", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public MetricsProvider MetricsProvider { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a metric provider
  /// </summary>
  public class PutPagesPageIdMetricsProviders {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("metrics_provider", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public MetricsProvider MetricsProvider { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Create a metric for a metric provider
  /// </summary>
  public class PostPagesPageIdMetricsProvidersMetricsProviderIdMetrics {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("metric", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public Metric4 Metric { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update status embed config settings
  /// </summary>
  public class StatusEmbedConfig {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Page identifier
    /// </summary>
    [JsonProperty("page_id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string PageId { get; set; }

    /// <summary>
    /// Corner where status embed iframe will appear on page
    /// </summary>
    [JsonProperty("position", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Position { get; set; }

    /// <summary>
    /// Color of status embed iframe background when displaying incident
    /// </summary>
    [JsonProperty("incident_background_color", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string IncidentBackgroundColor { get; set; }

    /// <summary>
    /// Color of status embed iframe text when displaying incident
    /// </summary>
    [JsonProperty("incident_text_color", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string IncidentTextColor { get; set; }

    /// <summary>
    /// Color of status embed iframe background when displaying maintenance
    /// </summary>
    [JsonProperty("maintenance_background_color", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string MaintenanceBackgroundColor { get; set; }

    /// <summary>
    /// Color of status embed iframe text when displaying maintenance
    /// </summary>
    [JsonProperty("maintenance_text_color", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string MaintenanceTextColor { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update status embed config settings
  /// </summary>
  public class PatchPagesPageIdStatusEmbedConfig {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("status_embed_config", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public StatusEmbedConfig StatusEmbedConfig { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update status embed config settings
  /// </summary>
  public class PutPagesPageIdStatusEmbedConfig {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("status_embed_config", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public StatusEmbedConfig StatusEmbedConfig { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a user's role permissions. Payload should contain a mapping of pages to a set of the desired roles,
  /// <br/>                  if the page has Role Based Access Control. Otherwise, the pages should map to an empty hash.
  /// <br/>                  User will lose access to any pages omitted from the payload.
  /// </summary>
  public class PutOrganizationsOrganizationIdPermissions {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("pages", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public Pages Pages { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Get a user's permissions
  /// </summary>
  public class Permissions {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("data", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public Data3 Data { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Get a list of users
  /// </summary>
  public class User {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// User identifier
    /// </summary>
    [JsonProperty("id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    /// <summary>
    /// Organization identifier
    /// </summary>
    [JsonProperty("organization_id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string OrganizationId { get; set; }

    /// <summary>
    /// Email address for the team member
    /// </summary>
    [JsonProperty("email", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Email { get; set; }

    /// <summary>
    /// Password the team member uses to access the site
    /// </summary>
    [JsonProperty("password", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Password { get; set; }

    [JsonProperty("first_name", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string FirstName { get; set; }

    [JsonProperty("last_name", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string LastName { get; set; }

    [JsonProperty("created_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonProperty("updated_at", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset UpdatedAt { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  /// <summary>
  /// Create a user
  /// </summary>
  public class PostOrganizationsOrganizationIdUsers {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("user", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public User User { get; set; } = new User();

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  public enum Type {
    [System.Runtime.Serialization.EnumMember(Value = @"email")]
    Email = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"sms")]
    Sms = 1,

    [System.Runtime.Serialization.EnumMember(Value = @"slack")]
    Slack = 2,

    [System.Runtime.Serialization.EnumMember(Value = @"webhook")]
    Webhook = 3,

    [System.Runtime.Serialization.EnumMember(Value = @"teams")]
    Teams = 4,

    [System.Runtime.Serialization.EnumMember(Value = @"integration_partner")]
    IntegrationPartner = 5,
  }

  public enum State {
    [System.Runtime.Serialization.EnumMember(Value = @"active")]
    Active = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"unconfirmed")]
    Unconfirmed = 1,

    [System.Runtime.Serialization.EnumMember(Value = @"quarantined")]
    Quarantined = 2,

    [System.Runtime.Serialization.EnumMember(Value = @"all")]
    All = 3,
  }

  public enum SortField {
    [System.Runtime.Serialization.EnumMember(Value = @"primary")]
    Primary = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"created_at")]
    CreatedAt = 1,

    [System.Runtime.Serialization.EnumMember(Value = @"quarantined_at")]
    QuarantinedAt = 2,

    [System.Runtime.Serialization.EnumMember(Value = @"relevance")]
    Relevance = 3,
  }

  public enum SortDirection {
    [System.Runtime.Serialization.EnumMember(Value = @"asc")]
    Asc = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"desc")]
    Desc = 1,
  }

  public class Body {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// List of page access users to add to component
    /// </summary>
    [JsonProperty("page_access_user_ids", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public ICollection<string> PageAccessUserIds { get; set; } =
      new System.Collections.ObjectModel.Collection<string>();

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  public class Page2 {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Name of your page to be displayed
    /// </summary>
    [JsonProperty("name", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    /// <summary>
    /// CNAME alias for your status page
    /// </summary>
    [JsonProperty("domain", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Domain { get; set; }

    /// <summary>
    /// Subdomain at which to access your status page
    /// </summary>
    [JsonProperty("subdomain", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Subdomain { get; set; }

    /// <summary>
    /// Website of your page.  Clicking on your statuspage image will link here.
    /// </summary>
    [JsonProperty("url", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Url { get; set; }

    /// <summary>
    /// The main template your statuspage will use
    /// </summary>
    [JsonProperty("branding", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public Page2Branding Branding { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_body_background_color", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssBodyBackgroundColor { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_font_color", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssFontColor { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_light_font_color", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssLightFontColor { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_greens", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssGreens { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_yellows", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssYellows { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_oranges", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssOranges { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_reds", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssReds { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_blues", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssBlues { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_border_color", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssBorderColor { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_graph_color", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssGraphColor { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_link_color", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssLinkColor { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_no_data", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssNoData { get; set; }

    /// <summary>
    /// Should your page hide itself from search engines
    /// </summary>
    [JsonProperty("hidden_from_search", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool HiddenFromSearch { get; set; }

    [JsonProperty("viewers_must_be_team_members", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool ViewersMustBeTeamMembers { get; set; }

    /// <summary>
    /// Can your users subscribe to all notifications on the page
    /// </summary>
    [JsonProperty("allow_page_subscribers", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool AllowPageSubscribers { get; set; }

    /// <summary>
    /// Can your users subscribe to notifications for a single incident
    /// </summary>
    [JsonProperty("allow_incident_subscribers", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool AllowIncidentSubscribers { get; set; }

    /// <summary>
    /// Can your users choose to receive notifications via email
    /// </summary>
    [JsonProperty("allow_email_subscribers", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool AllowEmailSubscribers { get; set; }

    /// <summary>
    /// Can your users choose to receive notifications via SMS
    /// </summary>
    [JsonProperty("allow_sms_subscribers", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool AllowSmsSubscribers { get; set; }

    /// <summary>
    /// Can your users choose to access incident feeds via RSS/Atom (not functional on Audience-Specific pages)
    /// </summary>
    [JsonProperty("allow_rss_atom_feeds", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool AllowRssAtomFeeds { get; set; }

    /// <summary>
    /// Can your users choose to receive notifications via Webhooks
    /// </summary>
    [JsonProperty("allow_webhook_subscribers", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool AllowWebhookSubscribers { get; set; }

    /// <summary>
    /// Allows you to customize the email address your page notifications come from
    /// </summary>
    [JsonProperty("notifications_from_email", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string NotificationsFromEmail { get; set; }

    /// <summary>
    /// Timezone configured for your page
    /// </summary>
    [JsonProperty("time_zone", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string TimeZone { get; set; }

    /// <summary>
    /// Allows you to customize the footer appearing on your notification emails.  Accepts Markdown for formatting
    /// </summary>
    [JsonProperty("notifications_email_footer", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string NotificationsEmailFooter { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  public class Page3 {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Name of your page to be displayed
    /// </summary>
    [JsonProperty("name", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    /// <summary>
    /// CNAME alias for your status page
    /// </summary>
    [JsonProperty("domain", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Domain { get; set; }

    /// <summary>
    /// Subdomain at which to access your status page
    /// </summary>
    [JsonProperty("subdomain", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Subdomain { get; set; }

    /// <summary>
    /// Website of your page.  Clicking on your statuspage image will link here.
    /// </summary>
    [JsonProperty("url", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Url { get; set; }

    /// <summary>
    /// The main template your statuspage will use
    /// </summary>
    [JsonProperty("branding", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public Page3Branding Branding { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_body_background_color", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssBodyBackgroundColor { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_font_color", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssFontColor { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_light_font_color", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssLightFontColor { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_greens", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssGreens { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_yellows", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssYellows { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_oranges", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssOranges { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_reds", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssReds { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_blues", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssBlues { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_border_color", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssBorderColor { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_graph_color", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssGraphColor { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_link_color", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssLinkColor { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_no_data", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CssNoData { get; set; }

    /// <summary>
    /// Should your page hide itself from search engines
    /// </summary>
    [JsonProperty("hidden_from_search", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool HiddenFromSearch { get; set; }

    [JsonProperty("viewers_must_be_team_members", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool ViewersMustBeTeamMembers { get; set; }

    /// <summary>
    /// Can your users subscribe to all notifications on the page
    /// </summary>
    [JsonProperty("allow_page_subscribers", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool AllowPageSubscribers { get; set; }

    /// <summary>
    /// Can your users subscribe to notifications for a single incident
    /// </summary>
    [JsonProperty("allow_incident_subscribers", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool AllowIncidentSubscribers { get; set; }

    /// <summary>
    /// Can your users choose to receive notifications via email
    /// </summary>
    [JsonProperty("allow_email_subscribers", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool AllowEmailSubscribers { get; set; }

    /// <summary>
    /// Can your users choose to receive notifications via SMS
    /// </summary>
    [JsonProperty("allow_sms_subscribers", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool AllowSmsSubscribers { get; set; }

    /// <summary>
    /// Can your users choose to access incident feeds via RSS/Atom (not functional on Audience-Specific pages)
    /// </summary>
    [JsonProperty("allow_rss_atom_feeds", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool AllowRssAtomFeeds { get; set; }

    /// <summary>
    /// Can your users choose to receive notifications via Webhooks
    /// </summary>
    [JsonProperty("allow_webhook_subscribers", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool AllowWebhookSubscribers { get; set; }

    /// <summary>
    /// Allows you to customize the email address your page notifications come from
    /// </summary>
    [JsonProperty("notifications_from_email", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string NotificationsFromEmail { get; set; }

    /// <summary>
    /// Timezone configured for your page
    /// </summary>
    [JsonProperty("time_zone", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string TimeZone { get; set; }

    /// <summary>
    /// Allows you to customize the footer appearing on your notification emails.  Accepts Markdown for formatting
    /// </summary>
    [JsonProperty("notifications_email_footer", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string NotificationsEmailFooter { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  public enum ComponentStatus {
    [System.Runtime.Serialization.EnumMember(Value = @"operational")]
    Operational = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"under_maintenance")]
    UnderMaintenance = 1,

    [System.Runtime.Serialization.EnumMember(Value = @"degraded_performance")]
    DegradedPerformance = 2,

    [System.Runtime.Serialization.EnumMember(Value = @"partial_outage")]
    PartialOutage = 3,

    [System.Runtime.Serialization.EnumMember(Value = @"major_outage")]
    MajorOutage = 4,

    [System.Runtime.Serialization.EnumMember(Value = @"")]
    Empty = 5,
  }

  public enum PostPagesPageIdSubscribersUnsubscribeType {
    [System.Runtime.Serialization.EnumMember(Value = @"email")]
    Email = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"sms")]
    Sms = 1,

    [System.Runtime.Serialization.EnumMember(Value = @"slack")]
    Slack = 2,

    [System.Runtime.Serialization.EnumMember(Value = @"webhook")]
    Webhook = 3,

    [System.Runtime.Serialization.EnumMember(Value = @"teams")]
    Teams = 4,

    [System.Runtime.Serialization.EnumMember(Value = @"integration_partner")]
    IntegrationPartner = 5,
  }

  public enum PostPagesPageIdSubscribersUnsubscribeState {
    [System.Runtime.Serialization.EnumMember(Value = @"active")]
    Active = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"unconfirmed")]
    Unconfirmed = 1,

    [System.Runtime.Serialization.EnumMember(Value = @"quarantined")]
    Quarantined = 2,

    [System.Runtime.Serialization.EnumMember(Value = @"all")]
    All = 3,
  }

  public enum PostPagesPageIdSubscribersReactivateType {
    [System.Runtime.Serialization.EnumMember(Value = @"email")]
    Email = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"sms")]
    Sms = 1,

    [System.Runtime.Serialization.EnumMember(Value = @"slack")]
    Slack = 2,

    [System.Runtime.Serialization.EnumMember(Value = @"webhook")]
    Webhook = 3,

    [System.Runtime.Serialization.EnumMember(Value = @"integration_partner")]
    IntegrationPartner = 4,
  }

  public class Template {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Name of the template, as shown in the list on the "Templates" tab of the "Incidents" page
    /// </summary>
    [JsonProperty("name", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Name { get; set; }

    /// <summary>
    /// Title to be applied to the incident or maintenance when selecting this template
    /// </summary>
    [JsonProperty("title", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Title { get; set; }

    /// <summary>
    /// The initial message, created as the first incident or maintenance update.
    /// </summary>
    [JsonProperty("body", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Body { get; set; }

    /// <summary>
    /// Identifier of Template Group this template belongs to
    /// </summary>
    [JsonProperty("group_id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string GroupId { get; set; }

    /// <summary>
    /// The status the incident or maintenance should transition to when selecting this template
    /// </summary>
    [JsonProperty("update_status", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public TemplateUpdateStatus UpdateStatus { get; set; }

    /// <summary>
    /// Whether the "tweet update" checkbox should be selected when selecting this template
    /// </summary>
    [JsonProperty("should_tweet", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool ShouldTweet { get; set; }

    /// <summary>
    /// Whether the "deliver notifications" checkbox should be selected when selecting this template
    /// </summary>
    [JsonProperty("should_send_notifications", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool ShouldSendNotifications { get; set; }

    /// <summary>
    /// List of component_ids affected by this incident
    /// </summary>
    [JsonProperty("component_ids", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> ComponentIds { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  public enum IncidentTemplateUpdateStatus {
    [System.Runtime.Serialization.EnumMember(Value = @"investigating")]
    Investigating = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"identified")]
    Identified = 1,

    [System.Runtime.Serialization.EnumMember(Value = @"monitoring")]
    Monitoring = 2,

    [System.Runtime.Serialization.EnumMember(Value = @"resolved")]
    Resolved = 3,

    [System.Runtime.Serialization.EnumMember(Value = @"scheduled")]
    Scheduled = 4,

    [System.Runtime.Serialization.EnumMember(Value = @"in_progress")]
    InProgress = 5,

    [System.Runtime.Serialization.EnumMember(Value = @"verifying")]
    Verifying = 6,

    [System.Runtime.Serialization.EnumMember(Value = @"completed")]
    Completed = 7,
  }

  public enum IncidentImpactOverride {
    [System.Runtime.Serialization.EnumMember(Value = @"none")]
    None = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"maintenance")]
    Maintenance = 1,

    [System.Runtime.Serialization.EnumMember(Value = @"minor")]
    Minor = 2,

    [System.Runtime.Serialization.EnumMember(Value = @"major")]
    Major = 3,

    [System.Runtime.Serialization.EnumMember(Value = @"critical")]
    Critical = 4,
  }

  public enum IncidentStatus {
    [System.Runtime.Serialization.EnumMember(Value = @"investigating")]
    Investigating = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"identified")]
    Identified = 1,

    [System.Runtime.Serialization.EnumMember(Value = @"monitoring")]
    Monitoring = 2,

    [System.Runtime.Serialization.EnumMember(Value = @"resolved")]
    Resolved = 3,

    [System.Runtime.Serialization.EnumMember(Value = @"scheduled")]
    Scheduled = 4,

    [System.Runtime.Serialization.EnumMember(Value = @"in_progress")]
    InProgress = 5,

    [System.Runtime.Serialization.EnumMember(Value = @"verifying")]
    Verifying = 6,

    [System.Runtime.Serialization.EnumMember(Value = @"completed")]
    Completed = 7,
  }

  public enum IncidentUpdateStatus {
    [System.Runtime.Serialization.EnumMember(Value = @"investigating")]
    Investigating = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"identified")]
    Identified = 1,

    [System.Runtime.Serialization.EnumMember(Value = @"monitoring")]
    Monitoring = 2,

    [System.Runtime.Serialization.EnumMember(Value = @"resolved")]
    Resolved = 3,

    [System.Runtime.Serialization.EnumMember(Value = @"scheduled")]
    Scheduled = 4,

    [System.Runtime.Serialization.EnumMember(Value = @"in_progress")]
    InProgress = 5,

    [System.Runtime.Serialization.EnumMember(Value = @"verifying")]
    Verifying = 6,

    [System.Runtime.Serialization.EnumMember(Value = @"completed")]
    Completed = 7,
  }

  public class Subscriber3 {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// The email address for creating Email subscribers.
    /// </summary>
    [JsonProperty("email", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Email { get; set; }

    /// <summary>
    /// The two-character country where the phone number is located to use for the new SMS subscriber.
    /// </summary>
    [JsonProperty("phone_country", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string PhoneCountry { get; set; }

    /// <summary>
    /// The phone number (as you would dial from the phone_country) to use for the new SMS subscriber.
    /// </summary>
    [JsonProperty("phone_number", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string PhoneNumber { get; set; }

    /// <summary>
    /// If skip_confirmation_notification is true, the subscriber does not receive any notifications when their subscription changes. Email subscribers will be automatically opted in. This option is only available for paid pages. This option has no effect for trial customers.
    /// </summary>
    [JsonProperty("skip_confirmation_notification",
      Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool SkipConfirmationNotification { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  public class Postmortem2 {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Body of Postmortem to create.
    /// </summary>
    [JsonProperty("body_draft", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string BodyDraft { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  public class Postmortem3 {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Whether to notify Twitter followers
    /// </summary>
    [JsonProperty("notify_twitter", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool NotifyTwitter { get; set; }

    /// <summary>
    /// Whether to notify e-mail subscribers
    /// </summary>
    [JsonProperty("notify_subscribers", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool NotifySubscribers { get; set; }

    /// <summary>
    /// Custom postmortem tweet to publish
    /// </summary>
    [JsonProperty("custom_tweet", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string CustomTweet { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  public class RelatedEvents {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Incident identifier
    /// </summary>
    [JsonProperty("id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  public class ComponentGroup {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("components", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public ICollection<string> Components { get; set; } =
      new System.Collections.ObjectModel.Collection<string>();

    [JsonProperty("name", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Name { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  public class ComponentGroup2 {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("components", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public ICollection<string> Components { get; set; } =
      new System.Collections.ObjectModel.Collection<string>();

    [JsonProperty("name", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Name { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  public class ComponentGroup3 {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("components", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public ICollection<string> Components { get; set; } =
      new System.Collections.ObjectModel.Collection<string>();

    [JsonProperty("name", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Name { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  public class RelatedEvents2 {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Component identifier
    /// </summary>
    [JsonProperty("component_id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string ComponentId { get; set; }

    /// <summary>
    /// Related incidents
    /// </summary>
    [JsonProperty("incidents", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public Incidents Incidents { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  public class MetricId {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("timestamp", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public int Timestamp { get; set; }

    [JsonProperty("value", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public float Value { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  public class Metric2 {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Name of metric
    /// </summary>
    [JsonProperty("name", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    /// <summary>
    /// Metric Display identifier used to look up the metric data from the provider
    /// </summary>
    [JsonProperty("metric_identifier", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string MetricIdentifier { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  public class Metric3 {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Name of metric
    /// </summary>
    [JsonProperty("name", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    /// <summary>
    /// Metric Display identifier used to look up the metric data from the provider
    /// </summary>
    [JsonProperty("metric_identifier", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string MetricIdentifier { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  public class Data {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Time to store the metric against
    /// </summary>
    [JsonProperty("timestamp", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public int Timestamp { get; set; }

    [JsonProperty("value", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public float Value { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  public class Data2 {
    private IDictionary<string, object> additionalProperties;

    [JsonProperty("timestamp", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public int Timestamp { get; set; }

    [JsonProperty("value", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public float Value { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  public class Metric4 {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Name of metric
    /// </summary>
    [JsonProperty("name", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    /// <summary>
    /// The identifier used to look up the metric data from the provider
    /// </summary>
    [JsonProperty("metric_identifier", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string MetricIdentifier { get; set; }

    /// <summary>
    /// The transform to apply to metric before pulling into Statuspage. One of: "average", "count", "max", "min", or "sum"
    /// </summary>
    [JsonProperty("transform", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Transform { get; set; }

    /// <summary>
    /// The Identifier for new relic application. Required in the case of NewRelic only
    /// </summary>
    [JsonProperty("application_id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string ApplicationId { get; set; }

    /// <summary>
    /// Suffix to describe the units on the graph
    /// </summary>
    [JsonProperty("suffix", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Suffix { get; set; }

    /// <summary>
    /// The lower bound of the y axis
    /// </summary>
    [JsonProperty("y_axis_min", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public int YAxisMin { get; set; }

    /// <summary>
    /// The upper bound of the y axis
    /// </summary>
    [JsonProperty("y_axis_max", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public int YAxisMax { get; set; }

    /// <summary>
    /// Should the values on the y axis be hidden on render
    /// </summary>
    [JsonProperty("y_axis_hidden", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool YAxisHidden { get; set; }

    /// <summary>
    /// Should the metric be displayed
    /// </summary>
    [JsonProperty("display", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool Display { get; set; }

    /// <summary>
    /// How many decimal places to render on the graph
    /// </summary>
    [JsonProperty("decimal_places", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public int DecimalPlaces { get; set; }

    [JsonProperty("tooltip_description", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string TooltipDescription { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  public class Pages {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Page identifier
    /// </summary>
    [JsonProperty("page_id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string PageId { get; set; }

    /// <summary>
    /// User has page configuration role. This field will only be present if the organization has Role Based Access Control.
    /// </summary>
    [JsonProperty("page_configuration", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool PageConfiguration { get; set; }

    /// <summary>
    /// User has incident manager role. This field will only be present if the organization has Role Based Access Control.
    /// </summary>
    [JsonProperty("incident_manager", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool IncidentManager { get; set; }

    /// <summary>
    /// User has maintenance manager role. This field will only be present if the organization has Role Based Access Control.
    /// </summary>
    [JsonProperty("maintenance_manager", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool MaintenanceManager { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  public class Data3 {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// User identifier
    /// </summary>
    [JsonProperty("user_id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string UserId { get; set; }

    /// <summary>
    /// Pages accessible by the user.
    /// </summary>
    [JsonProperty("pages", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public Pages Pages { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  public enum Page2Branding {
    [System.Runtime.Serialization.EnumMember(Value = @"basic")]
    Basic = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"premium")]
    Premium = 1,
  }

  public enum Page3Branding {
    [System.Runtime.Serialization.EnumMember(Value = @"basic")]
    Basic = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"premium")]
    Premium = 1,
  }

  public enum TemplateUpdateStatus {
    [System.Runtime.Serialization.EnumMember(Value = @"investigating")]
    Investigating = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"identified")]
    Identified = 1,

    [System.Runtime.Serialization.EnumMember(Value = @"monitoring")]
    Monitoring = 2,

    [System.Runtime.Serialization.EnumMember(Value = @"resolved")]
    Resolved = 3,

    [System.Runtime.Serialization.EnumMember(Value = @"scheduled")]
    Scheduled = 4,

    [System.Runtime.Serialization.EnumMember(Value = @"in_progress")]
    InProgress = 5,

    [System.Runtime.Serialization.EnumMember(Value = @"verifying")]
    Verifying = 6,

    [System.Runtime.Serialization.EnumMember(Value = @"completed")]
    Completed = 7,
  }

  public class Incidents {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Incident identifier
    /// </summary>
    [JsonProperty("id", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  public class PageId {
    private IDictionary<string, object> additionalProperties;

    /// <summary>
    /// Whether or not user should have page configuration role. This field will only be present for pages with Role Based Access Control.
    /// </summary>
    [JsonProperty("page_configuration", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool PageConfiguration { get; set; }

    /// <summary>
    /// Whether or not user should have incident manager role. This field will only be present for pages with Role Based Access Control.
    /// </summary>
    [JsonProperty("incident_manager", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool IncidentManager { get; set; }

    /// <summary>
    /// Whether or not user should have maintenance manager role. This field will only be present for pages with Role Based Access Control.
    /// </summary>
    [JsonProperty("maintenance_manager", Required = Required.DisallowNull,
      NullValueHandling = NullValueHandling.Ignore)]
    public bool MaintenanceManager { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return additionalProperties ??
               (additionalProperties = new Dictionary<string, object>());
      }
      set { additionalProperties = value; }
    }
  }

  internal class DateFormatConverter : Newtonsoft.Json.Converters.IsoDateTimeConverter {
    public DateFormatConverter() {
      DateTimeFormat = "yyyy-MM-dd";
    }
  }

  public class ApiException : Exception {
    public ApiException(string message, int statusCode, string response,
      IReadOnlyDictionary<string, IEnumerable<string>>
        headers, Exception innerException)
      : base(
        message + "\n\nStatus: " + statusCode + "\nResponse: \n" + ((response == null)
          ? "(null)"
          : response.Substring(0, response.Length >= 512 ? 512 : response.Length)), innerException) {
      StatusCode = statusCode;
      Response = response;
      Headers = headers;
    }

    public int StatusCode { get; private set; }

    public string Response { get; private set; }

    public IReadOnlyDictionary<string, IEnumerable<string>>
      Headers { get; private set; }

    public override string ToString() {
      return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
    }
  }

  public class ApiException<TResult> : ApiException {
    public ApiException(string message, int statusCode, string response,
      IReadOnlyDictionary<string, IEnumerable<string>>
        headers, TResult result, Exception innerException)
      : base(message, statusCode, response, headers, innerException) {
      Result = result;
    }

    public TResult Result { get; private set; }
  }
}