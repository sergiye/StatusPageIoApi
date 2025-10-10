using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StatusPageIoApi {

  public partial class StatusPageIoApi {

    /// <summary>
    /// Update a user's role permissions
    /// </summary>
    /// <remarks>
    /// Update a user's role permissions. Payload should contain a mapping of pages to a set of the desired roles,
    /// <br/>                  if the page has Role Based Access Control. Otherwise, the pages should map to an empty hash.
    /// <br/>                  User will lose access to any pages omitted from the payload.
    /// </remarks>
    /// <param name="organizationId">Organization Identifier</param>
    /// <param name="userId">User identifier</param>
    /// <param name="body"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Update a user's role permissions. Payload should contain a mapping of pages to a set of the desired roles,
    /// <br/>                  if the page has Role Based Access Control. Otherwise, the pages should map to an empty hash.
    /// <br/>                  User will lose access to any pages omitted from the payload.</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Permissions> PutOrganizationPermissionsUser(string organizationId, string userId,
      PutOrganizationsOrganizationIdPermissions body,
      CancellationToken cancellationToken = default(CancellationToken)) {
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

      using (var request = new HttpRequestMessage()) {
        var json = JsonConvert.SerializeObject(body, jsonSettings);
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
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Get a user's permissions
    /// </summary>
    /// <param name="organizationId">Organization Identifier</param>
    /// <param name="userId">User identifier</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Get a user's permissions</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Permissions> GetOrganizationPermissionsUser(string organizationId, string userId,
      CancellationToken cancellationToken = default(CancellationToken)) {
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
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Delete a user
    /// </summary>
    /// <param name="organizationId">Organization Identifier</param>
    /// <param name="userId">User Identifier</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Delete a user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<User> DeleteOrganizationUser(string organizationId, string userId, CancellationToken cancellationToken = default(CancellationToken)) {
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
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Create a user
    /// </summary>
    /// <param name="organizationId">Organization Identifier</param>
    /// <param name="body"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Create a user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<User> PostOrganizationUsers(string organizationId, PostOrganizationUser body, CancellationToken cancellationToken = default(CancellationToken)) {
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

      using (var request = new HttpRequestMessage()) {
        var json = JsonConvert.SerializeObject(body, jsonSettings);
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
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Get a list of users
    /// </summary>
    /// <param name="organizationId">Organization Identifier</param>
    /// <param name="page">Page offset to fetch. Beginning February 28, 2023, this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <param name="perPage">Number of results to return per page. Beginning February 28, 2023, a default and maximum limit of 100 will be imposed and this endpoint will return paginated data even if this query parameter is not provided.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Get a list of users</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<User[]> GetOrganizationUsers(string organizationId, int? page = null, int? perPage = null,
      CancellationToken cancellationToken = default(CancellationToken)) {
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
                await ReadObjectResponseAsync<User[]>(response,
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
  }
}