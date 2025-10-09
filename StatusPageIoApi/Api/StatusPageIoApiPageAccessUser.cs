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
    /// Add a page access user
    /// </summary>
    /// <remarks>
    /// Add a page access user
    /// </remarks>
    /// <param name="pageId">Page identifier</param>
    /// <param name="body"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Add a page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessUser> PostPageAccessUsers(string pageId, PostPagesPageIdPageAccessUsers body,
      CancellationToken cancellationToken = default(CancellationToken)) {
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
    /// Get a list of page access users
    /// </summary>
    /// <remarks>
    /// Get a list of page access users
    /// </remarks>
    /// <param name="pageId">Page identifier</param>
    /// <param name="email">Email address to search for</param>
    /// <param name="page">Page offset to fetch. Beginning February 28, 2023, this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <param name="perPage">Number of results to return per page. Beginning February 28, 2023, a default and maximum limit of 100 will be imposed and this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <returns>Get a list of page access users</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessUser[]>
      GetPageAccessUsers(string pageId, string email, int? page, int? perPage,
        CancellationToken cancellationToken = default(CancellationToken)) {
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

        var url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        var response = await httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead, cancellationToken)
          .ConfigureAwait(false);
        var disposeResponse = true;
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          foreach (var item in response.Content.Headers)
            headers[item.Key] = item.Value;

          var status = (int) response.StatusCode;
          if (status == 200) {
            var objectResponse =
              await ReadObjectResponseAsync<PageAccessUser[]>(
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
    /// <param name="pageId">Page identifier</param>
    /// <param name="pageAccessUserId">Page Access User Identifier</param>
    /// <returns>Update page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessUser>
      PatchPageAccessUser(string pageId, string pageAccessUserId,
        CancellationToken cancellationToken = default(CancellationToken)) {
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
            var objectResponse = await ReadObjectResponseAsync<PageAccessUser>(response, headers, cancellationToken)
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
          else if (status == 403) {
            var objectResponse = await ReadObjectResponseAsync<ErrorEntity>(response, headers, cancellationToken)
              .ConfigureAwait(false);
            if (objectResponse.Object == null)
              throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers,
                null);
            throw new ApiException<ErrorEntity>("You are not authorized to access this resource.", status,
              objectResponse.Text, headers, objectResponse.Object, null);
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

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Update page access user
    /// </summary>
    /// <remarks>
    /// Update page access user
    /// </remarks>
    /// <param name="pageId">Page identifier</param>
    /// <param name="pageAccessUserId">Page Access User Identifier</param>
    /// <returns>Update page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessUser> PutPageAccessUser(string pageId, string pageAccessUserId,
      CancellationToken cancellationToken = default(CancellationToken)) {
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
    /// <param name="pageId">Page identifier</param>
    /// <param name="pageAccessUserId">Page Access User Identifier</param>
    /// <returns>Delete page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task DeletePageAccessUser(
      string pageId, string pageAccessUserId, CancellationToken cancellationToken = default(CancellationToken)) {
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
        try {
          var headers =
            response.Headers.ToDictionary(h => h.Key, h => h.Value);
          if (response.Content?.Headers != null) {
            foreach (var item in response.Content.Headers)
              headers[item.Key] = item.Value;
          }

          var status = (int) response.StatusCode;
          if (status == 204) {
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
    /// <param name="pageId">Page identifier</param>
    /// <param name="pageAccessUserId">Page Access User Identifier</param>
    /// <returns>Get page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessUser> GetPageAccessUser(string pageId, string pageAccessUserId,
      CancellationToken cancellationToken = default(CancellationToken)) {
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
    /// <param name="pageId">Page identifier</param>
    /// <param name="pageAccessUserId">Page Access User Identifier</param>
    /// <param name="page">Page offset to fetch. Beginning February 28, 2023, this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <param name="perPage">Number of results to return per page. Beginning February 28, 2023, a default and maximum limit of 100 will be imposed and this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <returns>Get components for page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Component[]> GetPageAccessUserComponents(string pageId, string pageAccessUserId,
      int? page = null, int? perPage = null, CancellationToken cancellationToken = default(CancellationToken)) {
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
              await ReadObjectResponseAsync<Component[]>(
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

    /// <summary>
    /// Add components for page access user
    /// </summary>
    /// <remarks>
    /// Add components for page access user
    /// </remarks>
    /// <param name="pageId">Page identifier</param>
    /// <param name="pageAccessUserId">Page Access User Identifier</param>
    /// <param name="body"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Add components for page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessUser> PatchPageAccessUserComponents(string pageId, string pageAccessUserId,
      PatchPagesPageIdPageAccessUsersPageAccessUserIdComponents body,
      CancellationToken cancellationToken = default(CancellationToken)) {
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
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Add components for page access user
    /// </summary>
    /// <remarks>
    /// Add components for page access user
    /// </remarks>
    /// <param name="pageId">Page identifier</param>
    /// <param name="pageAccessUserId">Page Access User Identifier</param>
    /// <param name="body"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Add components for page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessUser> PutPageAccessUserComponents(string pageId, string pageAccessUserId,
      PutPagesPageIdPageAccessUsersPageAccessUserIdComponents body,
      CancellationToken cancellationToken = default(CancellationToken)) {
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
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Replace components for page access user
    /// </summary>
    /// <remarks>
    /// Replace components for page access user
    /// </remarks>
    /// <param name="pageId">Page identifier</param>
    /// <param name="pageAccessUserId">Page Access User Identifier</param>
    /// <param name="body"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Replace components for page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessUser>
      PostPageAccessUserComponents(string pageId, string pageAccessUserId,
        PostPagesPageIdPageAccessUsersPageAccessUserIdComponents body,
        CancellationToken cancellationToken = default(CancellationToken)) {
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
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Remove components for page access user
    /// </summary>
    /// <remarks>
    /// Remove components for page access user
    /// </remarks>
    /// <param name="pageId">Page identifier</param>
    /// <param name="pageAccessUserId">Page Access User Identifier</param>
    /// <param name="body"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Remove components for page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessUser> DeletePageAccessUserComponents(string pageId, string pageAccessUserId,
      DeletePagesPageIdPageAccessUsersPageAccessUserIdComponents body,
      CancellationToken cancellationToken = default(CancellationToken)) {
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
    /// <param name="pageId">Page identifier</param>
    /// <param name="pageAccessUserId">Page Access User Identifier</param>
    /// <param name="componentId">Component identifier</param>
    /// <returns>Remove component for page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessUser> DeletePageAccessUserComponent(string pageId, string pageAccessUserId,
      string componentId, CancellationToken cancellationToken = default(CancellationToken)) {
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
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Get metrics for page access user
    /// </summary>
    /// <remarks>
    /// Get metrics for page access user
    /// </remarks>
    /// <param name="pageId">Page identifier</param>
    /// <param name="pageAccessUserId">Page Access User Identifier</param>
    /// <param name="page">Page offset to fetch. Beginning February 28, 2023, this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <param name="perPage">Number of results to return per page. Beginning February 28, 2023, a default and maximum limit of 100 will be imposed and this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Get metrics for page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Metric[]> GetPageAccessUserMetrics(string pageId, string pageAccessUserId,
      int? page = null, int? perPage = null, CancellationToken cancellationToken = default(CancellationToken)) {
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
              await ReadObjectResponseAsync<Metric[]>(response,
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

    /// <summary>
    /// Add metrics for page access user
    /// </summary>
    /// <remarks>
    /// Add metrics for page access user
    /// </remarks>
    /// <param name="pageId">Page identifier</param>
    /// <param name="pageAccessUserId">Page Access User Identifier</param>
    /// <param name="body"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Add metrics for page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessUser> PatchPageAccessUserMetrics(string pageId, string pageAccessUserId,
      PatchPagesPageIdPageAccessUsersPageAccessUserIdMetrics body,
      CancellationToken cancellationToken = default(CancellationToken)) {
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
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Add metrics for page access user
    /// </summary>
    /// <remarks>
    /// Add metrics for page access user
    /// </remarks>
    /// <param name="pageId">Page identifier</param>
    /// <param name="pageAccessUserId">Page Access User Identifier</param>
    /// <param name="body"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Add metrics for page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessUser> PutPageAccessUserMetrics(string pageId, string pageAccessUserId,
      PutPagesPageIdPageAccessUsersPageAccessUserIdMetrics body,
      CancellationToken cancellationToken = default(CancellationToken)) {
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
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Replace metrics for page access user
    /// </summary>
    /// <remarks>
    /// Replace metrics for page access user
    /// </remarks>
    /// <param name="pageId">Page identifier</param>
    /// <param name="pageAccessUserId">Page Access User Identifier</param>
    /// <param name="body"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Replace metrics for page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessUser> PostPageAccessUserMetrics(string pageId, string pageAccessUserId,
      PostPagesPageIdPageAccessUsersPageAccessUserIdMetrics body,
      CancellationToken cancellationToken = default(CancellationToken)) {
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
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Delete metrics for page access user
    /// </summary>
    /// <remarks>
    /// Delete metrics for page access user
    /// </remarks>
    /// <param name="pageId">Page identifier</param>
    /// <param name="pageAccessUserId">Page Access User Identifier</param>
    /// <param name="body"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Delete metrics for page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessUser> DeletePageAccessUserMetrics(string pageId, string pageAccessUserId,
      DeletePagesPageIdPageAccessUsersPageAccessUserIdMetrics body,
      CancellationToken cancellationToken = default(CancellationToken)) {
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
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Delete metric for page access user
    /// </summary>
    /// <remarks>
    /// Delete metric for page access user
    /// </remarks>
    /// <param name="pageId">Page identifier</param>
    /// <param name="pageAccessUserId">Page Access User Identifier</param>
    /// <param name="metricId">Identifier of metric requested</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Delete metric for page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessUser> DeletePageAccessUserMetric(string pageId, string pageAccessUserId,
      string metricId, CancellationToken cancellationToken = default(CancellationToken)) {
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
          response.Dispose();
        }
      }
    }
  }
}