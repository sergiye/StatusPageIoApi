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

    /// <summary>
    /// Get a list of pages
    /// </summary>
    /// <remarks>
    /// Get a list of pages
    /// </remarks>
    /// <returns>Get a list of pages</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual Task<ICollection<Page>> GetPages() {
      return GetPages(CancellationToken.None);
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
      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/pages");

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ = Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            foreach (var item_ in response_.Content.Headers)
              headers_[item_.Key] = item_.Value;

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ICollection<Page>>(response_,
                      headers_, cancellationToken).ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

    /// <summary>
    /// Update a page
    /// </summary>
    /// <remarks>
    /// Update a page
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Update a page</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual Task<Page> PatchPages(string page_id, PatchPages body) {
      return PatchPages(page_id, body, CancellationToken.None);
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
    public virtual async Task<Page> PatchPages(string page_id, PatchPages body,
        CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/pages/{page_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("PATCH");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Page>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 400) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Bad request", status_, objectResponse_.Text, headers_,
                  objectResponse_.Object, null);
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

    /// <summary>
    /// Update a page
    /// </summary>
    /// <remarks>
    /// Update a page
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Update a page</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual Task<Page> PutPages(string page_id, PutPages body) {
      return PutPages(page_id, body, CancellationToken.None);
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
    public virtual async Task<Page> PutPages(string page_id, PutPages body,
        CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/pages/{page_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("PUT");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Page>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 400) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Bad request", status_, objectResponse_.Text, headers_,
                  objectResponse_.Object, null);
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

    /// <summary>
    /// Get a page
    /// </summary>
    /// <remarks>
    /// Get a page
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Get a page</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual Task<Page> GetPages(string page_id) {
      return GetPages(page_id, CancellationToken.None);
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
    public virtual async Task<Page> GetPages(string page_id,
        CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/pages/{page_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Page>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 400) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Bad request", status_, objectResponse_.Text, headers_,
                  objectResponse_.Object, null);
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

    /// <summary>
    /// Add a page access user
    /// </summary>
    /// <remarks>
    /// Add a page access user
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Add a page access user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual Task<PageAccessUser> PostPageAccessUsers(string page_id,
        PostPagesPageIdPageAccessUsers body) {
      return PostPageAccessUsers(page_id, body, CancellationToken.None);
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
        string page_id, PostPagesPageIdPageAccessUsers body, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/page_access_users");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("POST");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<PageAccessUser>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 400) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Bad request", status_, objectResponse_.Text, headers_,
                  objectResponse_.Object, null);
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 409) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>(
                  "The request could not be processed due to a conflict in resource state.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<ICollection<PageAccessUser>>
        GetPageAccessUsers(string page_id, string email, int? page, int? per_page) {
      return GetPageAccessUsers(page_id, email, page, per_page,
          CancellationToken.None);
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
        GetPageAccessUsers(string page_id, string email, int? page, int? per_page,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/page_access_users?");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      if (email != null) {
        urlBuilder_.Append(Uri.EscapeDataString("email") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(email,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (per_page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("per_page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(per_page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder_.Length--;

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ICollection<PageAccessUser>>(
                      response_, headers_, cancellationToken).ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<PageAccessUser> PatchPageAccessUser(
        string page_id, string page_access_user_id) {
      return PatchPageAccessUser(page_id, page_access_user_id,
          CancellationToken.None);
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
        PatchPageAccessUser(string page_id, string page_access_user_id,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (page_access_user_id == null)
        throw new ArgumentNullException("page_access_user_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/page_access_users/{page_access_user_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{page_access_user_id}",
          Uri.EscapeDataString(ConvertToString(page_access_user_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Content =
              new StringContent(string.Empty, Encoding.UTF8, "application/json");
          request_.Method = new HttpMethod("PATCH");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<PageAccessUser>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<PageAccessUser> PutPageAccessUser(
        string page_id, string page_access_user_id) {
      return PutPageAccessUser(page_id, page_access_user_id,
          CancellationToken.None);
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
        PutPageAccessUser(string page_id, string page_access_user_id,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (page_access_user_id == null)
        throw new ArgumentNullException("page_access_user_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/page_access_users/{page_access_user_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{page_access_user_id}",
          Uri.EscapeDataString(ConvertToString(page_access_user_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Content =
              new StringContent(string.Empty, Encoding.UTF8, "application/json");
          request_.Method = new HttpMethod("PUT");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<PageAccessUser>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task DeletePageAccessUser(string page_id,
        string page_access_user_id) {
      return DeletePageAccessUser(page_id, page_access_user_id,
          CancellationToken.None);
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
        string page_id, string page_access_user_id, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (page_access_user_id == null)
        throw new ArgumentNullException("page_access_user_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/page_access_users/{page_access_user_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{page_access_user_id}",
          Uri.EscapeDataString(ConvertToString(page_access_user_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("DELETE");

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 204) {
              return;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<PageAccessUser> GetPageAccessUser(
        string page_id, string page_access_user_id) {
      return GetPageAccessUser(page_id, page_access_user_id,
          CancellationToken.None);
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
        GetPageAccessUser(string page_id, string page_access_user_id,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (page_access_user_id == null)
        throw new ArgumentNullException("page_access_user_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/page_access_users/{page_access_user_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{page_access_user_id}",
          Uri.EscapeDataString(ConvertToString(page_access_user_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<PageAccessUser>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<PageAccessUser>
        PatchPageAccessUserComponents(string page_id, string page_access_user_id,
            PatchPagesPageIdPageAccessUsersPageAccessUserIdComponents body) {
      return PatchPageAccessUserComponents(page_id, page_access_user_id, body,
          CancellationToken.None);
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
        PatchPageAccessUserComponents(string page_id, string page_access_user_id,
            PatchPagesPageIdPageAccessUsersPageAccessUserIdComponents body,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (page_access_user_id == null)
        throw new ArgumentNullException("page_access_user_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/page_access_users/{page_access_user_id}/components");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{page_access_user_id}",
          Uri.EscapeDataString(ConvertToString(page_access_user_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("PATCH");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<PageAccessUser>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<PageAccessUser>
        PutPageAccessUserComponents(string page_id, string page_access_user_id,
            PutPagesPageIdPageAccessUsersPageAccessUserIdComponents body) {
      return PutPageAccessUserComponents(page_id, page_access_user_id, body,
          CancellationToken.None);
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
        PutPageAccessUserComponents(string page_id, string page_access_user_id,
            PutPagesPageIdPageAccessUsersPageAccessUserIdComponents body,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (page_access_user_id == null)
        throw new ArgumentNullException("page_access_user_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/page_access_users/{page_access_user_id}/components");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{page_access_user_id}",
          Uri.EscapeDataString(ConvertToString(page_access_user_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("PUT");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<PageAccessUser>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<PageAccessUser>
        PostPageAccessUserComponents(string page_id, string page_access_user_id,
            PostPagesPageIdPageAccessUsersPageAccessUserIdComponents body) {
      return PostPageAccessUserComponents(page_id, page_access_user_id, body,
          CancellationToken.None);
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
        PostPageAccessUserComponents(string page_id, string page_access_user_id,
            PostPagesPageIdPageAccessUsersPageAccessUserIdComponents body,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (page_access_user_id == null)
        throw new ArgumentNullException("page_access_user_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/page_access_users/{page_access_user_id}/components");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{page_access_user_id}",
          Uri.EscapeDataString(ConvertToString(page_access_user_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("POST");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 201) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<PageAccessUser>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<PageAccessUser>
        DeletePageAccessUserComponents(string page_id, string page_access_user_id,
            DeletePagesPageIdPageAccessUsersPageAccessUserIdComponents body) {
      return DeletePageAccessUserComponents(page_id, page_access_user_id, body,
          CancellationToken.None);
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
        DeletePageAccessUserComponents(string page_id, string page_access_user_id,
            DeletePagesPageIdPageAccessUsersPageAccessUserIdComponents body,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (page_access_user_id == null)
        throw new ArgumentNullException("page_access_user_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/page_access_users/{page_access_user_id}/components");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{page_access_user_id}",
          Uri.EscapeDataString(ConvertToString(page_access_user_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("DELETE");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<PageAccessUser>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<ICollection<Component>>
        GetPageAccessUserComponents(string page_id, string page_access_user_id,
            int? page, int? per_page) {
      return GetPageAccessUserComponents(page_id, page_access_user_id, page,
          per_page, CancellationToken.None);
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
        GetPageAccessUserComponents(string page_id, string page_access_user_id,
            int? page, int? per_page, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (page_access_user_id == null)
        throw new ArgumentNullException("page_access_user_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/page_access_users/{page_access_user_id}/components?");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{page_access_user_id}",
          Uri.EscapeDataString(ConvertToString(page_access_user_id,
              CultureInfo.InvariantCulture)));
      if (page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (per_page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("per_page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(per_page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder_.Length--;

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ICollection<Component>>(
                      response_, headers_, cancellationToken).ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<PageAccessUser>
        DeletePageAccessUserComponent(string page_id,
            string page_access_user_id, string component_id) {
      return DeletePageAccessUserComponent(page_id,
          page_access_user_id, component_id, CancellationToken.None);
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
        DeletePageAccessUserComponent(string page_id,
            string page_access_user_id, string component_id, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (page_access_user_id == null)
        throw new ArgumentNullException("page_access_user_id");

      if (component_id == null)
        throw new ArgumentNullException("component_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/page_access_users/{page_access_user_id}/components/{component_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{page_access_user_id}",
          Uri.EscapeDataString(ConvertToString(page_access_user_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{component_id}",
          Uri.EscapeDataString(ConvertToString(component_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("DELETE");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<PageAccessUser>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<PageAccessUser>
        PatchPageAccessUserMetrics(string page_id, string page_access_user_id,
            PatchPagesPageIdPageAccessUsersPageAccessUserIdMetrics body) {
      return PatchPageAccessUserMetrics(page_id, page_access_user_id, body,
          CancellationToken.None);
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
        PatchPageAccessUserMetrics(string page_id, string page_access_user_id,
            PatchPagesPageIdPageAccessUsersPageAccessUserIdMetrics body,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (page_access_user_id == null)
        throw new ArgumentNullException("page_access_user_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/page_access_users/{page_access_user_id}/metrics");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{page_access_user_id}",
          Uri.EscapeDataString(ConvertToString(page_access_user_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("PATCH");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<PageAccessUser>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<PageAccessUser>
        PutPageAccessUserMetrics(string page_id, string page_access_user_id,
            PutPagesPageIdPageAccessUsersPageAccessUserIdMetrics body) {
      return PutPageAccessUserMetrics(page_id, page_access_user_id, body,
          CancellationToken.None);
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
        PutPageAccessUserMetrics(string page_id, string page_access_user_id,
            PutPagesPageIdPageAccessUsersPageAccessUserIdMetrics body,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (page_access_user_id == null)
        throw new ArgumentNullException("page_access_user_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/page_access_users/{page_access_user_id}/metrics");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{page_access_user_id}",
          Uri.EscapeDataString(ConvertToString(page_access_user_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("PUT");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<PageAccessUser>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<PageAccessUser>
        PostPageAccessUserMetrics(string page_id, string page_access_user_id,
            PostPagesPageIdPageAccessUsersPageAccessUserIdMetrics body) {
      return PostPageAccessUserMetrics(page_id, page_access_user_id, body,
          CancellationToken.None);
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
        PostPageAccessUserMetrics(string page_id, string page_access_user_id,
            PostPagesPageIdPageAccessUsersPageAccessUserIdMetrics body,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (page_access_user_id == null)
        throw new ArgumentNullException("page_access_user_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/page_access_users/{page_access_user_id}/metrics");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{page_access_user_id}",
          Uri.EscapeDataString(ConvertToString(page_access_user_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("POST");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 201) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<PageAccessUser>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<PageAccessUser>
        DeletePageAccessUserMetrics(string page_id, string page_access_user_id,
            DeletePagesPageIdPageAccessUsersPageAccessUserIdMetrics body) {
      return DeletePageAccessUserMetrics(page_id, page_access_user_id, body,
          CancellationToken.None);
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
        DeletePageAccessUserMetrics(string page_id, string page_access_user_id,
            DeletePagesPageIdPageAccessUsersPageAccessUserIdMetrics body,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (page_access_user_id == null)
        throw new ArgumentNullException("page_access_user_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/page_access_users/{page_access_user_id}/metrics");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{page_access_user_id}",
          Uri.EscapeDataString(ConvertToString(page_access_user_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("DELETE");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<PageAccessUser>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<ICollection<Metric>>
        GetPageAccessUserMetrics(string page_id, string page_access_user_id,
            int? page,
            int? per_page) {
      return GetPageAccessUserMetrics(page_id, page_access_user_id, page,
          per_page, CancellationToken.None);
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
        GetPageAccessUserMetrics(string page_id, string page_access_user_id,
            int? page,
            int? per_page, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (page_access_user_id == null)
        throw new ArgumentNullException("page_access_user_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/page_access_users/{page_access_user_id}/metrics?");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{page_access_user_id}",
          Uri.EscapeDataString(ConvertToString(page_access_user_id,
              CultureInfo.InvariantCulture)));
      if (page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (per_page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("per_page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(per_page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder_.Length--;

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ICollection<Metric>>(response_,
                      headers_, cancellationToken).ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<PageAccessUser>
        DeletePageAccessUserMetric(string page_id,
            string page_access_user_id,
            string metric_id) {
      return DeletePageAccessUserMetric(page_id, page_access_user_id,
          metric_id, CancellationToken.None);
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
        DeletePageAccessUserMetric(string page_id,
            string page_access_user_id,
            string metric_id, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (page_access_user_id == null)
        throw new ArgumentNullException("page_access_user_id");

      if (metric_id == null)
        throw new ArgumentNullException("metric_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/page_access_users/{page_access_user_id}/metrics/{metric_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{page_access_user_id}",
          Uri.EscapeDataString(ConvertToString(page_access_user_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{metric_id}",
          Uri.EscapeDataString(ConvertToString(metric_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("DELETE");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<PageAccessUser>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<ICollection<PageAccessGroup>>
        GetPageAccessGroups(string page_id, int? page, int? per_page) {
      return GetPageAccessGroups(page_id, page, per_page,
          CancellationToken.None);
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
        GetPageAccessGroups(string page_id, int? page, int? per_page,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/page_access_groups?");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      if (page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (per_page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("per_page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(per_page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder_.Length--;

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ICollection<PageAccessGroup>>(
                      response_, headers_, cancellationToken).ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

    /// <summary>
    /// Create a page access group
    /// </summary>
    /// <remarks>
    /// Create a page access group
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Create a page access group</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual Task<PageAccessGroup> PostPageAccessGroups(string page_id,
        PostPagesPageIdPageAccessGroups body) {
      return PostPageAccessGroups(page_id, body, CancellationToken.None);
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
    public virtual async Task<PageAccessGroup> PostPageAccessGroups(
        string page_id, PostPagesPageIdPageAccessGroups body,
        CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/page_access_groups");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("POST");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 201) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<PageAccessGroup>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 400) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Bad request", status_, objectResponse_.Text, headers_,
                  objectResponse_.Object, null);
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<PageAccessGroup>
        GetPageAccessGroup(string page_id, string page_access_group_id) {
      return GetPageAccessGroup(page_id, page_access_group_id,
          CancellationToken.None);
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
    public virtual async Task<PageAccessGroup>
        GetPageAccessGroup(string page_id, string page_access_group_id,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (page_access_group_id == null)
        throw new ArgumentNullException("page_access_group_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/page_access_groups/{page_access_group_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{page_access_group_id}",
          Uri.EscapeDataString(ConvertToString(page_access_group_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<PageAccessGroup>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<PageAccessGroup>
        PatchPageAccessGroup(string page_id, string page_access_group_id,
            PatchPagesPageIdPageAccessGroups body) {
      return PatchPageAccessGroup(page_id, page_access_group_id, body,
          CancellationToken.None);
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
        PatchPageAccessGroup(string page_id, string page_access_group_id,
            PatchPagesPageIdPageAccessGroups body, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (page_access_group_id == null)
        throw new ArgumentNullException("page_access_group_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/page_access_groups/{page_access_group_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{page_access_group_id}",
          Uri.EscapeDataString(ConvertToString(page_access_group_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("PATCH");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<PageAccessGroup>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<PageAccessGroup>
        PutPageAccessGroup(string page_id, string page_access_group_id,
            PutPagesPageIdPageAccessGroups body) {
      return PutPageAccessGroup(page_id, page_access_group_id, body,
          CancellationToken.None);
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
        PutPageAccessGroup(string page_id, string page_access_group_id,
            PutPagesPageIdPageAccessGroups body, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (page_access_group_id == null)
        throw new ArgumentNullException("page_access_group_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/page_access_groups/{page_access_group_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{page_access_group_id}",
          Uri.EscapeDataString(ConvertToString(page_access_group_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("PUT");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<PageAccessGroup>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<PageAccessGroup>
        DeletePageAccessGroup(string page_id, string page_access_group_id) {
      return DeletePageAccessGroup(page_id, page_access_group_id,
          CancellationToken.None);
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
    public virtual async Task<PageAccessGroup>
        DeletePageAccessGroup(string page_id, string page_access_group_id,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (page_access_group_id == null)
        throw new ArgumentNullException("page_access_group_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/page_access_groups/{page_access_group_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{page_access_group_id}",
          Uri.EscapeDataString(ConvertToString(page_access_group_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("DELETE");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<PageAccessGroup>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<PageAccessGroup>
        PatchPageAccessGroupComponents(string page_id,
            string page_access_group_id,
            PatchPagesPageIdPageAccessGroupsPageAccessGroupIdComponents body) {
      return PatchPageAccessGroupComponents(page_id, page_access_group_id, body,
          CancellationToken.None);
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
    public virtual async Task<PageAccessGroup>
        PatchPageAccessGroupComponents(string page_id,
            string page_access_group_id,
            PatchPagesPageIdPageAccessGroupsPageAccessGroupIdComponents body,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (page_access_group_id == null)
        throw new ArgumentNullException("page_access_group_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/page_access_groups/{page_access_group_id}/components");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{page_access_group_id}",
          Uri.EscapeDataString(ConvertToString(page_access_group_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("PATCH");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<PageAccessGroup>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<PageAccessGroup>
        PutPageAccessGroupComponents(string page_id, string page_access_group_id,
            PutPagesPageIdPageAccessGroupsPageAccessGroupIdComponents body) {
      return PutPageAccessGroupComponents(page_id, page_access_group_id, body,
          CancellationToken.None);
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
    public virtual async Task<PageAccessGroup>
        PutPageAccessGroupComponents(string page_id, string page_access_group_id,
            PutPagesPageIdPageAccessGroupsPageAccessGroupIdComponents body,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (page_access_group_id == null)
        throw new ArgumentNullException("page_access_group_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/page_access_groups/{page_access_group_id}/components");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{page_access_group_id}",
          Uri.EscapeDataString(ConvertToString(page_access_group_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("PUT");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<PageAccessGroup>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<PageAccessGroup>
        PostPageAccessGroupComponents(string page_id, string page_access_group_id,
            PostPagesPageIdPageAccessGroupsPageAccessGroupIdComponents body) {
      return PostPageAccessGroupComponents(page_id, page_access_group_id, body,
          CancellationToken.None);
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
    public virtual async Task<PageAccessGroup>
        PostPageAccessGroupComponents(string page_id, string page_access_group_id,
            PostPagesPageIdPageAccessGroupsPageAccessGroupIdComponents body,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (page_access_group_id == null)
        throw new ArgumentNullException("page_access_group_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/page_access_groups/{page_access_group_id}/components");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{page_access_group_id}",
          Uri.EscapeDataString(ConvertToString(page_access_group_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("POST");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 201) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<PageAccessGroup>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<PageAccessGroup>
        DeletePageAccessGroupComponents(string page_id,
            string page_access_group_id,
            DeletePagesPageIdPageAccessGroupsPageAccessGroupIdComponents body) {
      return DeletePageAccessGroupComponents(page_id, page_access_group_id,
          body, CancellationToken.None);
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
    public virtual async Task<PageAccessGroup>
        DeletePageAccessGroupComponents(string page_id,
            string page_access_group_id,
            DeletePagesPageIdPageAccessGroupsPageAccessGroupIdComponents body,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (page_access_group_id == null)
        throw new ArgumentNullException("page_access_group_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/page_access_groups/{page_access_group_id}/components");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{page_access_group_id}",
          Uri.EscapeDataString(ConvertToString(page_access_group_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("DELETE");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<PageAccessGroup>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<ICollection<Component>>
        GetPageAccessGroupComponents(string page_id, string page_access_group_id,
            int? page, int? per_page) {
      return GetPageAccessGroupComponents(page_id, page_access_group_id, page,
          per_page, CancellationToken.None);
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
    public virtual async Task<ICollection<Component>>
        GetPageAccessGroupComponents(string page_id, string page_access_group_id,
            int? page, int? per_page, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (page_access_group_id == null)
        throw new ArgumentNullException("page_access_group_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/page_access_groups/{page_access_group_id}/components?");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{page_access_group_id}",
          Uri.EscapeDataString(ConvertToString(page_access_group_id,
              CultureInfo.InvariantCulture)));
      if (page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (per_page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("per_page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(per_page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder_.Length--;

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ICollection<Component>>(
                      response_, headers_, cancellationToken).ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

    /// <summary>
    /// Remove a component from a page access group
    /// </summary>
    /// <remarks>
    /// Remove a component from a page access group
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page_access_group_id">Page Access Group Identifier</param>
    /// <param name="component_id">Component identifier</param>
    /// <returns>Remove a component from a page access group</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual Task<PageAccessGroup>
        DeletePageAccessGroupComponent(string page_id,
            string page_access_group_id, string component_id) {
      return DeletePageAccessGroupComponent(page_id,
          page_access_group_id, component_id, CancellationToken.None);
    }

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Remove a component from a page access group
    /// </summary>
    /// <remarks>
    /// Remove a component from a page access group
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <param name="page_access_group_id">Page Access Group Identifier</param>
    /// <param name="component_id">Component identifier</param>
    /// <returns>Remove a component from a page access group</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<PageAccessGroup>
        DeletePageAccessGroupComponent(string page_id,
            string page_access_group_id, string component_id,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (page_access_group_id == null)
        throw new ArgumentNullException("page_access_group_id");

      if (component_id == null)
        throw new ArgumentNullException("component_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/page_access_groups/{page_access_group_id}/components/{component_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{page_access_group_id}",
          Uri.EscapeDataString(ConvertToString(page_access_group_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{component_id}",
          Uri.EscapeDataString(ConvertToString(component_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("DELETE");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<PageAccessGroup>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

    /// <summary>
    /// Resend confirmations to a list of subscribers
    /// </summary>
    /// <remarks>
    /// Resend confirmations to a list of subscribers
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Resend confirmations to a list of subscribers</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual Task PostSubscribersResendConfirmation(string page_id,
        PostPagesPageIdSubscribersResendConfirmation body) {
      return PostSubscribersResendConfirmation(page_id, body,
          CancellationToken.None);
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
    public virtual async Task PostSubscribersResendConfirmation(
        string page_id, PostPagesPageIdSubscribersResendConfirmation body,
        CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/subscribers/resend_confirmation");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("POST");

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 201) {
              return;
            }
            else if (status_ == 400) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Bad request", status_, objectResponse_.Text, headers_,
                  objectResponse_.Object, null);
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

    /// <summary>
    /// Unsubscribe a list of subscribers
    /// </summary>
    /// <remarks>
    /// Unsubscribe a list of subscribers
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Unsubscribe a list of subscribers</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual Task PostSubscribersUnsubscribe(string page_id,
        PostPagesPageIdSubscribersUnsubscribe body) {
      return PostSubscribersUnsubscribe(page_id, body, CancellationToken.None);
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
    public virtual async Task PostSubscribersUnsubscribe(string page_id,
        PostPagesPageIdSubscribersUnsubscribe body, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/subscribers/unsubscribe");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("POST");

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 201) {
              return;
            }
            else if (status_ == 400) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Bad request", status_, objectResponse_.Text, headers_,
                  objectResponse_.Object, null);
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

    /// <summary>
    /// Reactivate a list of subscribers
    /// </summary>
    /// <remarks>
    /// Reactivate a list of quarantined subscribers
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Reactivate a list of quarantined subscribers</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual Task PostSubscribersReactivate(string page_id,
        PostPagesPageIdSubscribersReactivate body) {
      return PostSubscribersReactivate(page_id, body, CancellationToken.None);
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
    public virtual async Task PostSubscribersReactivate(string page_id,
        PostPagesPageIdSubscribersReactivate body, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/subscribers/reactivate");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("POST");

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 201) {
              return;
            }
            else if (status_ == 400) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Bad request", status_, objectResponse_.Text, headers_,
                  objectResponse_.Object, null);
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

    /// <summary>
    /// Get a histogram of subscribers by type and then state
    /// </summary>
    /// <remarks>
    /// Get a histogram of subscribers by type and then state
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Get a histogram of subscribers by type and then state</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual Task<SubscriberCountByTypeAndState>
        GetSubscribersHistogramByState(string page_id) {
      return GetSubscribersHistogramByState(page_id, CancellationToken.None);
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
    public virtual async Task<SubscriberCountByTypeAndState>
        GetSubscribersHistogramByState(string page_id,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/subscribers/histogram_by_state");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<SubscriberCountByTypeAndState>(response_, headers_,
                      cancellationToken).ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<SubscriberCountByType> GetSubscribersCount(
        string page_id, Type? type, State? state) {
      return GetSubscribersCount(page_id, type, state, CancellationToken.None);
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
    public virtual async Task<SubscriberCountByType> GetSubscribersCount(
        string page_id, Type? type, State? state, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/subscribers/count?");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      if (type != null) {
        urlBuilder_.Append(Uri.EscapeDataString("type") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(type,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (state != null) {
        urlBuilder_.Append(Uri.EscapeDataString("state") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(state,
                CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder_.Length--;

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<SubscriberCountByType>(response_, headers_,
                      cancellationToken).ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 400) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Bad request", status_, objectResponse_.Text, headers_,
                  objectResponse_.Object, null);
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<ICollection<Subscriber>>
        GetSubscribersUnsubscribed(string page_id, int? page, int? per_page) {
      return GetSubscribersUnsubscribed(page_id, page, per_page,
          CancellationToken.None);
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
    public virtual async Task<ICollection<Subscriber>>
        GetSubscribersUnsubscribed(string page_id, int? page, int? per_page,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/subscribers/unsubscribed?");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      if (page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (per_page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("per_page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(per_page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder_.Length--;

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ICollection<Subscriber>>(
                      response_, headers_, cancellationToken).ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

    /// <summary>
    /// Create a subscriber
    /// </summary>
    /// <remarks>
    /// Create a subscriber. Not applicable for Slack subscribers.
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Create a subscriber. Not applicable for Slack subscribers.</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual Task<Subscriber> PostSubscribers(string page_id,
        PostPagesPageIdSubscribers body) {
      return PostSubscribers(page_id, body, CancellationToken.None);
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
    public virtual async Task<Subscriber> PostSubscribers(string page_id,
        PostPagesPageIdSubscribers body, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/pages/{page_id}/subscribers");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("POST");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 201) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Subscriber>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 400) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Bad request", status_, objectResponse_.Text, headers_,
                  objectResponse_.Object, null);
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<ICollection<Subscriber>>
        GetSubscribers(string page_id, string q, Type2? type, State2? state, int? limit, int? page,
            Sort_field? sort_field, Sort_direction? sort_direction) {
      return GetSubscribers(page_id, q, type, state, limit, page, sort_field, sort_direction,
          CancellationToken.None);
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
    public virtual async Task<ICollection<Subscriber>>
        GetSubscribers(string page_id, string q, Type2? type, State2? state, int? limit, int? page,
            Sort_field? sort_field, Sort_direction? sort_direction,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/pages/{page_id}/subscribers?");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      if (q != null) {
        urlBuilder_.Append(Uri.EscapeDataString("q") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(q,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (type != null) {
        urlBuilder_.Append(Uri.EscapeDataString("type") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(type,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (state != null) {
        urlBuilder_.Append(Uri.EscapeDataString("state") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(state,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (limit != null) {
        urlBuilder_.Append(Uri.EscapeDataString("limit") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(limit,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (sort_field != null) {
        urlBuilder_.Append(Uri.EscapeDataString("sort_field") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(sort_field,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (sort_direction != null) {
        urlBuilder_.Append(Uri.EscapeDataString("sort_direction") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(sort_direction,
                CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder_.Length--;

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ICollection<Subscriber>>(
                      response_, headers_, cancellationToken).ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 400) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Bad request", status_, objectResponse_.Text, headers_,
                  objectResponse_.Object, null);
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task PostSubscriberResendConfirmation(
        string page_id, string subscriber_id) {
      return PostSubscriberResendConfirmation(page_id, subscriber_id,
          CancellationToken.None);
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
    public virtual async Task PostSubscriberResendConfirmation(
        string page_id, string subscriber_id, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (subscriber_id == null)
        throw new ArgumentNullException("subscriber_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/subscribers/{subscriber_id}/resend_confirmation");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{subscriber_id}",
          Uri.EscapeDataString(ConvertToString(subscriber_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Content =
              new StringContent(string.Empty, Encoding.UTF8, "application/json");
          request_.Method = new HttpMethod("POST");

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 201) {
              return;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<Subscriber> DeleteSubscriber(
        string page_id, string subscriber_id, bool? skip_unsubscription_notification) {
      return DeleteSubscriber(page_id, subscriber_id,
          skip_unsubscription_notification, CancellationToken.None);
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
    public virtual async Task<Subscriber> DeleteSubscriber(
        string page_id, string subscriber_id, bool? skip_unsubscription_notification,
        CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (subscriber_id == null)
        throw new ArgumentNullException("subscriber_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/subscribers/{subscriber_id}?");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{subscriber_id}",
          Uri.EscapeDataString(ConvertToString(subscriber_id,
              CultureInfo.InvariantCulture)));
      if (skip_unsubscription_notification != null) {
        urlBuilder_.Append(Uri.EscapeDataString("skip_unsubscription_notification") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(skip_unsubscription_notification,
                CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder_.Length--;

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("DELETE");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Subscriber>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<Subscriber> PatchSubscriber(
        string page_id, string subscriber_id, PatchPagesPageIdSubscribers body) {
      return PatchSubscriber(page_id, subscriber_id, body,
          CancellationToken.None);
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
    public virtual async Task<Subscriber> PatchSubscriber(
        string page_id, string subscriber_id, PatchPagesPageIdSubscribers body,
        CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (subscriber_id == null)
        throw new ArgumentNullException("subscriber_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/subscribers/{subscriber_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{subscriber_id}",
          Uri.EscapeDataString(ConvertToString(subscriber_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("PATCH");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Subscriber>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
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
    public virtual Task<Subscriber> GetSubscriber(
        string page_id, string subscriber_id) {
      return GetSubscriber(page_id, subscriber_id,
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
    public virtual async Task<Subscriber> GetSubscriber(
        string page_id, string subscriber_id, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (subscriber_id == null)
        throw new ArgumentNullException("subscriber_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/subscribers/{subscriber_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{subscriber_id}",
          Uri.EscapeDataString(ConvertToString(subscriber_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Subscriber>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

    /// <summary>
    /// Create a template
    /// </summary>
    /// <remarks>
    /// Create a template
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Create a template</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual Task<IncidentTemplate> PostIncidentTemplates(
        string page_id, PostPagesPageIdIncidentTemplates body) {
      return PostIncidentTemplates(page_id, body, CancellationToken.None);
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
    public virtual async Task<IncidentTemplate> PostIncidentTemplates(
        string page_id, PostPagesPageIdIncidentTemplates body,
        CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/incident_templates");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("POST");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 201) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<IncidentTemplate>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 400) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Bad request", status_, objectResponse_.Text, headers_,
                  objectResponse_.Object, null);
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<ICollection<IncidentTemplate>>
        GetIncidentTemplates(string page_id, int? page, int? per_page) {
      return GetIncidentTemplates(page_id, page, per_page,
          CancellationToken.None);
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
    public virtual async Task<ICollection<IncidentTemplate>>
        GetIncidentTemplates(string page_id, int? page, int? per_page,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/incident_templates?");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      if (page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (per_page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("per_page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(per_page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder_.Length--;

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ICollection<IncidentTemplate>>(
                      response_, headers_, cancellationToken).ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

    /// <summary>
    /// Create an incident
    /// </summary>
    /// <remarks>
    /// Create an incident
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Create an incident</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual Task<Incident> PostIncidents(string page_id,
        PostPagesPageIdIncidents body) {
      return PostIncidents(page_id, body, CancellationToken.None);
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
    public virtual async Task<Incident> PostIncidents(string page_id,
        PostPagesPageIdIncidents body, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/pages/{page_id}/incidents");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("POST");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 201) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Incident>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 400) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Bad request", status_, objectResponse_.Text, headers_,
                  objectResponse_.Object, null);
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<ICollection<Incident>>
        GetIncidents(string page_id, string q, int? limit, int? page) {
      return GetIncidents(page_id, q, limit, page, CancellationToken.None);
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
    public virtual async Task<ICollection<Incident>>
        GetIncidents(string page_id, string q, int? limit, int? page,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/pages/{page_id}/incidents?");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      if (q != null) {
        urlBuilder_.Append(Uri.EscapeDataString("q") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(q,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (limit != null) {
        urlBuilder_.Append(Uri.EscapeDataString("limit") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(limit,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder_.Length--;

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ICollection<Incident>>(
                      response_, headers_, cancellationToken).ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 400) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Bad request", status_, objectResponse_.Text, headers_,
                  objectResponse_.Object, null);
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<ICollection<Incident>>
        GetPagesPageIdIncidentsActiveMaintenanceAsync(string page_id, int? page, int? per_page) {
      return GetPagesPageIdIncidentsActiveMaintenanceAsync(page_id, page, per_page,
          CancellationToken.None);
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
    public virtual async Task<ICollection<Incident>>
        GetPagesPageIdIncidentsActiveMaintenanceAsync(string page_id, int? page, int? per_page,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/incidents/active_maintenance?");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      if (page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (per_page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("per_page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(per_page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder_.Length--;

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ICollection<Incident>>(
                      response_, headers_, cancellationToken).ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<ICollection<Incident>>
        GetPagesPageIdIncidentsUpcomingAsync(string page_id, int? page, int? per_page) {
      return GetPagesPageIdIncidentsUpcomingAsync(page_id, page, per_page,
          CancellationToken.None);
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
    public virtual async Task<ICollection<Incident>>
        GetPagesPageIdIncidentsUpcomingAsync(string page_id, int? page, int? per_page,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/incidents/upcoming?");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      if (page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (per_page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("per_page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(per_page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder_.Length--;

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ICollection<Incident>>(
                      response_, headers_, cancellationToken).ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<ICollection<Incident>>
        GetPagesPageIdIncidentsScheduledAsync(string page_id, int? page, int? per_page) {
      return GetPagesPageIdIncidentsScheduledAsync(page_id, page, per_page,
          CancellationToken.None);
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
    public virtual async Task<ICollection<Incident>>
        GetPagesPageIdIncidentsScheduledAsync(string page_id, int? page, int? per_page,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/incidents/scheduled?");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      if (page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (per_page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("per_page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(per_page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder_.Length--;

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ICollection<Incident>>(
                      response_, headers_, cancellationToken).ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<ICollection<Incident>>
        GetPagesPageIdIncidentsUnresolvedAsync(string page_id, int? page, int? per_page) {
      return GetPagesPageIdIncidentsUnresolvedAsync(page_id, page, per_page,
          CancellationToken.None);
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
    public virtual async Task<ICollection<Incident>>
        GetPagesPageIdIncidentsUnresolvedAsync(string page_id, int? page, int? per_page,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/incidents/unresolved?");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      if (page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (per_page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("per_page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(per_page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder_.Length--;

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ICollection<Incident>>(
                      response_, headers_, cancellationToken).ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<Incident> DeletePagesPageIdIncidentsIncidentIdAsync(string page_id,
        string incident_id) {
      return DeletePagesPageIdIncidentsIncidentIdAsync(page_id, incident_id,
          CancellationToken.None);
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
    public virtual async Task<Incident> DeletePagesPageIdIncidentsIncidentIdAsync(
        string page_id, string incident_id, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (incident_id == null)
        throw new ArgumentNullException("incident_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/incidents/{incident_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{incident_id}",
          Uri.EscapeDataString(ConvertToString(incident_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("DELETE");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Incident>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<Incident> PatchPagesPageIdIncidentsIncidentIdAsync(string page_id,
        string incident_id, PatchPagesPageIdIncidents body) {
      return PatchPagesPageIdIncidentsIncidentIdAsync(page_id, incident_id, body,
          CancellationToken.None);
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
    public virtual async Task<Incident> PatchPagesPageIdIncidentsIncidentIdAsync(
        string page_id, string incident_id, PatchPagesPageIdIncidents body,
        CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (incident_id == null)
        throw new ArgumentNullException("incident_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/incidents/{incident_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{incident_id}",
          Uri.EscapeDataString(ConvertToString(incident_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("PATCH");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Incident>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<Incident> PutPagesPageIdIncidentsIncidentIdAsync(string page_id,
        string incident_id, PutPagesPageIdIncidents body) {
      return PutPagesPageIdIncidentsIncidentIdAsync(page_id, incident_id, body,
          CancellationToken.None);
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
    public virtual async Task<Incident> PutPagesPageIdIncidentsIncidentIdAsync(
        string page_id, string incident_id, PutPagesPageIdIncidents body,
        CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (incident_id == null)
        throw new ArgumentNullException("incident_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/incidents/{incident_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{incident_id}",
          Uri.EscapeDataString(ConvertToString(incident_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("PUT");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Incident>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<Incident> GetPagesPageIdIncidentsIncidentIdAsync(string page_id,
        string incident_id) {
      return GetPagesPageIdIncidentsIncidentIdAsync(page_id, incident_id,
          CancellationToken.None);
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
    public virtual async Task<Incident> GetPagesPageIdIncidentsIncidentIdAsync(
        string page_id, string incident_id, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (incident_id == null)
        throw new ArgumentNullException("incident_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/incidents/{incident_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{incident_id}",
          Uri.EscapeDataString(ConvertToString(incident_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Incident>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<IncidentUpdate>
        PatchPagesPageIdIncidentsIncidentIdIncidentUpdatesIncidentUpdateIdAsync(string page_id, string incident_id,
            string incident_update_id, PatchPagesPageIdIncidentsIncidentIdIncidentUpdates body) {
      return PatchPagesPageIdIncidentsIncidentIdIncidentUpdatesIncidentUpdateIdAsync(page_id, incident_id,
          incident_update_id, body, CancellationToken.None);
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
    public virtual async Task<IncidentUpdate>
        PatchPagesPageIdIncidentsIncidentIdIncidentUpdatesIncidentUpdateIdAsync(string page_id, string incident_id,
            string incident_update_id, PatchPagesPageIdIncidentsIncidentIdIncidentUpdates body,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (incident_id == null)
        throw new ArgumentNullException("incident_id");

      if (incident_update_id == null)
        throw new ArgumentNullException("incident_update_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/incidents/{incident_id}/incident_updates/{incident_update_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{incident_id}",
          Uri.EscapeDataString(ConvertToString(incident_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{incident_update_id}",
          Uri.EscapeDataString(ConvertToString(incident_update_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("PATCH");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<IncidentUpdate>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<IncidentUpdate>
        PutPagesPageIdIncidentsIncidentIdIncidentUpdatesIncidentUpdateIdAsync(string page_id, string incident_id,
            string incident_update_id, PutPagesPageIdIncidentsIncidentIdIncidentUpdates body) {
      return PutPagesPageIdIncidentsIncidentIdIncidentUpdatesIncidentUpdateIdAsync(page_id, incident_id,
          incident_update_id, body, CancellationToken.None);
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
    public virtual async Task<IncidentUpdate>
        PutPagesPageIdIncidentsIncidentIdIncidentUpdatesIncidentUpdateIdAsync(string page_id, string incident_id,
            string incident_update_id, PutPagesPageIdIncidentsIncidentIdIncidentUpdates body,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (incident_id == null)
        throw new ArgumentNullException("incident_id");

      if (incident_update_id == null)
        throw new ArgumentNullException("incident_update_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/incidents/{incident_id}/incident_updates/{incident_update_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{incident_id}",
          Uri.EscapeDataString(ConvertToString(incident_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{incident_update_id}",
          Uri.EscapeDataString(ConvertToString(incident_update_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("PUT");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<IncidentUpdate>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<Subscriber> PostPagesPageIdIncidentsIncidentIdSubscribersAsync(
        string page_id, string incident_id, PostPagesPageIdIncidentsIncidentIdSubscribers body) {
      return PostPagesPageIdIncidentsIncidentIdSubscribersAsync(page_id, incident_id, body,
          CancellationToken.None);
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
    public virtual async Task<Subscriber> PostPagesPageIdIncidentsIncidentIdSubscribersAsync(
        string page_id, string incident_id, PostPagesPageIdIncidentsIncidentIdSubscribers body,
        CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (incident_id == null)
        throw new ArgumentNullException("incident_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/incidents/{incident_id}/subscribers");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{incident_id}",
          Uri.EscapeDataString(ConvertToString(incident_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("POST");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 201) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Subscriber>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 400) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Bad request", status_, objectResponse_.Text, headers_,
                  objectResponse_.Object, null);
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<ICollection<Subscriber>>
        GetPagesPageIdIncidentsIncidentIdSubscribersAsync(string page_id, string incident_id, int? page,
            int? per_page) {
      return GetPagesPageIdIncidentsIncidentIdSubscribersAsync(page_id, incident_id, page, per_page,
          CancellationToken.None);
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
    public virtual async Task<ICollection<Subscriber>>
        GetPagesPageIdIncidentsIncidentIdSubscribersAsync(string page_id, string incident_id, int? page,
            int? per_page,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (incident_id == null)
        throw new ArgumentNullException("incident_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/incidents/{incident_id}/subscribers?");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{incident_id}",
          Uri.EscapeDataString(ConvertToString(incident_id,
              CultureInfo.InvariantCulture)));
      if (page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (per_page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("per_page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(per_page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder_.Length--;

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ICollection<Subscriber>>(
                      response_, headers_, cancellationToken).ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<Subscriber>
        DeletePagesPageIdIncidentsIncidentIdSubscribersSubscriberIdAsync(string page_id, string incident_id,
            string subscriber_id) {
      return DeletePagesPageIdIncidentsIncidentIdSubscribersSubscriberIdAsync(page_id, incident_id, subscriber_id,
          CancellationToken.None);
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
    public virtual async Task<Subscriber>
        DeletePagesPageIdIncidentsIncidentIdSubscribersSubscriberIdAsync(string page_id, string incident_id,
            string subscriber_id, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (incident_id == null)
        throw new ArgumentNullException("incident_id");

      if (subscriber_id == null)
        throw new ArgumentNullException("subscriber_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/incidents/{incident_id}/subscribers/{subscriber_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{incident_id}",
          Uri.EscapeDataString(ConvertToString(incident_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{subscriber_id}",
          Uri.EscapeDataString(ConvertToString(subscriber_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("DELETE");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Subscriber>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<Subscriber>
        GetPagesPageIdIncidentsIncidentIdSubscribersSubscriberIdAsync(string page_id, string incident_id,
            string subscriber_id) {
      return GetPagesPageIdIncidentsIncidentIdSubscribersSubscriberIdAsync(page_id, incident_id, subscriber_id,
          CancellationToken.None);
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
    public virtual async Task<Subscriber>
        GetPagesPageIdIncidentsIncidentIdSubscribersSubscriberIdAsync(string page_id, string incident_id,
            string subscriber_id, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (incident_id == null)
        throw new ArgumentNullException("incident_id");

      if (subscriber_id == null)
        throw new ArgumentNullException("subscriber_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/incidents/{incident_id}/subscribers/{subscriber_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{incident_id}",
          Uri.EscapeDataString(ConvertToString(incident_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{subscriber_id}",
          Uri.EscapeDataString(ConvertToString(subscriber_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Subscriber>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task
        PostPagesPageIdIncidentsIncidentIdSubscribersSubscriberIdResendConfirmationAsync(string page_id,
            string incident_id, string subscriber_id) {
      return PostPagesPageIdIncidentsIncidentIdSubscribersSubscriberIdResendConfirmationAsync(page_id,
          incident_id, subscriber_id, CancellationToken.None);
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
    public virtual async Task
        PostPagesPageIdIncidentsIncidentIdSubscribersSubscriberIdResendConfirmationAsync(string page_id,
            string incident_id, string subscriber_id, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (incident_id == null)
        throw new ArgumentNullException("incident_id");

      if (subscriber_id == null)
        throw new ArgumentNullException("subscriber_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append(
          "/pages/{page_id}/incidents/{incident_id}/subscribers/{subscriber_id}/resend_confirmation");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{incident_id}",
          Uri.EscapeDataString(ConvertToString(incident_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{subscriber_id}",
          Uri.EscapeDataString(ConvertToString(subscriber_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Content =
              new StringContent(string.Empty, Encoding.UTF8, "application/json");
          request_.Method = new HttpMethod("POST");

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 201) {
              return;
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<Postmortem> GetPagesPageIdIncidentsIncidentIdPostmortemAsync(
        string page_id, string incident_id) {
      return GetPagesPageIdIncidentsIncidentIdPostmortemAsync(page_id, incident_id,
          CancellationToken.None);
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
    public virtual async Task<Postmortem> GetPagesPageIdIncidentsIncidentIdPostmortemAsync(
        string page_id, string incident_id, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (incident_id == null)
        throw new ArgumentNullException("incident_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/incidents/{incident_id}/postmortem");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{incident_id}",
          Uri.EscapeDataString(ConvertToString(incident_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Postmortem>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<Postmortem> PutPagesPageIdIncidentsIncidentIdPostmortemAsync(
        string page_id, string incident_id, PutPagesPageIdIncidentsIncidentIdPostmortem body) {
      return PutPagesPageIdIncidentsIncidentIdPostmortemAsync(page_id, incident_id, body,
          CancellationToken.None);
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
    public virtual async Task<Postmortem> PutPagesPageIdIncidentsIncidentIdPostmortemAsync(
        string page_id, string incident_id, PutPagesPageIdIncidentsIncidentIdPostmortem body,
        CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (incident_id == null)
        throw new ArgumentNullException("incident_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/incidents/{incident_id}/postmortem");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{incident_id}",
          Uri.EscapeDataString(ConvertToString(incident_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("PUT");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Postmortem>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 400) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Bad request", status_, objectResponse_.Text, headers_,
                  objectResponse_.Object, null);
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task DeletePagesPageIdIncidentsIncidentIdPostmortemAsync(string page_id,
        string incident_id) {
      return DeletePagesPageIdIncidentsIncidentIdPostmortemAsync(page_id, incident_id,
          CancellationToken.None);
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
    public virtual async Task DeletePagesPageIdIncidentsIncidentIdPostmortemAsync(
        string page_id, string incident_id, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (incident_id == null)
        throw new ArgumentNullException("incident_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/incidents/{incident_id}/postmortem");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{incident_id}",
          Uri.EscapeDataString(ConvertToString(incident_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("DELETE");

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 204) {
              return;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<Postmortem> PutPagesPageIdIncidentsIncidentIdPostmortemPublishAsync(
        string page_id, string incident_id, PutPagesPageIdIncidentsIncidentIdPostmortemPublish body) {
      return PutPagesPageIdIncidentsIncidentIdPostmortemPublishAsync(page_id, incident_id, body,
          CancellationToken.None);
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
    public virtual async Task<Postmortem>
        PutPagesPageIdIncidentsIncidentIdPostmortemPublishAsync(string page_id, string incident_id,
            PutPagesPageIdIncidentsIncidentIdPostmortemPublish body,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (incident_id == null)
        throw new ArgumentNullException("incident_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/incidents/{incident_id}/postmortem/publish");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{incident_id}",
          Uri.EscapeDataString(ConvertToString(incident_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("PUT");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Postmortem>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 400) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Bad request", status_, objectResponse_.Text, headers_,
                  objectResponse_.Object, null);
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<Postmortem> PutPagesPageIdIncidentsIncidentIdPostmortemRevertAsync(
        string page_id, string incident_id) {
      return PutPagesPageIdIncidentsIncidentIdPostmortemRevertAsync(page_id, incident_id,
          CancellationToken.None);
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
    public virtual async Task<Postmortem>
        PutPagesPageIdIncidentsIncidentIdPostmortemRevertAsync(string page_id, string incident_id,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (incident_id == null)
        throw new ArgumentNullException("incident_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/incidents/{incident_id}/postmortem/revert");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{incident_id}",
          Uri.EscapeDataString(ConvertToString(incident_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Content =
              new StringContent(string.Empty, Encoding.UTF8, "application/json");
          request_.Method = new HttpMethod("PUT");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Postmortem>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 400) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Bad request", status_, objectResponse_.Text, headers_,
                  objectResponse_.Object, null);
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

    /// <summary>
    /// Create a component
    /// </summary>
    /// <remarks>
    /// Create a component
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Create a component</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual Task<Component> PostPagesPageIdComponentsAsync(string page_id,
        PostPagesPageIdComponents body) {
      return PostPagesPageIdComponentsAsync(page_id, body, CancellationToken.None);
    }

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    /// Create a component
    /// </summary>
    /// <remarks>
    /// Create a component
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Create a component</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<Component> PostPagesPageIdComponentsAsync(string page_id,
        PostPagesPageIdComponents body, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/pages/{page_id}/components");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("POST");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 201) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Component>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<ICollection<Component>>
        GetPagesComponentsAsync(string page_id, int? page = null, int? per_page = null) {
      return GetPagesComponentsAsync(page_id, page, per_page, CancellationToken.None);
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
    public virtual async Task<ICollection<Component>>
        GetPagesComponentsAsync(string page_id, int? page, int? per_page,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/pages/{page_id}/components?");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      if (page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (per_page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("per_page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(per_page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder_.Length--;

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ICollection<Component>>(
                      response_, headers_, cancellationToken).ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<Component> PatchPagesPageIdComponentsComponentIdAsync(string page_id,
        string component_id, PatchPagesPageIdComponents body) {
      return PatchPagesPageIdComponentsComponentIdAsync(page_id, component_id, body,
          CancellationToken.None);
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
    public virtual async Task<Component> PatchPagesPageIdComponentsComponentIdAsync(
        string page_id, string component_id, PatchPagesPageIdComponents body,
        CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (component_id == null)
        throw new ArgumentNullException("component_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/components/{component_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{component_id}",
          Uri.EscapeDataString(ConvertToString(component_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("PATCH");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Component>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<Component> PutPagesPageIdComponentsComponentIdAsync(string page_id,
        string component_id, PutPagesPageIdComponents body) {
      return PutPagesPageIdComponentsComponentIdAsync(page_id, component_id, body,
          CancellationToken.None);
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
    public virtual async Task<Component> PutPagesPageIdComponentsComponentIdAsync(
        string page_id, string component_id, PutPagesPageIdComponents body,
        CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (component_id == null)
        throw new ArgumentNullException("component_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/components/{component_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{component_id}",
          Uri.EscapeDataString(ConvertToString(component_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("PUT");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Component>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task DeletePagesPageIdComponentsComponentIdAsync(string page_id,
        string component_id) {
      return DeletePagesPageIdComponentsComponentIdAsync(page_id, component_id,
          CancellationToken.None);
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
    public virtual async Task DeletePagesPageIdComponentsComponentIdAsync(string page_id,
        string component_id, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (component_id == null)
        throw new ArgumentNullException("component_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/components/{component_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{component_id}",
          Uri.EscapeDataString(ConvertToString(component_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("DELETE");

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 204) {
              return;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<Component> GetPagesPageIdComponentsComponentIdAsync(string page_id,
        string component_id) {
      return GetPagesPageIdComponentsComponentIdAsync(page_id, component_id,
          CancellationToken.None);
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
    public virtual async Task<Component> GetPagesPageIdComponentsComponentIdAsync(
        string page_id, string component_id, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (component_id == null)
        throw new ArgumentNullException("component_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/components/{component_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{component_id}",
          Uri.EscapeDataString(ConvertToString(component_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Component>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<ComponentUptime> GetPagesPageIdComponentsComponentIdUptimeAsync(
        string page_id, string component_id, bool? skip_related_events, object start, object end) {
      return GetPagesPageIdComponentsComponentIdUptimeAsync(page_id, component_id, skip_related_events, start,
          end, CancellationToken.None);
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
    public virtual async Task<ComponentUptime>
        GetPagesPageIdComponentsComponentIdUptimeAsync(string page_id, string component_id,
            bool? skip_related_events,
            object start, object end, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (component_id == null)
        throw new ArgumentNullException("component_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/components/{component_id}/uptime?");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{component_id}",
          Uri.EscapeDataString(ConvertToString(component_id,
              CultureInfo.InvariantCulture)));
      if (skip_related_events != null) {
        urlBuilder_.Append(Uri.EscapeDataString("skip_related_events") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(skip_related_events,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (start != null) {
        urlBuilder_.Append(Uri.EscapeDataString("start") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(start,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (end != null) {
        urlBuilder_.Append(Uri.EscapeDataString("end") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(end,
                CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder_.Length--;

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ComponentUptime>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<Component>
        DeletePagesPageIdComponentsComponentIdPageAccessUsersAsync(string page_id, string component_id) {
      return DeletePagesPageIdComponentsComponentIdPageAccessUsersAsync(page_id, component_id,
          CancellationToken.None);
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
    public virtual async Task<Component>
        DeletePagesPageIdComponentsComponentIdPageAccessUsersAsync(string page_id, string component_id,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (component_id == null)
        throw new ArgumentNullException("component_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/components/{component_id}/page_access_users");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{component_id}",
          Uri.EscapeDataString(ConvertToString(component_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("DELETE");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Component>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<Component> PostPagesPageIdComponentsComponentIdPageAccessUsersAsync(
        string page_id, string component_id, Body body) {
      return PostPagesPageIdComponentsComponentIdPageAccessUsersAsync(page_id, component_id, body,
          CancellationToken.None);
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
    public virtual async Task<Component>
        PostPagesPageIdComponentsComponentIdPageAccessUsersAsync(string page_id, string component_id, Body body,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (component_id == null)
        throw new ArgumentNullException("component_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/components/{component_id}/page_access_users");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{component_id}",
          Uri.EscapeDataString(ConvertToString(component_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var dictionary_ =
              JsonConvert
                  .DeserializeObject<Dictionary<string, string>>(json_,
                      settings.Value);
          var content_ = new FormUrlEncodedContent(dictionary_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");
          request_.Content = content_;
          request_.Method = new HttpMethod("POST");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 201) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Component>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<Component>
        DeletePagesPageIdComponentsComponentIdPageAccessGroupsAsync(string page_id, string component_id) {
      return DeletePagesPageIdComponentsComponentIdPageAccessGroupsAsync(page_id, component_id,
          CancellationToken.None);
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
    public virtual async Task<Component>
        DeletePagesPageIdComponentsComponentIdPageAccessGroupsAsync(string page_id, string component_id,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (component_id == null)
        throw new ArgumentNullException("component_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/components/{component_id}/page_access_groups");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{component_id}",
          Uri.EscapeDataString(ConvertToString(component_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("DELETE");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Component>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<Component> PostPagesPageIdComponentsComponentIdPageAccessGroupsAsync(
        string page_id, string component_id) {
      return PostPagesPageIdComponentsComponentIdPageAccessGroupsAsync(page_id, component_id,
          CancellationToken.None);
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
    public virtual async Task<Component>
        PostPagesPageIdComponentsComponentIdPageAccessGroupsAsync(string page_id, string component_id,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (component_id == null)
        throw new ArgumentNullException("component_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/components/{component_id}/page_access_groups");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{component_id}",
          Uri.EscapeDataString(ConvertToString(component_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Content =
              new StringContent(string.Empty, Encoding.UTF8, "application/json");
          request_.Method = new HttpMethod("POST");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 201) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Component>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

    /// <summary>
    /// Create a component group
    /// </summary>
    /// <remarks>
    /// Create a component group
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Create a component group</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual Task<GroupComponent> PostPagesPageIdComponentGroupsAsync(string page_id,
        PostPagesPageIdComponentGroups body) {
      return PostPagesPageIdComponentGroupsAsync(page_id, body, CancellationToken.None);
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
    public virtual async Task<GroupComponent> PostPagesPageIdComponentGroupsAsync(
        string page_id, PostPagesPageIdComponentGroups body, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/pages/{page_id}/component-groups");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("POST");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 201) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<GroupComponent>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 400) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Bad request", status_, objectResponse_.Text, headers_,
                  objectResponse_.Object, null);
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<ICollection<GroupComponent>>
        GetPagesPageIdComponentGroupsAsync(string page_id, int? page, int? per_page) {
      return GetPagesPageIdComponentGroupsAsync(page_id, page, per_page, CancellationToken.None);
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
    public virtual async Task<ICollection<GroupComponent>>
        GetPagesPageIdComponentGroupsAsync(string page_id, int? page, int? per_page,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/component-groups?");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      if (page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (per_page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("per_page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(per_page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder_.Length--;

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ICollection<GroupComponent>>(
                      response_, headers_, cancellationToken).ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<GroupComponent> PatchPagesPageIdComponentGroupsIdAsync(
        string page_id, string id, PatchPagesPageIdComponentGroups body) {
      return PatchPagesPageIdComponentGroupsIdAsync(page_id, id, body, CancellationToken.None);
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
    public virtual async Task<GroupComponent> PatchPagesPageIdComponentGroupsIdAsync(
        string page_id, string id, PatchPagesPageIdComponentGroups body,
        CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (id == null)
        throw new ArgumentNullException("id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/component-groups/{id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{id}",
          Uri.EscapeDataString(ConvertToString(id, CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("PATCH");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<GroupComponent>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<GroupComponent> PutPagesPageIdComponentGroupsIdAsync(string page_id,
        string id, PutPagesPageIdComponentGroups body) {
      return PutPagesPageIdComponentGroupsIdAsync(page_id, id, body, CancellationToken.None);
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
    public virtual async Task<GroupComponent> PutPagesPageIdComponentGroupsIdAsync(
        string page_id, string id, PutPagesPageIdComponentGroups body,
        CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (id == null)
        throw new ArgumentNullException("id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/component-groups/{id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{id}",
          Uri.EscapeDataString(ConvertToString(id, CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("PUT");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<GroupComponent>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<GroupComponent> DeletePagesPageIdComponentGroupsIdAsync(
        string page_id, string id) {
      return DeletePagesPageIdComponentGroupsIdAsync(page_id, id, CancellationToken.None);
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
    public virtual async Task<GroupComponent> DeletePagesPageIdComponentGroupsIdAsync(
        string page_id, string id, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (id == null)
        throw new ArgumentNullException("id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/component-groups/{id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{id}",
          Uri.EscapeDataString(ConvertToString(id, CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("DELETE");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<GroupComponent>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<GroupComponent> GetPagesPageIdComponentGroupsIdAsync(string page_id,
        string id) {
      return GetPagesPageIdComponentGroupsIdAsync(page_id, id, CancellationToken.None);
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
    public virtual async Task<GroupComponent> GetPagesPageIdComponentGroupsIdAsync(
        string page_id, string id, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (id == null)
        throw new ArgumentNullException("id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/component-groups/{id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{id}",
          Uri.EscapeDataString(ConvertToString(id, CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<GroupComponent>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<ComponentGroupUptime> GetPagesPageIdComponentGroupsIdUptimeAsync(
        string page_id, string id, bool? skip_related_events, object start, object end) {
      return GetPagesPageIdComponentGroupsIdUptimeAsync(page_id, id, skip_related_events, start, end,
          CancellationToken.None);
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
    public virtual async Task<ComponentGroupUptime>
        GetPagesPageIdComponentGroupsIdUptimeAsync(string page_id, string id, bool? skip_related_events,
            object start,
            object end, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (id == null)
        throw new ArgumentNullException("id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/component-groups/{id}/uptime?");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{id}",
          Uri.EscapeDataString(ConvertToString(id, CultureInfo.InvariantCulture)));
      if (skip_related_events != null) {
        urlBuilder_.Append(Uri.EscapeDataString("skip_related_events") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(skip_related_events,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (start != null) {
        urlBuilder_.Append(Uri.EscapeDataString("start") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(start,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (end != null) {
        urlBuilder_.Append(Uri.EscapeDataString("end") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(end,
                CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder_.Length--;

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ComponentGroupUptime>(response_, headers_,
                      cancellationToken).ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

    /// <summary>
    /// Add data points to metrics
    /// </summary>
    /// <remarks>
    /// Add data points to metrics
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Data Point is submitted and is currently being added to the metrics</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual Task<MetricAddResponse> PostPagesPageIdMetricsDataAsync(string page_id,
        PostPagesPageIdMetricsData body) {
      return PostPagesPageIdMetricsDataAsync(page_id, body, CancellationToken.None);
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
    public virtual async Task<MetricAddResponse> PostPagesPageIdMetricsDataAsync(
        string page_id, PostPagesPageIdMetricsData body, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/pages/{page_id}/metrics/data");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("POST");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 202) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<MetricAddResponse>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 405) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Method not allowed.", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<Metric> GetPagesPageIdMetricsAsync(string page_id, int? page,
        int? per_page) {
      return GetPagesPageIdMetricsAsync(page_id, page, per_page, CancellationToken.None);
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
    public virtual async Task<Metric> GetPagesPageIdMetricsAsync(string page_id, int? page,
        int? per_page, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/pages/{page_id}/metrics?");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      if (page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (per_page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("per_page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(per_page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder_.Length--;

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Metric>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<Metric> PatchPagesPageIdMetricsMetricIdAsync(string page_id,
        string metric_id, PatchPagesPageIdMetrics body) {
      return PatchPagesPageIdMetricsMetricIdAsync(page_id, metric_id, body,
          CancellationToken.None);
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
    public virtual async Task<Metric> PatchPagesPageIdMetricsMetricIdAsync(string page_id,
        string metric_id, PatchPagesPageIdMetrics body, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (metric_id == null)
        throw new ArgumentNullException("metric_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/metrics/{metric_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{metric_id}",
          Uri.EscapeDataString(ConvertToString(metric_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("PATCH");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Metric>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<Metric> PutPagesPageIdMetricsMetricIdAsync(string page_id,
        string metric_id, PutPagesPageIdMetrics body) {
      return PutPagesPageIdMetricsMetricIdAsync(page_id, metric_id, body,
          CancellationToken.None);
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
    public virtual async Task<Metric> PutPagesPageIdMetricsMetricIdAsync(string page_id,
        string metric_id, PutPagesPageIdMetrics body, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (metric_id == null)
        throw new ArgumentNullException("metric_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/metrics/{metric_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{metric_id}",
          Uri.EscapeDataString(ConvertToString(metric_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("PUT");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Metric>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<Metric> DeletePagesPageIdMetricsMetricIdAsync(string page_id,
        string metric_id) {
      return DeletePagesPageIdMetricsMetricIdAsync(page_id, metric_id, CancellationToken.None);
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
    public virtual async Task<Metric> DeletePagesPageIdMetricsMetricIdAsync(string page_id,
        string metric_id, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (metric_id == null)
        throw new ArgumentNullException("metric_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/metrics/{metric_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{metric_id}",
          Uri.EscapeDataString(ConvertToString(metric_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("DELETE");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Metric>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<Metric> GetPagesPageIdMetricsMetricIdAsync(string page_id,
        string metric_id) {
      return GetPagesPageIdMetricsMetricIdAsync(page_id, metric_id, CancellationToken.None);
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
    public virtual async Task<Metric> GetPagesPageIdMetricsMetricIdAsync(string page_id,
        string metric_id, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (metric_id == null)
        throw new ArgumentNullException("metric_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/metrics/{metric_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{metric_id}",
          Uri.EscapeDataString(ConvertToString(metric_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Metric>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<Metric> DeletePagesPageIdMetricsMetricIdDataAsync(string page_id,
        string metric_id) {
      return DeletePagesPageIdMetricsMetricIdDataAsync(page_id, metric_id,
          CancellationToken.None);
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
        string page_id, string metric_id, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (metric_id == null)
        throw new ArgumentNullException("metric_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/metrics/{metric_id}/data");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{metric_id}",
          Uri.EscapeDataString(ConvertToString(metric_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("DELETE");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Metric>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<SingleMetricAddResponse> PostPagesPageIdMetricsMetricIdDataAsync(
        string page_id, string metric_id, PostPagesPageIdMetricsMetricIdData body) {
      return PostPagesPageIdMetricsMetricIdDataAsync(page_id, metric_id, body,
          CancellationToken.None);
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
    public virtual async Task<SingleMetricAddResponse>
        PostPagesPageIdMetricsMetricIdDataAsync(string page_id, string metric_id,
            PostPagesPageIdMetricsMetricIdData body, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (metric_id == null)
        throw new ArgumentNullException("metric_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/metrics/{metric_id}/data");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{metric_id}",
          Uri.EscapeDataString(ConvertToString(metric_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("POST");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 201) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<SingleMetricAddResponse>(response_, headers_,
                      cancellationToken).ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 405) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Method not allowed.", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

    /// <summary>
    /// Get a list of metric providers
    /// </summary>
    /// <remarks>
    /// Get a list of metric providers
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Get a list of metric providers</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual Task<ICollection<MetricsProvider>>
        GetPagesPageIdMetricsProvidersAsync(string page_id) {
      return GetPagesPageIdMetricsProvidersAsync(page_id, CancellationToken.None);
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
    public virtual async Task<ICollection<MetricsProvider>>
        GetPagesPageIdMetricsProvidersAsync(string page_id, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/metrics_providers");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ICollection<MetricsProvider>>(
                      response_, headers_, cancellationToken).ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

    /// <summary>
    /// Create a metric provider
    /// </summary>
    /// <remarks>
    /// Create a metric provider
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Create a metric provider</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual Task<MetricsProvider> PostPagesPageIdMetricsProvidersAsync(string page_id,
        PostPagesPageIdMetricsProviders body) {
      return PostPagesPageIdMetricsProvidersAsync(page_id, body, CancellationToken.None);
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
    public virtual async Task<MetricsProvider> PostPagesPageIdMetricsProvidersAsync(
        string page_id, PostPagesPageIdMetricsProviders body,
        CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/metrics_providers");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("POST");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 201) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<MetricsProvider>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 400) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Bad request", status_, objectResponse_.Text, headers_,
                  objectResponse_.Object, null);
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<MetricsProvider>
        GetPagesPageIdMetricsProvidersMetricsProviderIdAsync(string page_id, string metrics_provider_id) {
      return GetPagesPageIdMetricsProvidersMetricsProviderIdAsync(page_id, metrics_provider_id,
          CancellationToken.None);
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
    public virtual async Task<MetricsProvider>
        GetPagesPageIdMetricsProvidersMetricsProviderIdAsync(string page_id, string metrics_provider_id,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (metrics_provider_id == null)
        throw new ArgumentNullException("metrics_provider_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/metrics_providers/{metrics_provider_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{metrics_provider_id}",
          Uri.EscapeDataString(ConvertToString(metrics_provider_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<MetricsProvider>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<MetricsProvider>
        PatchPagesPageIdMetricsProvidersMetricsProviderIdAsync(string page_id, string metrics_provider_id,
            PatchPagesPageIdMetricsProviders body) {
      return PatchPagesPageIdMetricsProvidersMetricsProviderIdAsync(page_id, metrics_provider_id, body,
          CancellationToken.None);
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
    public virtual async Task<MetricsProvider>
        PatchPagesPageIdMetricsProvidersMetricsProviderIdAsync(string page_id, string metrics_provider_id,
            PatchPagesPageIdMetricsProviders body, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (metrics_provider_id == null)
        throw new ArgumentNullException("metrics_provider_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/metrics_providers/{metrics_provider_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{metrics_provider_id}",
          Uri.EscapeDataString(ConvertToString(metrics_provider_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("PATCH");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<MetricsProvider>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<MetricsProvider>
        PutPagesPageIdMetricsProvidersMetricsProviderIdAsync(string page_id, string metrics_provider_id,
            PutPagesPageIdMetricsProviders body) {
      return PutPagesPageIdMetricsProvidersMetricsProviderIdAsync(page_id, metrics_provider_id, body,
          CancellationToken.None);
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
    public virtual async Task<MetricsProvider>
        PutPagesPageIdMetricsProvidersMetricsProviderIdAsync(string page_id, string metrics_provider_id,
            PutPagesPageIdMetricsProviders body, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (metrics_provider_id == null)
        throw new ArgumentNullException("metrics_provider_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/metrics_providers/{metrics_provider_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{metrics_provider_id}",
          Uri.EscapeDataString(ConvertToString(metrics_provider_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("PUT");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<MetricsProvider>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<MetricsProvider>
        DeletePagesPageIdMetricsProvidersMetricsProviderIdAsync(string page_id, string metrics_provider_id) {
      return DeletePagesPageIdMetricsProvidersMetricsProviderIdAsync(page_id, metrics_provider_id,
          CancellationToken.None);
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
    public virtual async Task<MetricsProvider>
        DeletePagesPageIdMetricsProvidersMetricsProviderIdAsync(string page_id, string metrics_provider_id,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (metrics_provider_id == null)
        throw new ArgumentNullException("metrics_provider_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/metrics_providers/{metrics_provider_id}");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{metrics_provider_id}",
          Uri.EscapeDataString(ConvertToString(metrics_provider_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("DELETE");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<MetricsProvider>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<Metric> GetPagesPageIdMetricsProvidersMetricsProviderIdMetricsAsync(
        string page_id, string metrics_provider_id, int? page, int? per_page) {
      return GetPagesPageIdMetricsProvidersMetricsProviderIdMetricsAsync(page_id, metrics_provider_id, page,
          per_page, CancellationToken.None);
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
    public virtual async Task<Metric>
        GetPagesPageIdMetricsProvidersMetricsProviderIdMetricsAsync(string page_id, string metrics_provider_id,
            int? page, int? per_page, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (metrics_provider_id == null)
        throw new ArgumentNullException("metrics_provider_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/metrics_providers/{metrics_provider_id}/metrics?");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{metrics_provider_id}",
          Uri.EscapeDataString(ConvertToString(metrics_provider_id,
              CultureInfo.InvariantCulture)));
      if (page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (per_page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("per_page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(per_page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder_.Length--;

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Metric>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<Metric> PostPagesPageIdMetricsProvidersMetricsProviderIdMetricsAsync(
        string page_id, string metrics_provider_id, PostPagesPageIdMetricsProvidersMetricsProviderIdMetrics body) {
      return PostPagesPageIdMetricsProvidersMetricsProviderIdMetricsAsync(page_id, metrics_provider_id, body,
          CancellationToken.None);
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
    public virtual async Task<Metric>
        PostPagesPageIdMetricsProvidersMetricsProviderIdMetricsAsync(string page_id, string metrics_provider_id,
            PostPagesPageIdMetricsProvidersMetricsProviderIdMetrics body,
            CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (metrics_provider_id == null)
        throw new ArgumentNullException("metrics_provider_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/metrics_providers/{metrics_provider_id}/metrics");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{metrics_provider_id}",
          Uri.EscapeDataString(ConvertToString(metrics_provider_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("POST");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 201) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Metric>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 400) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Bad request", status_, objectResponse_.Text, headers_,
                  objectResponse_.Object, null);
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

    /// <summary>
    /// Get status embed config settings
    /// </summary>
    /// <remarks>
    /// Get status embed config settings
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Get status embed config settings</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual Task<StatusEmbedConfig> GetPagesPageIdStatusEmbedConfigAsync(
        string page_id) {
      return GetPagesPageIdStatusEmbedConfigAsync(page_id, CancellationToken.None);
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
    public virtual async Task<StatusEmbedConfig> GetPagesPageIdStatusEmbedConfigAsync(
        string page_id, CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/status_embed_config");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<StatusEmbedConfig>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

    /// <summary>
    /// Update status embed config settings
    /// </summary>
    /// <remarks>
    /// Update status embed config settings
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Update status embed config settings</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual Task<StatusEmbedConfig> PatchPagesPageIdStatusEmbedConfigAsync(
        string page_id, PatchPagesPageIdStatusEmbedConfig body) {
      return PatchPagesPageIdStatusEmbedConfigAsync(page_id, body, CancellationToken.None);
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
    public virtual async Task<StatusEmbedConfig> PatchPagesPageIdStatusEmbedConfigAsync(
        string page_id, PatchPagesPageIdStatusEmbedConfig body,
        CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/status_embed_config");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("PATCH");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<StatusEmbedConfig>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 400) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Bad request", status_, objectResponse_.Text, headers_,
                  objectResponse_.Object, null);
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

    /// <summary>
    /// Update status embed config settings
    /// </summary>
    /// <remarks>
    /// Update status embed config settings
    /// </remarks>
    /// <param name="page_id">Page identifier</param>
    /// <returns>Update status embed config settings</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual Task<StatusEmbedConfig> PutPagesPageIdStatusEmbedConfigAsync(
        string page_id, PutPagesPageIdStatusEmbedConfig body) {
      return PutPagesPageIdStatusEmbedConfigAsync(page_id, body, CancellationToken.None);
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
    public virtual async Task<StatusEmbedConfig> PutPagesPageIdStatusEmbedConfigAsync(
        string page_id, PutPagesPageIdStatusEmbedConfig body,
        CancellationToken cancellationToken) {
      if (page_id == null)
        throw new ArgumentNullException("page_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/pages/{page_id}/status_embed_config");
      urlBuilder_.Replace("{page_id}",
          Uri.EscapeDataString(ConvertToString(page_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("PUT");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<StatusEmbedConfig>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 400) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Bad request", status_, objectResponse_.Text, headers_,
                  objectResponse_.Object, null);
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<Permissions> PutOrganizationsOrganizationIdPermissionsUserIdAsync(
        string organization_id, string user_id, PutOrganizationsOrganizationIdPermissions body) {
      return PutOrganizationsOrganizationIdPermissionsUserIdAsync(organization_id, user_id, body,
          CancellationToken.None);
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
    public virtual async Task<Permissions>
        PutOrganizationsOrganizationIdPermissionsUserIdAsync(string organization_id, string user_id,
            PutOrganizationsOrganizationIdPermissions body, CancellationToken cancellationToken) {
      if (organization_id == null)
        throw new ArgumentNullException("organization_id");

      if (user_id == null)
        throw new ArgumentNullException("user_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/organizations/{organization_id}/permissions/{user_id}");
      urlBuilder_.Replace("{organization_id}",
          Uri.EscapeDataString(ConvertToString(organization_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{user_id}",
          Uri.EscapeDataString(ConvertToString(user_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("PUT");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Permissions>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 400) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Bad request", status_, objectResponse_.Text, headers_,
                  objectResponse_.Object, null);
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<Permissions> GetOrganizationsOrganizationIdPermissionsUserIdAsync(
        string organization_id, string user_id) {
      return GetOrganizationsOrganizationIdPermissionsUserIdAsync(organization_id, user_id,
          CancellationToken.None);
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
    public virtual async Task<Permissions>
        GetOrganizationsOrganizationIdPermissionsUserIdAsync(string organization_id, string user_id,
            CancellationToken cancellationToken) {
      if (organization_id == null)
        throw new ArgumentNullException("organization_id");

      if (user_id == null)
        throw new ArgumentNullException("user_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/organizations/{organization_id}/permissions/{user_id}");
      urlBuilder_.Replace("{organization_id}",
          Uri.EscapeDataString(ConvertToString(organization_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{user_id}",
          Uri.EscapeDataString(ConvertToString(user_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<Permissions>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<User> DeleteOrganizationsOrganizationIdUsersUserIdAsync(
        string organization_id, string user_id) {
      return DeleteOrganizationsOrganizationIdUsersUserIdAsync(organization_id, user_id,
          CancellationToken.None);
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
    public virtual async Task<User> DeleteOrganizationsOrganizationIdUsersUserIdAsync(
        string organization_id, string user_id, CancellationToken cancellationToken) {
      if (organization_id == null)
        throw new ArgumentNullException("organization_id");

      if (user_id == null)
        throw new ArgumentNullException("user_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/organizations/{organization_id}/users/{user_id}");
      urlBuilder_.Replace("{organization_id}",
          Uri.EscapeDataString(ConvertToString(organization_id,
              CultureInfo.InvariantCulture)));
      urlBuilder_.Replace("{user_id}",
          Uri.EscapeDataString(ConvertToString(user_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("DELETE");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<User>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 403) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("You are not authorized to access this resource.",
                  status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

    /// <summary>
    /// Create a user
    /// </summary>
    /// <remarks>
    /// Create a user
    /// </remarks>
    /// <param name="organization_id">Organization Identifier</param>
    /// <returns>Create a user</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual Task<User> PostOrganizationsOrganizationIdUsersAsync(
        string organization_id, PostOrganizationsOrganizationIdUsers body) {
      return PostOrganizationsOrganizationIdUsersAsync(organization_id, body,
          CancellationToken.None);
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
    public virtual async Task<User> PostOrganizationsOrganizationIdUsersAsync(
        string organization_id, PostOrganizationsOrganizationIdUsers body,
        CancellationToken cancellationToken) {
      if (organization_id == null)
        throw new ArgumentNullException("organization_id");

      if (body == null)
        throw new ArgumentNullException("body");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/organizations/{organization_id}/users");
      urlBuilder_.Replace("{organization_id}",
          Uri.EscapeDataString(ConvertToString(organization_id,
              CultureInfo.InvariantCulture)));

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          var json_ = JsonConvert.SerializeObject(body, settings.Value);
          var content_ = new StringContent(json_);
          content_.Headers.ContentType =
              MediaTypeHeaderValue.Parse("application/json");
          request_.Content = content_;
          request_.Method = new HttpMethod("POST");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 201) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<User>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else if (status_ == 422) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Unprocessable entity", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
      }
    }

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
    public virtual Task<ICollection<User>>
        GetOrganizationsOrganizationIdUsersAsync(string organization_id, int? page, int? per_page) {
      return GetOrganizationsOrganizationIdUsersAsync(organization_id, page, per_page,
          CancellationToken.None);
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
    public virtual async Task<ICollection<User>>
        GetOrganizationsOrganizationIdUsersAsync(string organization_id, int? page, int? per_page,
            CancellationToken cancellationToken) {
      if (organization_id == null)
        throw new ArgumentNullException("organization_id");

      var urlBuilder_ = new StringBuilder();
      urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
          .Append("/organizations/{organization_id}/users?");
      urlBuilder_.Replace("{organization_id}",
          Uri.EscapeDataString(ConvertToString(organization_id,
              CultureInfo.InvariantCulture)));
      if (page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      if (per_page != null) {
        urlBuilder_.Append(Uri.EscapeDataString("per_page") + "=")
            .Append(Uri.EscapeDataString(ConvertToString(per_page,
                CultureInfo.InvariantCulture))).Append("&");
      }

      urlBuilder_.Length--;

      var client_ = httpClient;
      var disposeClient_ = false;
      try {
        using (var request_ = new HttpRequestMessage()) {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(
              MediaTypeWithQualityHeaderValue.Parse("application/json"));

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          var response_ = await client_.SendAsync(request_,
                  HttpCompletionOption.ResponseHeadersRead, cancellationToken)
              .ConfigureAwait(false);
          var disposeResponse_ = true;
          try {
            var headers_ =
                Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null) {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            var status_ = (int)response_.StatusCode;
            if (status_ == 200) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ICollection<User>>(response_,
                      headers_, cancellationToken).ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              return objectResponse_.Object;
            }
            else if (status_ == 401) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("Could not authenticate", status_, objectResponse_.Text,
                  headers_, objectResponse_.Object, null);
            }
            else if (status_ == 404) {
              var objectResponse_ =
                  await ReadObjectResponseAsync<ErrorEntity>(response_, headers_, cancellationToken)
                      .ConfigureAwait(false);
              if (objectResponse_.Object == null) {
                throw new ApiException("Response was null which was not expected.", status_,
                    objectResponse_.Text, headers_, null);
              }

              throw new ApiException<ErrorEntity>("The requested resource could not be found.", status_,
                  objectResponse_.Text, headers_, objectResponse_.Object, null);
            }
            else {
              var responseData_ = response_.Content == null
                  ? null
                  : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException(
                  "The HTTP status code of the response was not expected (" + status_ + ").", status_,
                  responseData_, headers_, null);
            }
          }
          finally {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally {
        if (disposeClient_)
          client_.Dispose();
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
        var array = Enumerable.OfType<object>((Array)value);
        return string.Join(",", Enumerable.Select(array, o => ConvertToString(o, cultureInfo)));
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
    private IDictionary<string, object> _additionalProperties;

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
    public DateTimeOffset Created_at { get; set; }

    /// <summary>
    /// Timestamp the record was last updated
    /// </summary>
    [JsonProperty("updated_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Updated_at { get; set; }

    /// <summary>
    /// Name of your page to be displayed
    /// </summary>
    [JsonProperty("name", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    [JsonProperty("page_description", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Page_description { get; set; }

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
    public string Support_url { get; set; }

    /// <summary>
    /// Should your page hide itself from search engines
    /// </summary>
    [JsonProperty("hidden_from_search", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Hidden_from_search { get; set; }

    /// <summary>
    /// Can your users subscribe to all notifications on the page
    /// </summary>
    [JsonProperty("allow_page_subscribers", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Allow_page_subscribers { get; set; }

    /// <summary>
    /// Can your users subscribe to notifications for a single incident
    /// </summary>
    [JsonProperty("allow_incident_subscribers", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Allow_incident_subscribers { get; set; }

    /// <summary>
    /// Can your users choose to receive notifications via email
    /// </summary>
    [JsonProperty("allow_email_subscribers", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Allow_email_subscribers { get; set; }

    /// <summary>
    /// Can your users choose to receive notifications via SMS
    /// </summary>
    [JsonProperty("allow_sms_subscribers", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Allow_sms_subscribers { get; set; }

    /// <summary>
    /// Can your users choose to access incident feeds via RSS/Atom (not functional on Audience-Specific pages)
    /// </summary>
    [JsonProperty("allow_rss_atom_feeds", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Allow_rss_atom_feeds { get; set; }

    /// <summary>
    /// Can your users choose to receive notifications via Webhooks
    /// </summary>
    [JsonProperty("allow_webhook_subscribers", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Allow_webhook_subscribers { get; set; }

    /// <summary>
    /// Allows you to customize the email address your page notifications come from
    /// </summary>
    [JsonProperty("notifications_from_email", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Notifications_from_email { get; set; }

    /// <summary>
    /// Allows you to customize the footer appearing on your notification emails.  Accepts Markdown for formatting
    /// </summary>
    [JsonProperty("notifications_email_footer", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Notifications_email_footer { get; set; }

    [JsonProperty("activity_score", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public float Activity_score { get; set; }

    [JsonProperty("twitter_username", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Twitter_username { get; set; }

    [JsonProperty("viewers_must_be_team_members", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Viewers_must_be_team_members { get; set; }

    [JsonProperty("ip_restrictions", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Ip_restrictions { get; set; }

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
    public string Time_zone { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_body_background_color", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_body_background_color { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_font_color", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_font_color { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_light_font_color", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_light_font_color { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_greens", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_greens { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_yellows", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_yellows { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_oranges", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_oranges { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_blues", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_blues { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_reds", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_reds { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_border_color", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_border_color { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_graph_color", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_graph_color { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_link_color", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_link_color { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_no_data", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_no_data { get; set; }

    [JsonProperty("favicon_logo", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Favicon_logo { get; set; }

    [JsonProperty("transactional_logo", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Transactional_logo { get; set; }

    [JsonProperty("hero_cover", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Hero_cover { get; set; }

    [JsonProperty("email_logo", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Email_logo { get; set; }

    [JsonProperty("twitter_logo", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Twitter_logo { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Get a list of users
  /// </summary>
  public class ErrorEntity {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("message", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Message { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a page
  /// </summary>
  public class PatchPages {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("page", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Page2 Page { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a page
  /// </summary>
  public class PutPages {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("page", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Page3 Page { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Add a page access user
  /// </summary>
  public class PostPagesPageIdPageAccessUsers {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("page_access_user", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Page_access_user Page_access_user { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Delete metric for page access user
  /// </summary>
  public class PageAccessUser {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// Page Access User Identifier
    /// </summary>
    [JsonProperty("id", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    [JsonProperty("page_id", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Page_id { get; set; }

    [JsonProperty("email", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Email { get; set; }

    /// <summary>
    /// IDP login user id. Key is typically "uid".
    /// </summary>
    [JsonProperty("external_login", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string External_login { get; set; }

    [JsonProperty("page_access_group_id", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Page_access_group_id { get; set; }

    [JsonProperty("page_access_group_ids", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Page_access_group_ids { get; set; }

    [JsonProperty("created_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Created_at { get; set; }

    [JsonProperty("updated_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Updated_at { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Add components for page access user
  /// </summary>
  public class PatchPagesPageIdPageAccessUsersPageAccessUserIdComponents {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// List of component codes to allow access to
    /// </summary>
    [JsonProperty("component_ids", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public ICollection<string> Component_ids { get; set; } =
        new System.Collections.ObjectModel.Collection<string>();

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Add components for page access user
  /// </summary>
  public class PutPagesPageIdPageAccessUsersPageAccessUserIdComponents {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// List of component codes to allow access to
    /// </summary>
    [JsonProperty("component_ids", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public ICollection<string> Component_ids { get; set; } =
        new System.Collections.ObjectModel.Collection<string>();

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Replace components for page access user
  /// </summary>
  public class PostPagesPageIdPageAccessUsersPageAccessUserIdComponents {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// List of component codes to allow access to
    /// </summary>
    [JsonProperty("component_ids", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public ICollection<string> Component_ids { get; set; } =
        new System.Collections.ObjectModel.Collection<string>();

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Remove components for page access user
  /// </summary>
  public class DeletePagesPageIdPageAccessUsersPageAccessUserIdComponents {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// List of components codes to remove.  If omitted, all components will be removed.
    /// </summary>
    [JsonProperty("component_ids", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> Component_ids { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Add page access groups to a component
  /// </summary>
  public class Component {
    private IDictionary<string, object> _additionalProperties;

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
    public string Page_id { get; set; }

    /// <summary>
    /// Component Group identifier
    /// </summary>
    [JsonProperty("group_id", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Group_id { get; set; }

    [JsonProperty("created_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Created_at { get; set; }

    [JsonProperty("updated_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Updated_at { get; set; }

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
    public bool Only_show_if_degraded { get; set; }

    /// <summary>
    /// Requires a special feature flag to be enabled
    /// </summary>
    [JsonProperty("automation_email", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Automation_email { get; set; }

    /// <summary>
    /// The date this component started being used
    /// </summary>
    [JsonProperty("start_date", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(DateFormatConverter))]
    public DateTimeOffset Start_date { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Add metrics for page access user
  /// </summary>
  public class PatchPagesPageIdPageAccessUsersPageAccessUserIdMetrics {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// List of metrics to add
    /// </summary>
    [JsonProperty("metric_ids", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public ICollection<string> Metric_ids { get; set; } =
        new System.Collections.ObjectModel.Collection<string>();

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Add metrics for page access user
  /// </summary>
  public class PutPagesPageIdPageAccessUsersPageAccessUserIdMetrics {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// List of metrics to add
    /// </summary>
    [JsonProperty("metric_ids", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public ICollection<string> Metric_ids { get; set; } =
        new System.Collections.ObjectModel.Collection<string>();

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Replace metrics for page access user
  /// </summary>
  public class PostPagesPageIdPageAccessUsersPageAccessUserIdMetrics {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// List of metrics to add
    /// </summary>
    [JsonProperty("metric_ids", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public ICollection<string> Metric_ids { get; set; } =
        new System.Collections.ObjectModel.Collection<string>();

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Delete metrics for page access user
  /// </summary>
  public class DeletePagesPageIdPageAccessUsersPageAccessUserIdMetrics {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// List of metrics to remove
    /// </summary>
    [JsonProperty("metric_ids", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> Metric_ids { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Create a metric for a metric provider
  /// </summary>
  public class Metric {
    private IDictionary<string, object> _additionalProperties;

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
    public string Metrics_provider_id { get; set; }

    /// <summary>
    /// Metric Display identifier used to look up the metric data from the provider
    /// </summary>
    [JsonProperty("metric_identifier", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Metric_identifier { get; set; }

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
    public string Tooltip_description { get; set; }

    [JsonProperty("backfilled", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Backfilled { get; set; }

    [JsonProperty("y_axis_min", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public float Y_axis_min { get; set; }

    [JsonProperty("y_axis_max", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public float Y_axis_max { get; set; }

    /// <summary>
    /// Should the values on the y axis be hidden on render
    /// </summary>
    [JsonProperty("y_axis_hidden", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Y_axis_hidden { get; set; }

    /// <summary>
    /// Suffix to describe the units on the graph
    /// </summary>
    [JsonProperty("suffix", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Suffix { get; set; }

    [JsonProperty("decimal_places", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public int Decimal_places { get; set; }

    [JsonProperty("most_recent_data_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Most_recent_data_at { get; set; }

    [JsonProperty("created_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Created_at { get; set; }

    [JsonProperty("updated_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Updated_at { get; set; }

    [JsonProperty("last_fetched_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Last_fetched_at { get; set; }

    [JsonProperty("backfill_percentage", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public int Backfill_percentage { get; set; }

    [JsonProperty("reference_name", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Reference_name { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Remove a component from a page access group
  /// </summary>
  public class PageAccessGroup {
    private IDictionary<string, object> _additionalProperties;

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
    public string Page_id { get; set; }

    /// <summary>
    /// Name for this Group.
    /// </summary>
    [JsonProperty("name", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    [JsonProperty("page_access_user_ids", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> Page_access_user_ids { get; set; }

    /// <summary>
    /// Associates group with external group.
    /// </summary>
    [JsonProperty("external_identifier", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string External_identifier { get; set; }

    [JsonProperty("metric_ids", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> Metric_ids { get; set; }

    [JsonProperty("component_ids", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> Component_ids { get; set; }

    [JsonProperty("created_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Created_at { get; set; }

    [JsonProperty("updated_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Updated_at { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Create a page access group
  /// </summary>
  public class PostPagesPageIdPageAccessGroups {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("page_access_group", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Page_access_group Page_access_group { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a page access group
  /// </summary>
  public class PatchPagesPageIdPageAccessGroups {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("page_access_group", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Page_access_group2 Page_access_group { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a page access group
  /// </summary>
  public class PutPagesPageIdPageAccessGroups {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("page_access_group", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Page_access_group3 Page_access_group { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Add components to page access group
  /// </summary>
  public class PatchPagesPageIdPageAccessGroupsPageAccessGroupIdComponents {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// List of Component identifiers
    /// </summary>
    [JsonProperty("component_ids", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> Component_ids { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Add components to page access group
  /// </summary>
  public class PutPagesPageIdPageAccessGroupsPageAccessGroupIdComponents {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// List of Component identifiers
    /// </summary>
    [JsonProperty("component_ids", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> Component_ids { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Replace components for a page access group
  /// </summary>
  public class PostPagesPageIdPageAccessGroupsPageAccessGroupIdComponents {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// List of components codes to set on the page access group
    /// </summary>
    [JsonProperty("component_ids", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public ICollection<string> Component_ids { get; set; } =
        new System.Collections.ObjectModel.Collection<string>();

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Delete components for a page access group
  /// </summary>
  public class DeletePagesPageIdPageAccessGroupsPageAccessGroupIdComponents {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("component_ids", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> Component_ids { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Resend confirmations to a list of subscribers
  /// </summary>
  public class PostPagesPageIdSubscribersResendConfirmation {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// The array of subscriber codes to resend confirmations for, or "all" to resend confirmations to all subscribers. Only unconfirmed email subscribers will receive this notification.
    /// </summary>
    [JsonProperty("subscribers", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Subscribers { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Unsubscribe a list of subscribers
  /// </summary>
  public class PostPagesPageIdSubscribersUnsubscribe {
    private IDictionary<string, object> _additionalProperties;

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
    public bool Skip_unsubscription_notification { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Reactivate a list of quarantined subscribers
  /// </summary>
  public class PostPagesPageIdSubscribersReactivate {
    private IDictionary<string, object> _additionalProperties;

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
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Get a histogram of subscribers by type and then state
  /// </summary>
  public class SubscriberCountByTypeAndState {
    private IDictionary<string, object> _additionalProperties;

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
    public SubscriberCountByState Integration_partner { get; set; }

    [JsonProperty("slack", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public SubscriberCountByState Slack { get; set; }

    [JsonProperty("teams", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public SubscriberCountByState Teams { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class SubscriberCountByState {
    private IDictionary<string, object> _additionalProperties;

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
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Get a count of subscribers by type
  /// </summary>
  public class SubscriberCountByType {
    private IDictionary<string, object> _additionalProperties;

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
    public int Integration_partner { get; set; }

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
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Get an incident subscriber
  /// </summary>
  public class Subscriber {
    private IDictionary<string, object> _additionalProperties;

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
    public bool Skip_confirmation_notification { get; set; }

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
    public string Phone_number { get; set; }

    /// <summary>
    /// The two-character country code representing the country of which the phone_number is a part.
    /// </summary>
    [JsonProperty("phone_country", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Phone_country { get; set; }

    /// <summary>
    /// A formatted version of the phone_number and phone_country pair, nicely formatted for display.
    /// </summary>
    [JsonProperty("display_phone_number", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Display_phone_number { get; set; }

    /// <summary>
    /// Obfuscated slack channel name
    /// </summary>
    [JsonProperty("obfuscated_channel_name", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Obfuscated_channel_name { get; set; }

    /// <summary>
    /// The workspace name of the slack subscriber.
    /// </summary>
    [JsonProperty("workspace_name", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Workspace_name { get; set; }

    /// <summary>
    /// The timestamp when the subscriber was quarantined due to an issue reaching them.
    /// </summary>
    [JsonProperty("quarantined_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Quarantined_at { get; set; }

    /// <summary>
    /// The timestamp when a quarantined subscriber will be purged (unsubscribed).
    /// </summary>
    [JsonProperty("purge_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Purge_at { get; set; }

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
    public string Page_access_user_id { get; set; }

    [JsonProperty("created_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Created_at { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Create a subscriber. Not applicable for Slack subscribers.
  /// </summary>
  public class PostPagesPageIdSubscribers {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("subscriber", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Subscriber2 Subscriber { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a subscriber
  /// </summary>
  public class PatchPagesPageIdSubscribers {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// A list of component ids for which the subscriber should recieve updates for. Components must be an array with at least one element if it is passed at all. Each component must belong to the page indicated in the path. To set the subscriber to be subscribed to all components on the page, exclude this parameter.
    /// </summary>
    [JsonProperty("component_ids", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> Component_ids { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Create a template
  /// </summary>
  public class PostPagesPageIdIncidentTemplates {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("template", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Template Template { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Get a list of templates
  /// </summary>
  public class IncidentTemplate {
    private IDictionary<string, object> _additionalProperties;

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
    public string Group_id { get; set; }

    /// <summary>
    /// The status the incident or maintenance should transition to when selecting this template
    /// </summary>
    [JsonProperty("update_status", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public IncidentTemplateUpdate_status Update_status { get; set; }

    /// <summary>
    /// Whether the "tweet update" checkbox should be selected when selecting this template
    /// </summary>
    [JsonProperty("should_tweet", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Should_tweet { get; set; }

    /// <summary>
    /// Whether the "deliver notifications" checkbox should be selected when selecting this template
    /// </summary>
    [JsonProperty("should_send_notifications", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Should_send_notifications { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Create an incident
  /// </summary>
  public class PostPagesPageIdIncidents {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("incident", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Incident2 Incident { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Get an incident
  /// </summary>
  public class Incident {
    private IDictionary<string, object> _additionalProperties;

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
    public DateTimeOffset Created_at { get; set; }

    /// <summary>
    /// The impact of the incident.
    /// </summary>
    [JsonProperty("impact", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public IncidentImpact2 Impact { get; set; }

    /// <summary>
    /// value to override calculated impact value
    /// </summary>
    [JsonProperty("impact_override", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public IncidentImpact_override Impact_override { get; set; }

    /// <summary>
    /// The incident updates for incident.
    /// </summary>
    [JsonProperty("incident_updates", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<IncidentUpdate> Incident_updates { get; set; }

    /// <summary>
    /// The incident impacts for the incident.
    /// </summary>
    [JsonProperty("incident_impacts", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<IncidentImpact> Incident_impacts { get; set; }

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
    public DateTimeOffset Monitoring_at { get; set; }

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
    public string Page_id { get; set; }

    /// <summary>
    /// Body of the Postmortem.
    /// </summary>
    [JsonProperty("postmortem_body", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Postmortem_body { get; set; }

    /// <summary>
    /// The timestamp when the incident postmortem body was last updated at.
    /// </summary>
    [JsonProperty("postmortem_body_last_updated_at",
        Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Postmortem_body_last_updated_at { get; set; }

    /// <summary>
    /// Controls whether the incident will have postmortem.
    /// </summary>
    [JsonProperty("postmortem_ignored", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Postmortem_ignored { get; set; }

    /// <summary>
    /// Indicates whether subscribers are already notificed about postmortem.
    /// </summary>
    [JsonProperty("postmortem_notified_subscribers",
        Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Postmortem_notified_subscribers { get; set; }

    /// <summary>
    /// Controls whether to decide if notify postmortem on twitter.
    /// </summary>
    [JsonProperty("postmortem_notified_twitter", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Postmortem_notified_twitter { get; set; }

    /// <summary>
    /// The timestamp when the postmortem was published.
    /// </summary>
    [JsonProperty("postmortem_published_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Postmortem_published_at { get; set; }

    /// <summary>
    /// The timestamp when incident was resolved.
    /// </summary>
    [JsonProperty("resolved_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Resolved_at { get; set; }

    /// <summary>
    /// Controls whether the incident is scheduled to automatically change to complete.
    /// </summary>
    [JsonProperty("scheduled_auto_completed", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Scheduled_auto_completed { get; set; }

    /// <summary>
    /// Controls whether the incident is scheduled to automatically change to in progress.
    /// </summary>
    [JsonProperty("scheduled_auto_in_progress", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Scheduled_auto_in_progress { get; set; }

    /// <summary>
    /// The timestamp the incident is scheduled for.
    /// </summary>
    [JsonProperty("scheduled_for", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Scheduled_for { get; set; }

    /// <summary>
    /// Controls whether send notification when scheduled maintenances auto transition to completed.
    /// </summary>
    [JsonProperty("auto_transition_deliver_notifications_at_end",
        Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Auto_transition_deliver_notifications_at_end { get; set; }

    /// <summary>
    /// Controls whether send notification when scheduled maintenances auto transition to started.
    /// </summary>
    [JsonProperty("auto_transition_deliver_notifications_at_start",
        Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Auto_transition_deliver_notifications_at_start { get; set; }

    /// <summary>
    /// Controls whether change components status to under_maintenance once scheduled maintenance is in progress.
    /// </summary>
    [JsonProperty("auto_transition_to_maintenance_state",
        Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Auto_transition_to_maintenance_state { get; set; }

    /// <summary>
    /// Controls whether change components status to operational once scheduled maintenance completes.
    /// </summary>
    [JsonProperty("auto_transition_to_operational_state",
        Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Auto_transition_to_operational_state { get; set; }

    /// <summary>
    /// Controls whether to remind subscribers prior to scheduled incidents.
    /// </summary>
    [JsonProperty("scheduled_remind_prior", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Scheduled_remind_prior { get; set; }

    /// <summary>
    /// The timestamp when the scheduled incident reminder was sent at.
    /// </summary>
    [JsonProperty("scheduled_reminded_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Scheduled_reminded_at { get; set; }

    /// <summary>
    /// The timestamp the incident is scheduled until.
    /// </summary>
    [JsonProperty("scheduled_until", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Scheduled_until { get; set; }

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
    public DateTimeOffset Updated_at { get; set; }

    /// <summary>
    /// Custom reminder intervals for unresolved/open incidents. Not applicable for &lt;strong&gt;Scheduled maintenance&lt;/strong&gt;&lt;br&gt;There are 4 possible states for reminder_intervals:&lt;br&gt;&lt;strong&gt;DEFAULT:&lt;/strong&gt; NULL, representing a default behavior with intervals [3, 6, 12, 24].&lt;br&gt;&lt;strong&gt;AFTER:&lt;/strong&gt; A serialized array of strictly increasing intervals, each integer ranges from [1-24] (inclusive). Ex "[1, 5, 7, 10]"&lt;br&gt;&lt;strong&gt;EVERY:&lt;/strong&gt; An integer in the range [1-24] as a string, representing equal intervals. Ex "4" for [4, 8, 12, 16, 20, 24]&lt;br&gt;&lt;strong&gt;OFF:&lt;/strong&gt; A serialized empty array, for example, "[]", meaning no reminder notifications will be sent.
    /// </summary>
    [JsonProperty("reminder_intervals", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Reminder_intervals { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a previous incident update
  /// </summary>
  public class IncidentUpdate {
    private IDictionary<string, object> _additionalProperties;

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
    public string Incident_id { get; set; }

    /// <summary>
    /// Affected components associated with the incident update.
    /// </summary>
    [JsonProperty("affected_components", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<object> Affected_components { get; set; }

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
    public DateTimeOffset Created_at { get; set; }

    /// <summary>
    /// An optional customized tweet message for incident postmortem.
    /// </summary>
    [JsonProperty("custom_tweet", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Custom_tweet { get; set; }

    /// <summary>
    /// Controls whether to delivery notifications.
    /// </summary>
    [JsonProperty("deliver_notifications", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Deliver_notifications { get; set; }

    /// <summary>
    /// Timestamp when incident update is happened.
    /// </summary>
    [JsonProperty("display_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Display_at { get; set; }

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
    public string Tweet_id { get; set; }

    /// <summary>
    /// The timestamp when twitter updated at.
    /// </summary>
    [JsonProperty("twitter_updated_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Twitter_updated_at { get; set; }

    /// <summary>
    /// The timestamp when the incident update is updated.
    /// </summary>
    [JsonProperty("updated_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Updated_at { get; set; }

    /// <summary>
    /// Controls whether to create twitter update.
    /// </summary>
    [JsonProperty("wants_twitter_update", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Wants_twitter_update { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class IncidentImpact {
    private IDictionary<string, object> _additionalProperties;

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
    public string Tenant_id { get; set; }

    /// <summary>
    /// The Atlassian organization ID associated with the impact.
    /// </summary>
    [JsonProperty("atlassian_organization_id", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Atlassian_organization_id { get; set; }

    /// <summary>
    /// The product name associated with the impact.
    /// </summary>
    [JsonProperty("product_name", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Product_name { get; set; }

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
    public DateTimeOffset Created_at { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update an incident
  /// </summary>
  public class PatchPagesPageIdIncidents {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("incident", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Incident3 Incident { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update an incident
  /// </summary>
  public class PutPagesPageIdIncidents {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("incident", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Incident4 Incident { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a previous incident update
  /// </summary>
  public class PatchPagesPageIdIncidentsIncidentIdIncidentUpdates {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("incident_update", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Incident_update Incident_update { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a previous incident update
  /// </summary>
  public class PutPagesPageIdIncidentsIncidentIdIncidentUpdates {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("incident_update", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Incident_update2 Incident_update { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Create an incident subscriber
  /// </summary>
  public class PostPagesPageIdIncidentsIncidentIdSubscribers {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("subscriber", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Subscriber3 Subscriber { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Revert Postmortem
  /// </summary>
  public class Postmortem {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// Preview Key
    /// </summary>
    [JsonProperty("preview_key", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Preview_key { get; set; }

    /// <summary>
    /// Postmortem body
    /// </summary>
    [JsonProperty("body", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Body { get; set; }

    [JsonProperty("body_updated_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Body_updated_at { get; set; }

    /// <summary>
    /// Body draft
    /// </summary>
    [JsonProperty("body_draft", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Body_draft { get; set; }

    [JsonProperty("body_draft_updated_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Body_draft_updated_at { get; set; }

    [JsonProperty("published_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Published_at { get; set; }

    /// <summary>
    /// Should email subscribers be notified.
    /// </summary>
    [JsonProperty("notify_subscribers", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Notify_subscribers { get; set; }

    /// <summary>
    /// Should Twitter followers be notified.
    /// </summary>
    [JsonProperty("notify_twitter", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Notify_twitter { get; set; }

    /// <summary>
    /// Custom tweet for Incident Postmortem
    /// </summary>
    [JsonProperty("custom_tweet", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Custom_tweet { get; set; }

    [JsonProperty("created_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Created_at { get; set; }

    [JsonProperty("updated_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Updated_at { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Create Postmortem
  /// </summary>
  public class PutPagesPageIdIncidentsIncidentIdPostmortem {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("postmortem", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Postmortem2 Postmortem { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Publish Postmortem
  /// </summary>
  public class PutPagesPageIdIncidentsIncidentIdPostmortemPublish {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("postmortem", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Postmortem3 Postmortem { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Create a component
  /// </summary>
  public class PostPagesPageIdComponents {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("component", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Component2 Component { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a component
  /// </summary>
  public class PatchPagesPageIdComponents {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("component", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Component3 Component { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a component
  /// </summary>
  public class PutPagesPageIdComponents {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("component", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Component4 Component { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Get uptime data for a component that has uptime showcase enabled
  /// </summary>
  public class ComponentUptime {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// Start date used for uptime calculation (see the warnings field in the response if this value does not match the start parameter you provided).
    /// </summary>
    [JsonProperty("range_start", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Range_start { get; set; }

    /// <summary>
    /// End date used for uptime calculation (see the warnings field in the response if this value does not match the end parameter you provided).
    /// </summary>
    [JsonProperty("range_end", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Range_end { get; set; }

    /// <summary>
    /// Uptime percentage for a component
    /// </summary>
    [JsonProperty("uptime_percentage", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public float Uptime_percentage { get; set; }

    /// <summary>
    /// Seconds of major outage
    /// </summary>
    [JsonProperty("major_outage", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public int Major_outage { get; set; }

    /// <summary>
    /// Seconds of partial outage
    /// </summary>
    [JsonProperty("partial_outage", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public int Partial_outage { get; set; }

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
    public Related_events Related_events { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Create a component group
  /// </summary>
  public class PostPagesPageIdComponentGroups {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// Description of the component group.
    /// </summary>
    [JsonProperty("description", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Description { get; set; }

    [JsonProperty("component_group", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Component_group Component_group { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Get a component group
  /// </summary>
  public class GroupComponent {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// Component Group Identifier
    /// </summary>
    [JsonProperty("id", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    [JsonProperty("page_id", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Page_id { get; set; }

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
    public DateTimeOffset Created_at { get; set; }

    [JsonProperty("updated_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Updated_at { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a component group
  /// </summary>
  public class PatchPagesPageIdComponentGroups {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// Updated description of the component group.
    /// </summary>
    [JsonProperty("description", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Description { get; set; }

    [JsonProperty("component_group", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Component_group2 Component_group { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a component group
  /// </summary>
  public class PutPagesPageIdComponentGroups {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// Updated description of the component group.
    /// </summary>
    [JsonProperty("description", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Description { get; set; }

    [JsonProperty("component_group", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Component_group3 Component_group { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Get uptime data for a component group that has uptime showcase enabled for at least one component.
  /// </summary>
  public class ComponentGroupUptime {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// Start date used for uptime calculation (see the warnings field in the response if this value does not match the start parameter you provided).
    /// </summary>
    [JsonProperty("range_start", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Range_start { get; set; }

    /// <summary>
    /// End date used for uptime calculation (see the warnings field in the response if this value does not match the end parameter you provided).
    /// </summary>
    [JsonProperty("range_end", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Range_end { get; set; }

    /// <summary>
    /// Uptime percentage for a component
    /// </summary>
    [JsonProperty("uptime_percentage", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public float Uptime_percentage { get; set; }

    /// <summary>
    /// Seconds of major outage
    /// </summary>
    [JsonProperty("major_outage", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public int Major_outage { get; set; }

    /// <summary>
    /// Seconds of partial outage
    /// </summary>
    [JsonProperty("partial_outage", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public int Partial_outage { get; set; }

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
    public Related_events2 Related_events { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Add data points to metrics
  /// </summary>
  public class MetricAddResponse {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// Metric identifier to add data to
    /// </summary>
    [JsonProperty("metric_id", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<Metric_id> Metric_id { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Add data points to metrics
  /// </summary>
  public class PostPagesPageIdMetricsData {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("data", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public MetricAddResponse Data { get; set; } = new MetricAddResponse();

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a metric
  /// </summary>
  public class PatchPagesPageIdMetrics {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("metric", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Metric2 Metric { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a metric
  /// </summary>
  public class PutPagesPageIdMetrics {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("metric", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Metric3 Metric { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Add data to a metric
  /// </summary>
  public class PostPagesPageIdMetricsMetricIdData {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("data", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public Data Data { get; set; } = new Data();

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Add data to a metric
  /// </summary>
  public class SingleMetricAddResponse {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("data", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Data2 Data { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Delete a metric provider
  /// </summary>
  public class MetricsProvider {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// Identifier for Metrics Provider
    /// </summary>
    [JsonProperty("id", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    [JsonProperty("type", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Type { get; set; }

    [JsonProperty("disabled", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Disabled { get; set; }

    [JsonProperty("metric_base_uri", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Metric_base_uri { get; set; }

    [JsonProperty("last_revalidated_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Last_revalidated_at { get; set; }

    [JsonProperty("created_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Created_at { get; set; }

    [JsonProperty("updated_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Updated_at { get; set; }

    [JsonProperty("page_id", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public int Page_id { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Create a metric provider
  /// </summary>
  public class PostPagesPageIdMetricsProviders {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("metrics_provider", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Metrics_provider Metrics_provider { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a metric provider
  /// </summary>
  public class PatchPagesPageIdMetricsProviders {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("metrics_provider", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Metrics_provider2 Metrics_provider { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a metric provider
  /// </summary>
  public class PutPagesPageIdMetricsProviders {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("metrics_provider", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Metrics_provider3 Metrics_provider { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Create a metric for a metric provider
  /// </summary>
  public class PostPagesPageIdMetricsProvidersMetricsProviderIdMetrics {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("metric", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Metric4 Metric { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update status embed config settings
  /// </summary>
  public class StatusEmbedConfig {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// Page identifier
    /// </summary>
    [JsonProperty("page_id", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Page_id { get; set; }

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
    public string Incident_background_color { get; set; }

    /// <summary>
    /// Color of status embed iframe text when displaying incident
    /// </summary>
    [JsonProperty("incident_text_color", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Incident_text_color { get; set; }

    /// <summary>
    /// Color of status embed iframe background when displaying maintenance
    /// </summary>
    [JsonProperty("maintenance_background_color", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Maintenance_background_color { get; set; }

    /// <summary>
    /// Color of status embed iframe text when displaying maintenance
    /// </summary>
    [JsonProperty("maintenance_text_color", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Maintenance_text_color { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update status embed config settings
  /// </summary>
  public class PatchPagesPageIdStatusEmbedConfig {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("status_embed_config", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Status_embed_config Status_embed_config { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update status embed config settings
  /// </summary>
  public class PutPagesPageIdStatusEmbedConfig {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("status_embed_config", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Status_embed_config2 Status_embed_config { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Update a user's role permissions. Payload should contain a mapping of pages to a set of the desired roles,
  /// <br/>                  if the page has Role Based Access Control. Otherwise, the pages should map to an empty hash.
  /// <br/>                  User will lose access to any pages omitted from the payload.
  /// </summary>
  public class PutOrganizationsOrganizationIdPermissions {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("pages", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Pages Pages { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Get a user's permissions
  /// </summary>
  public class Permissions {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("data", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Data3 Data { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Get a list of users
  /// </summary>
  public class User {
    private IDictionary<string, object> _additionalProperties;

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
    public string Organization_id { get; set; }

    /// <summary>
    /// Email address for the team member
    /// </summary>
    [JsonProperty("email", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Email { get; set; }

    [JsonProperty("first_name", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string First_name { get; set; }

    [JsonProperty("last_name", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Last_name { get; set; }

    [JsonProperty("created_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Created_at { get; set; }

    [JsonProperty("updated_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Updated_at { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  /// <summary>
  /// Create a user
  /// </summary>
  public class PostOrganizationsOrganizationIdUsers {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("user", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public User2 User { get; set; } = new User2();

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
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
    Integration_partner = 5,

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

  public enum Type2 {

    [System.Runtime.Serialization.EnumMember(Value = @"email")]
    Email = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"sms")]
    Sms = 1,

    [System.Runtime.Serialization.EnumMember(Value = @"webhook")]
    Webhook = 2,

    [System.Runtime.Serialization.EnumMember(Value = @"slack")]
    Slack = 3,

    [System.Runtime.Serialization.EnumMember(Value = @"teams")]
    Teams = 4,

    [System.Runtime.Serialization.EnumMember(Value = @"integration_partner")]
    Integration_partner = 5,

  }

  public enum State2 {

    [System.Runtime.Serialization.EnumMember(Value = @"active")]
    Active = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"unconfirmed")]
    Unconfirmed = 1,

    [System.Runtime.Serialization.EnumMember(Value = @"quarantined")]
    Quarantined = 2,

    [System.Runtime.Serialization.EnumMember(Value = @"all")]
    All = 3,

  }

  public enum Sort_field {

    [System.Runtime.Serialization.EnumMember(Value = @"primary")]
    Primary = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"created_at")]
    Created_at = 1,

    [System.Runtime.Serialization.EnumMember(Value = @"quarantined_at")]
    Quarantined_at = 2,

    [System.Runtime.Serialization.EnumMember(Value = @"relevance")]
    Relevance = 3,

  }

  public enum Sort_direction {

    [System.Runtime.Serialization.EnumMember(Value = @"asc")]
    Asc = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"desc")]
    Desc = 1,

  }

  public class Body {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// List of page access users to add to component
    /// </summary>
    [JsonProperty("page_access_user_ids", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required]
    public ICollection<string> Page_access_user_ids { get; set; } =
        new System.Collections.ObjectModel.Collection<string>();

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Page2 {
    private IDictionary<string, object> _additionalProperties;

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
    public string Css_body_background_color { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_font_color", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_font_color { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_light_font_color", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_light_font_color { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_greens", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_greens { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_yellows", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_yellows { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_oranges", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_oranges { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_reds", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_reds { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_blues", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_blues { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_border_color", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_border_color { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_graph_color", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_graph_color { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_link_color", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_link_color { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_no_data", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_no_data { get; set; }

    /// <summary>
    /// Should your page hide itself from search engines
    /// </summary>
    [JsonProperty("hidden_from_search", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Hidden_from_search { get; set; }

    [JsonProperty("viewers_must_be_team_members", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Viewers_must_be_team_members { get; set; }

    /// <summary>
    /// Can your users subscribe to all notifications on the page
    /// </summary>
    [JsonProperty("allow_page_subscribers", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Allow_page_subscribers { get; set; }

    /// <summary>
    /// Can your users subscribe to notifications for a single incident
    /// </summary>
    [JsonProperty("allow_incident_subscribers", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Allow_incident_subscribers { get; set; }

    /// <summary>
    /// Can your users choose to receive notifications via email
    /// </summary>
    [JsonProperty("allow_email_subscribers", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Allow_email_subscribers { get; set; }

    /// <summary>
    /// Can your users choose to receive notifications via SMS
    /// </summary>
    [JsonProperty("allow_sms_subscribers", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Allow_sms_subscribers { get; set; }

    /// <summary>
    /// Can your users choose to access incident feeds via RSS/Atom (not functional on Audience-Specific pages)
    /// </summary>
    [JsonProperty("allow_rss_atom_feeds", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Allow_rss_atom_feeds { get; set; }

    /// <summary>
    /// Can your users choose to receive notifications via Webhooks
    /// </summary>
    [JsonProperty("allow_webhook_subscribers", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Allow_webhook_subscribers { get; set; }

    /// <summary>
    /// Allows you to customize the email address your page notifications come from
    /// </summary>
    [JsonProperty("notifications_from_email", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Notifications_from_email { get; set; }

    /// <summary>
    /// Timezone configured for your page
    /// </summary>
    [JsonProperty("time_zone", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Time_zone { get; set; }

    /// <summary>
    /// Allows you to customize the footer appearing on your notification emails.  Accepts Markdown for formatting
    /// </summary>
    [JsonProperty("notifications_email_footer", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Notifications_email_footer { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Page3 {
    private IDictionary<string, object> _additionalProperties;

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
    public string Css_body_background_color { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_font_color", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_font_color { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_light_font_color", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_light_font_color { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_greens", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_greens { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_yellows", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_yellows { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_oranges", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_oranges { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_reds", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_reds { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_blues", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_blues { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_border_color", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_border_color { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_graph_color", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_graph_color { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_link_color", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_link_color { get; set; }

    /// <summary>
    /// CSS Color
    /// </summary>
    [JsonProperty("css_no_data", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Css_no_data { get; set; }

    /// <summary>
    /// Should your page hide itself from search engines
    /// </summary>
    [JsonProperty("hidden_from_search", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Hidden_from_search { get; set; }

    [JsonProperty("viewers_must_be_team_members", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Viewers_must_be_team_members { get; set; }

    /// <summary>
    /// Can your users subscribe to all notifications on the page
    /// </summary>
    [JsonProperty("allow_page_subscribers", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Allow_page_subscribers { get; set; }

    /// <summary>
    /// Can your users subscribe to notifications for a single incident
    /// </summary>
    [JsonProperty("allow_incident_subscribers", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Allow_incident_subscribers { get; set; }

    /// <summary>
    /// Can your users choose to receive notifications via email
    /// </summary>
    [JsonProperty("allow_email_subscribers", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Allow_email_subscribers { get; set; }

    /// <summary>
    /// Can your users choose to receive notifications via SMS
    /// </summary>
    [JsonProperty("allow_sms_subscribers", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Allow_sms_subscribers { get; set; }

    /// <summary>
    /// Can your users choose to access incident feeds via RSS/Atom (not functional on Audience-Specific pages)
    /// </summary>
    [JsonProperty("allow_rss_atom_feeds", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Allow_rss_atom_feeds { get; set; }

    /// <summary>
    /// Can your users choose to receive notifications via Webhooks
    /// </summary>
    [JsonProperty("allow_webhook_subscribers", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Allow_webhook_subscribers { get; set; }

    /// <summary>
    /// Allows you to customize the email address your page notifications come from
    /// </summary>
    [JsonProperty("notifications_from_email", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Notifications_from_email { get; set; }

    /// <summary>
    /// Timezone configured for your page
    /// </summary>
    [JsonProperty("time_zone", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Time_zone { get; set; }

    /// <summary>
    /// Allows you to customize the footer appearing on your notification emails.  Accepts Markdown for formatting
    /// </summary>
    [JsonProperty("notifications_email_footer", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Notifications_email_footer { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Page_access_user {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// IDP login user id. Key is typically "uid".
    /// </summary>
    [JsonProperty("external_login", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string External_login { get; set; }

    [JsonProperty("email", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Email { get; set; }

    [JsonProperty("page_access_group_ids", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> Page_access_group_ids { get; set; }

    [JsonProperty("subscribe_to_components", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Subscribe_to_components { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public enum ComponentStatus {

    [System.Runtime.Serialization.EnumMember(Value = @"operational")]
    Operational = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"under_maintenance")]
    Under_maintenance = 1,

    [System.Runtime.Serialization.EnumMember(Value = @"degraded_performance")]
    Degraded_performance = 2,

    [System.Runtime.Serialization.EnumMember(Value = @"partial_outage")]
    Partial_outage = 3,

    [System.Runtime.Serialization.EnumMember(Value = @"major_outage")]
    Major_outage = 4,

    [System.Runtime.Serialization.EnumMember(Value = @"")]
    Empty = 5,

  }

  public class Page_access_group {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// Name for this Group.
    /// </summary>
    [JsonProperty("name", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    /// <summary>
    /// Associates group with external group.
    /// </summary>
    [JsonProperty("external_identifier", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string External_identifier { get; set; }

    [JsonProperty("component_ids", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> Component_ids { get; set; }

    [JsonProperty("metric_ids", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> Metric_ids { get; set; }

    [JsonProperty("page_access_user_ids", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> Page_access_user_ids { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Page_access_group2 {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// Name for this Group.
    /// </summary>
    [JsonProperty("name", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    /// <summary>
    /// Associates group with external group.
    /// </summary>
    [JsonProperty("external_identifier", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string External_identifier { get; set; }

    [JsonProperty("component_ids", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> Component_ids { get; set; }

    [JsonProperty("metric_ids", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> Metric_ids { get; set; }

    [JsonProperty("page_access_user_ids", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> Page_access_user_ids { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Page_access_group3 {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// Name for this Group.
    /// </summary>
    [JsonProperty("name", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    /// <summary>
    /// Associates group with external group.
    /// </summary>
    [JsonProperty("external_identifier", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string External_identifier { get; set; }

    [JsonProperty("component_ids", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> Component_ids { get; set; }

    [JsonProperty("metric_ids", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> Metric_ids { get; set; }

    [JsonProperty("page_access_user_ids", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> Page_access_user_ids { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
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
    Integration_partner = 5,

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
    Integration_partner = 4,

  }

  public class Subscriber2 {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// The email address for creating Email and Webhook subscribers.
    /// </summary>
    [JsonProperty("email", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Email { get; set; }

    /// <summary>
    /// The endpoint URI for creating Webhook subscribers.
    /// </summary>
    [JsonProperty("endpoint", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Endpoint { get; set; }

    /// <summary>
    /// The two-character country where the phone number is located to use for the new SMS subscriber.
    /// </summary>
    [JsonProperty("phone_country", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Phone_country { get; set; }

    /// <summary>
    /// The phone number (as you would dial from the phone_country) to use for the new SMS subscriber.
    /// </summary>
    [JsonProperty("phone_number", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Phone_number { get; set; }

    /// <summary>
    /// If skip_confirmation_notification is true, the subscriber does not receive any notifications when their subscription changes.  Email subscribers will be automatically opted in. This option is only available for paid pages. This option has no effect for trial customers.
    /// </summary>
    [JsonProperty("skip_confirmation_notification",
        Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Skip_confirmation_notification { get; set; }

    /// <summary>
    /// The code of the page access user to which the subscriber belongs.
    /// </summary>
    [JsonProperty("page_access_user", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Page_access_user { get; set; }

    /// <summary>
    /// A list of component ids for which the subscriber should recieve updates for. Components must be an array with at least one element if it is passed at all. Each component must belong to the page indicated in the path.
    /// </summary>
    [JsonProperty("component_ids", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> Component_ids { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Template {
    private IDictionary<string, object> _additionalProperties;

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
    public string Group_id { get; set; }

    /// <summary>
    /// The status the incident or maintenance should transition to when selecting this template
    /// </summary>
    [JsonProperty("update_status", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public TemplateUpdate_status Update_status { get; set; }

    /// <summary>
    /// Whether the "tweet update" checkbox should be selected when selecting this template
    /// </summary>
    [JsonProperty("should_tweet", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Should_tweet { get; set; }

    /// <summary>
    /// Whether the "deliver notifications" checkbox should be selected when selecting this template
    /// </summary>
    [JsonProperty("should_send_notifications", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Should_send_notifications { get; set; }

    /// <summary>
    /// List of component_ids affected by this incident
    /// </summary>
    [JsonProperty("component_ids", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> Component_ids { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public enum IncidentTemplateUpdate_status {

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
    In_progress = 5,

    [System.Runtime.Serialization.EnumMember(Value = @"verifying")]
    Verifying = 6,

    [System.Runtime.Serialization.EnumMember(Value = @"completed")]
    Completed = 7,

  }

  public class Incident2 {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// Incident Name. There is a maximum limit of 255 characters.
    /// </summary>
    [JsonProperty("name", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Name { get; set; }

    /// <summary>
    /// The incident status. For realtime incidents, valid values are investigating, identified, monitoring, and resolved. For scheduled incidents, valid values are scheduled, in_progress, verifying, and completed.
    /// </summary>
    [JsonProperty("status", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public Incident2Status Status { get; set; }

    /// <summary>
    /// value to override calculated impact value
    /// </summary>
    [JsonProperty("impact_override", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public Incident2Impact_override Impact_override { get; set; }

    /// <summary>
    /// The timestamp the incident is scheduled for.
    /// </summary>
    [JsonProperty("scheduled_for", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Scheduled_for { get; set; }

    /// <summary>
    /// The timestamp the incident is scheduled until.
    /// </summary>
    [JsonProperty("scheduled_until", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Scheduled_until { get; set; }

    /// <summary>
    /// Controls whether to remind subscribers prior to scheduled incidents.
    /// </summary>
    [JsonProperty("scheduled_remind_prior", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Scheduled_remind_prior { get; set; }

    /// <summary>
    /// Controls whether change components status to under_maintenance once scheduled maintenance is in progress.
    /// </summary>
    [JsonProperty("auto_transition_to_maintenance_state",
        Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Auto_transition_to_maintenance_state { get; set; }

    /// <summary>
    /// Controls whether change components status to operational once scheduled maintenance completes.
    /// </summary>
    [JsonProperty("auto_transition_to_operational_state",
        Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Auto_transition_to_operational_state { get; set; }

    /// <summary>
    /// Controls whether the incident is scheduled to automatically change to in progress.
    /// </summary>
    [JsonProperty("scheduled_auto_in_progress", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Scheduled_auto_in_progress { get; set; }

    /// <summary>
    /// Controls whether the incident is scheduled to automatically change to complete.
    /// </summary>
    [JsonProperty("scheduled_auto_completed", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Scheduled_auto_completed { get; set; }

    /// <summary>
    /// Controls whether send notification when scheduled maintenances auto transition to started.
    /// </summary>
    [JsonProperty("auto_transition_deliver_notifications_at_start",
        Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Auto_transition_deliver_notifications_at_start { get; set; }

    /// <summary>
    /// Controls whether send notification when scheduled maintenances auto transition to completed.
    /// </summary>
    [JsonProperty("auto_transition_deliver_notifications_at_end",
        Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Auto_transition_deliver_notifications_at_end { get; set; }

    /// <summary>
    /// Custom reminder intervals for unresolved/open incidents. Not applicable for &lt;strong&gt;Scheduled maintenance&lt;/strong&gt;&lt;br&gt;There are 4 possible states for reminder_intervals:&lt;br&gt;&lt;strong&gt;DEFAULT:&lt;/strong&gt; NULL, representing a default behavior with intervals [3, 6, 12, 24].&lt;br&gt;&lt;strong&gt;AFTER:&lt;/strong&gt; A serialized array of strictly increasing intervals, each integer ranges from [1-24] (inclusive). Ex "[1, 5, 7, 10]"&lt;br&gt;&lt;strong&gt;EVERY:&lt;/strong&gt; An integer in the range [1-24] as a string, representing equal intervals. Ex "4" for [4, 8, 12, 16, 20, 24]&lt;br&gt;&lt;strong&gt;OFF:&lt;/strong&gt; A serialized empty array, for example, "[]", meaning no reminder notifications will be sent.
    /// </summary>
    [JsonProperty("reminder_intervals", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Reminder_intervals { get; set; }

    /// <summary>
    /// Attach a json object to the incident. All top-level values in the object must also be objects.
    /// </summary>
    [JsonProperty("metadata", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public object Metadata { get; set; }

    /// <summary>
    /// Deliver notifications to subscribers if this is true. If this is false, create an incident without notifying customers.
    /// </summary>
    [JsonProperty("deliver_notifications", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Deliver_notifications { get; set; } = true;

    /// <summary>
    /// Controls whether tweet automatically when scheduled maintenance starts.
    /// </summary>
    [JsonProperty("auto_tweet_at_beginning", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Auto_tweet_at_beginning { get; set; }

    /// <summary>
    /// Controls whether tweet automatically when scheduled maintenance completes.
    /// </summary>
    [JsonProperty("auto_tweet_on_completion", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Auto_tweet_on_completion { get; set; }

    /// <summary>
    /// Controls whether tweet automatically when scheduled maintenance is created.
    /// </summary>
    [JsonProperty("auto_tweet_on_creation", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Auto_tweet_on_creation { get; set; }

    /// <summary>
    /// Controls whether tweet automatically one hour before scheduled maintenance starts.
    /// </summary>
    [JsonProperty("auto_tweet_one_hour_before", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Auto_tweet_one_hour_before { get; set; }

    /// <summary>
    /// TimeStamp when incident was backfilled.
    /// </summary>
    [JsonProperty("backfill_date", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Backfill_date { get; set; }

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
    /// Map of status changes to apply to affected components
    /// </summary>
    [JsonProperty("components", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Components Components { get; set; }

    /// <summary>
    /// List of component_ids affected by this incident
    /// </summary>
    [JsonProperty("component_ids", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> Component_ids { get; set; }

    /// <summary>
    /// Same as :scheduled_auto_transition_in_progress. Controls whether the incident is scheduled to automatically change to in progress.
    /// </summary>
    [JsonProperty("scheduled_auto_transition", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Scheduled_auto_transition { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public enum IncidentImpact2 {

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

  public enum IncidentImpact_override {

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
    In_progress = 5,

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
    In_progress = 5,

    [System.Runtime.Serialization.EnumMember(Value = @"verifying")]
    Verifying = 6,

    [System.Runtime.Serialization.EnumMember(Value = @"completed")]
    Completed = 7,

  }

  public class Incident3 {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// Incident Name. There is a maximum limit of 255 characters.
    /// </summary>
    [JsonProperty("name", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    /// <summary>
    /// The incident status. For realtime incidents, valid values are investigating, identified, monitoring, and resolved. For scheduled incidents, valid values are scheduled, in_progress, verifying, and completed.
    /// </summary>
    [JsonProperty("status", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public Incident3Status Status { get; set; }

    /// <summary>
    /// value to override calculated impact value
    /// </summary>
    [JsonProperty("impact_override", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public Incident3Impact_override Impact_override { get; set; }

    /// <summary>
    /// The timestamp the incident is scheduled for.
    /// </summary>
    [JsonProperty("scheduled_for", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Scheduled_for { get; set; }

    /// <summary>
    /// The timestamp the incident is scheduled until.
    /// </summary>
    [JsonProperty("scheduled_until", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Scheduled_until { get; set; }

    /// <summary>
    /// Controls whether to remind subscribers prior to scheduled incidents.
    /// </summary>
    [JsonProperty("scheduled_remind_prior", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Scheduled_remind_prior { get; set; }

    /// <summary>
    /// Controls whether change components status to under_maintenance once scheduled maintenance is in progress.
    /// </summary>
    [JsonProperty("auto_transition_to_maintenance_state",
        Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Auto_transition_to_maintenance_state { get; set; }

    /// <summary>
    /// Controls whether change components status to operational once scheduled maintenance completes.
    /// </summary>
    [JsonProperty("auto_transition_to_operational_state",
        Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Auto_transition_to_operational_state { get; set; }

    /// <summary>
    /// Controls whether the incident is scheduled to automatically change to in progress.
    /// </summary>
    [JsonProperty("scheduled_auto_in_progress", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Scheduled_auto_in_progress { get; set; }

    /// <summary>
    /// Controls whether the incident is scheduled to automatically change to complete.
    /// </summary>
    [JsonProperty("scheduled_auto_completed", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Scheduled_auto_completed { get; set; }

    /// <summary>
    /// Controls whether send notification when scheduled maintenances auto transition to started.
    /// </summary>
    [JsonProperty("auto_transition_deliver_notifications_at_start",
        Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Auto_transition_deliver_notifications_at_start { get; set; }

    /// <summary>
    /// Controls whether send notification when scheduled maintenances auto transition to completed.
    /// </summary>
    [JsonProperty("auto_transition_deliver_notifications_at_end",
        Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Auto_transition_deliver_notifications_at_end { get; set; }

    /// <summary>
    /// Custom reminder intervals for unresolved/open incidents. Not applicable for &lt;strong&gt;Scheduled maintenance&lt;/strong&gt;&lt;br&gt;There are 4 possible states for reminder_intervals:&lt;br&gt;&lt;strong&gt;DEFAULT:&lt;/strong&gt; NULL, representing a default behavior with intervals [3, 6, 12, 24].&lt;br&gt;&lt;strong&gt;AFTER:&lt;/strong&gt; A serialized array of strictly increasing intervals, each integer ranges from [1-24] (inclusive). Ex "[1, 5, 7, 10]"&lt;br&gt;&lt;strong&gt;EVERY:&lt;/strong&gt; An integer in the range [1-24] as a string, representing equal intervals. Ex "4" for [4, 8, 12, 16, 20, 24]&lt;br&gt;&lt;strong&gt;OFF:&lt;/strong&gt; A serialized empty array, for example, "[]", meaning no reminder notifications will be sent.
    /// </summary>
    [JsonProperty("reminder_intervals", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Reminder_intervals { get; set; }

    /// <summary>
    /// Attach a json object to the incident. All top-level values in the object must also be objects.
    /// </summary>
    [JsonProperty("metadata", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public object Metadata { get; set; }

    /// <summary>
    /// Deliver notifications to subscribers if this is true. If this is false, create an incident without notifying customers.
    /// </summary>
    [JsonProperty("deliver_notifications", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Deliver_notifications { get; set; } = true;

    /// <summary>
    /// Controls whether tweet automatically when scheduled maintenance starts.
    /// </summary>
    [JsonProperty("auto_tweet_at_beginning", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Auto_tweet_at_beginning { get; set; }

    /// <summary>
    /// Controls whether tweet automatically when scheduled maintenance completes.
    /// </summary>
    [JsonProperty("auto_tweet_on_completion", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Auto_tweet_on_completion { get; set; }

    /// <summary>
    /// Controls whether tweet automatically when scheduled maintenance is created.
    /// </summary>
    [JsonProperty("auto_tweet_on_creation", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Auto_tweet_on_creation { get; set; }

    /// <summary>
    /// Controls whether tweet automatically one hour before scheduled maintenance starts.
    /// </summary>
    [JsonProperty("auto_tweet_one_hour_before", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Auto_tweet_one_hour_before { get; set; }

    /// <summary>
    /// TimeStamp when incident was backfilled.
    /// </summary>
    [JsonProperty("backfill_date", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Backfill_date { get; set; }

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
    /// Map of status changes to apply to affected components
    /// </summary>
    [JsonProperty("components", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Components2 Components { get; set; }

    /// <summary>
    /// List of component_ids affected by this incident
    /// </summary>
    [JsonProperty("component_ids", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> Component_ids { get; set; }

    /// <summary>
    /// Same as :scheduled_auto_transition_in_progress. Controls whether the incident is scheduled to automatically change to in progress.
    /// </summary>
    [JsonProperty("scheduled_auto_transition", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Scheduled_auto_transition { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Incident4 {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// Incident Name. There is a maximum limit of 255 characters.
    /// </summary>
    [JsonProperty("name", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    /// <summary>
    /// The incident status. For realtime incidents, valid values are investigating, identified, monitoring, and resolved. For scheduled incidents, valid values are scheduled, in_progress, verifying, and completed.
    /// </summary>
    [JsonProperty("status", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public Incident4Status Status { get; set; }

    /// <summary>
    /// value to override calculated impact value
    /// </summary>
    [JsonProperty("impact_override", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public Incident4Impact_override Impact_override { get; set; }

    /// <summary>
    /// The timestamp the incident is scheduled for.
    /// </summary>
    [JsonProperty("scheduled_for", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Scheduled_for { get; set; }

    /// <summary>
    /// The timestamp the incident is scheduled until.
    /// </summary>
    [JsonProperty("scheduled_until", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Scheduled_until { get; set; }

    /// <summary>
    /// Controls whether to remind subscribers prior to scheduled incidents.
    /// </summary>
    [JsonProperty("scheduled_remind_prior", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Scheduled_remind_prior { get; set; }

    /// <summary>
    /// Controls whether change components status to under_maintenance once scheduled maintenance is in progress.
    /// </summary>
    [JsonProperty("auto_transition_to_maintenance_state",
        Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Auto_transition_to_maintenance_state { get; set; }

    /// <summary>
    /// Controls whether change components status to operational once scheduled maintenance completes.
    /// </summary>
    [JsonProperty("auto_transition_to_operational_state",
        Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Auto_transition_to_operational_state { get; set; }

    /// <summary>
    /// Controls whether the incident is scheduled to automatically change to in progress.
    /// </summary>
    [JsonProperty("scheduled_auto_in_progress", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Scheduled_auto_in_progress { get; set; }

    /// <summary>
    /// Controls whether the incident is scheduled to automatically change to complete.
    /// </summary>
    [JsonProperty("scheduled_auto_completed", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Scheduled_auto_completed { get; set; }

    /// <summary>
    /// Controls whether send notification when scheduled maintenances auto transition to started.
    /// </summary>
    [JsonProperty("auto_transition_deliver_notifications_at_start",
        Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Auto_transition_deliver_notifications_at_start { get; set; }

    /// <summary>
    /// Controls whether send notification when scheduled maintenances auto transition to completed.
    /// </summary>
    [JsonProperty("auto_transition_deliver_notifications_at_end",
        Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Auto_transition_deliver_notifications_at_end { get; set; }

    /// <summary>
    /// Custom reminder intervals for unresolved/open incidents. Not applicable for &lt;strong&gt;Scheduled maintenance&lt;/strong&gt;&lt;br&gt;There are 4 possible states for reminder_intervals:&lt;br&gt;&lt;strong&gt;DEFAULT:&lt;/strong&gt; NULL, representing a default behavior with intervals [3, 6, 12, 24].&lt;br&gt;&lt;strong&gt;AFTER:&lt;/strong&gt; A serialized array of strictly increasing intervals, each integer ranges from [1-24] (inclusive). Ex "[1, 5, 7, 10]"&lt;br&gt;&lt;strong&gt;EVERY:&lt;/strong&gt; An integer in the range [1-24] as a string, representing equal intervals. Ex "4" for [4, 8, 12, 16, 20, 24]&lt;br&gt;&lt;strong&gt;OFF:&lt;/strong&gt; A serialized empty array, for example, "[]", meaning no reminder notifications will be sent.
    /// </summary>
    [JsonProperty("reminder_intervals", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Reminder_intervals { get; set; }

    /// <summary>
    /// Attach a json object to the incident. All top-level values in the object must also be objects.
    /// </summary>
    [JsonProperty("metadata", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public object Metadata { get; set; }

    /// <summary>
    /// Deliver notifications to subscribers if this is true. If this is false, create an incident without notifying customers.
    /// </summary>
    [JsonProperty("deliver_notifications", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Deliver_notifications { get; set; } = true;

    /// <summary>
    /// Controls whether tweet automatically when scheduled maintenance starts.
    /// </summary>
    [JsonProperty("auto_tweet_at_beginning", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Auto_tweet_at_beginning { get; set; }

    /// <summary>
    /// Controls whether tweet automatically when scheduled maintenance completes.
    /// </summary>
    [JsonProperty("auto_tweet_on_completion", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Auto_tweet_on_completion { get; set; }

    /// <summary>
    /// Controls whether tweet automatically when scheduled maintenance is created.
    /// </summary>
    [JsonProperty("auto_tweet_on_creation", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Auto_tweet_on_creation { get; set; }

    /// <summary>
    /// Controls whether tweet automatically one hour before scheduled maintenance starts.
    /// </summary>
    [JsonProperty("auto_tweet_one_hour_before", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Auto_tweet_one_hour_before { get; set; }

    /// <summary>
    /// TimeStamp when incident was backfilled.
    /// </summary>
    [JsonProperty("backfill_date", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Backfill_date { get; set; }

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
    /// Map of status changes to apply to affected components
    /// </summary>
    [JsonProperty("components", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Components3 Components { get; set; }

    /// <summary>
    /// List of component_ids affected by this incident
    /// </summary>
    [JsonProperty("component_ids", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public ICollection<string> Component_ids { get; set; }

    /// <summary>
    /// Same as :scheduled_auto_transition_in_progress. Controls whether the incident is scheduled to automatically change to in progress.
    /// </summary>
    [JsonProperty("scheduled_auto_transition", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Scheduled_auto_transition { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Incident_update {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// Controls whether to create twitter update.
    /// </summary>
    [JsonProperty("wants_twitter_update", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Wants_twitter_update { get; set; }

    /// <summary>
    /// Incident update body.
    /// </summary>
    [JsonProperty("body", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Body { get; set; }

    /// <summary>
    /// Timestamp when incident update is happened.
    /// </summary>
    [JsonProperty("display_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Display_at { get; set; }

    /// <summary>
    /// Controls whether to delivery notifications.
    /// </summary>
    [JsonProperty("deliver_notifications", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Deliver_notifications { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Incident_update2 {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// Controls whether to create twitter update.
    /// </summary>
    [JsonProperty("wants_twitter_update", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Wants_twitter_update { get; set; }

    /// <summary>
    /// Incident update body.
    /// </summary>
    [JsonProperty("body", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Body { get; set; }

    /// <summary>
    /// Timestamp when incident update is happened.
    /// </summary>
    [JsonProperty("display_at", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public DateTimeOffset Display_at { get; set; }

    /// <summary>
    /// Controls whether to delivery notifications.
    /// </summary>
    [JsonProperty("deliver_notifications", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Deliver_notifications { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Subscriber3 {
    private IDictionary<string, object> _additionalProperties;

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
    public string Phone_country { get; set; }

    /// <summary>
    /// The phone number (as you would dial from the phone_country) to use for the new SMS subscriber.
    /// </summary>
    [JsonProperty("phone_number", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Phone_number { get; set; }

    /// <summary>
    /// If skip_confirmation_notification is true, the subscriber does not receive any notifications when their subscription changes. Email subscribers will be automatically opted in. This option is only available for paid pages. This option has no effect for trial customers.
    /// </summary>
    [JsonProperty("skip_confirmation_notification",
        Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Skip_confirmation_notification { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Postmortem2 {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// Body of Postmortem to create.
    /// </summary>
    [JsonProperty("body_draft", Required = Required.Always)]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Body_draft { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Postmortem3 {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// Whether to notify Twitter followers
    /// </summary>
    [JsonProperty("notify_twitter", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Notify_twitter { get; set; }

    /// <summary>
    /// Whether to notify e-mail subscribers
    /// </summary>
    [JsonProperty("notify_subscribers", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Notify_subscribers { get; set; }

    /// <summary>
    /// Custom postmortem tweet to publish
    /// </summary>
    [JsonProperty("custom_tweet", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Custom_tweet { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Component2 {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// More detailed description for component
    /// </summary>
    [JsonProperty("description", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Description { get; set; }

    /// <summary>
    /// Status of component
    /// </summary>
    [JsonProperty("status", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public Component2Status Status { get; set; }

    /// <summary>
    /// Display name for component
    /// </summary>
    [JsonProperty("name", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    /// <summary>
    /// Requires a special feature flag to be enabled
    /// </summary>
    [JsonProperty("only_show_if_degraded", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Only_show_if_degraded { get; set; }

    /// <summary>
    /// Component Group identifier
    /// </summary>
    [JsonProperty("group_id", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Group_id { get; set; }

    /// <summary>
    /// Should this component be showcased
    /// </summary>
    [JsonProperty("showcase", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Showcase { get; set; }

    /// <summary>
    /// The date this component started being used
    /// </summary>
    [JsonProperty("start_date", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(DateFormatConverter))]
    public DateTimeOffset Start_date { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Component3 {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// More detailed description for component
    /// </summary>
    [JsonProperty("description", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Description { get; set; }

    /// <summary>
    /// Status of component
    /// </summary>
    [JsonProperty("status", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public Component3Status Status { get; set; }

    /// <summary>
    /// Display name for component
    /// </summary>
    [JsonProperty("name", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    /// <summary>
    /// Requires a special feature flag to be enabled
    /// </summary>
    [JsonProperty("only_show_if_degraded", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Only_show_if_degraded { get; set; }

    /// <summary>
    /// Component Group identifier
    /// </summary>
    [JsonProperty("group_id", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Group_id { get; set; }

    /// <summary>
    /// Should this component be showcased
    /// </summary>
    [JsonProperty("showcase", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Showcase { get; set; }

    /// <summary>
    /// The date this component started being used
    /// </summary>
    [JsonProperty("start_date", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(DateFormatConverter))]
    public DateTimeOffset Start_date { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Component4 {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// More detailed description for component
    /// </summary>
    [JsonProperty("description", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Description { get; set; }

    /// <summary>
    /// Status of component
    /// </summary>
    [JsonProperty("status", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public Component4Status Status { get; set; }

    /// <summary>
    /// Display name for component
    /// </summary>
    [JsonProperty("name", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    /// <summary>
    /// Requires a special feature flag to be enabled
    /// </summary>
    [JsonProperty("only_show_if_degraded", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Only_show_if_degraded { get; set; }

    /// <summary>
    /// Component Group identifier
    /// </summary>
    [JsonProperty("group_id", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Group_id { get; set; }

    /// <summary>
    /// Should this component be showcased
    /// </summary>
    [JsonProperty("showcase", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Showcase { get; set; }

    /// <summary>
    /// The date this component started being used
    /// </summary>
    [JsonProperty("start_date", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(DateFormatConverter))]
    public DateTimeOffset Start_date { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Related_events {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// Incident identifier
    /// </summary>
    [JsonProperty("id", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Component_group {
    private IDictionary<string, object> _additionalProperties;

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
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Component_group2 {
    private IDictionary<string, object> _additionalProperties;

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
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Component_group3 {
    private IDictionary<string, object> _additionalProperties;

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
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Related_events2 {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// Component identifier
    /// </summary>
    [JsonProperty("component_id", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Component_id { get; set; }

    /// <summary>
    /// Related incidents
    /// </summary>
    [JsonProperty("incidents", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Incidents Incidents { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Metric_id {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("timestamp", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public int Timestamp { get; set; }

    [JsonProperty("value", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public float Value { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Metric2 {
    private IDictionary<string, object> _additionalProperties;

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
    public string Metric_identifier { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Metric3 {
    private IDictionary<string, object> _additionalProperties;

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
    public string Metric_identifier { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Data {
    private IDictionary<string, object> _additionalProperties;

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
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Data2 {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("timestamp", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public int Timestamp { get; set; }

    [JsonProperty("value", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public float Value { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Metrics_provider {
    private IDictionary<string, object> _additionalProperties;

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
    public string Api_key { get; set; }

    /// <summary>
    /// Required by the Librato, Datadog and Pingdom type metrics providers.
    /// </summary>
    [JsonProperty("api_token", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Api_token { get; set; }

    /// <summary>
    /// Required by the Pingdom-type metrics provider.
    /// </summary>
    [JsonProperty("application_key", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Application_key { get; set; }

    /// <summary>
    /// One of "Pingdom", "NewRelic", "Librato", "Datadog", or "Self"
    /// </summary>
    [JsonProperty("type", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Type { get; set; }

    /// <summary>
    /// Required by the Datadog and NewRelic type metrics providers.
    /// </summary>
    [JsonProperty("metric_base_uri", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Metric_base_uri { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Metrics_provider2 {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("type", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Type { get; set; }

    [JsonProperty("metric_base_uri", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Metric_base_uri { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Metrics_provider3 {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("type", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Type { get; set; }

    [JsonProperty("metric_base_uri", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Metric_base_uri { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Metric4 {
    private IDictionary<string, object> _additionalProperties;

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
    public string Metric_identifier { get; set; }

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
    public string Application_id { get; set; }

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
    public int Y_axis_min { get; set; }

    /// <summary>
    /// The upper bound of the y axis
    /// </summary>
    [JsonProperty("y_axis_max", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public int Y_axis_max { get; set; }

    /// <summary>
    /// Should the values on the y axis be hidden on render
    /// </summary>
    [JsonProperty("y_axis_hidden", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Y_axis_hidden { get; set; }

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
    public int Decimal_places { get; set; }

    [JsonProperty("tooltip_description", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Tooltip_description { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Status_embed_config {
    private IDictionary<string, object> _additionalProperties;

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
    public string Incident_background_color { get; set; }

    /// <summary>
    /// Color of status embed iframe text when displaying incident
    /// </summary>
    [JsonProperty("incident_text_color", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Incident_text_color { get; set; }

    /// <summary>
    /// Color of status embed iframe background when displaying maintenance
    /// </summary>
    [JsonProperty("maintenance_background_color", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Maintenance_background_color { get; set; }

    /// <summary>
    /// Color of status embed iframe text when displaying maintenance
    /// </summary>
    [JsonProperty("maintenance_text_color", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Maintenance_text_color { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Status_embed_config2 {
    private IDictionary<string, object> _additionalProperties;

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
    public string Incident_background_color { get; set; }

    /// <summary>
    /// Color of status embed iframe text when displaying incident
    /// </summary>
    [JsonProperty("incident_text_color", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Incident_text_color { get; set; }

    /// <summary>
    /// Color of status embed iframe background when displaying maintenance
    /// </summary>
    [JsonProperty("maintenance_background_color", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Maintenance_background_color { get; set; }

    /// <summary>
    /// Color of status embed iframe text when displaying maintenance
    /// </summary>
    [JsonProperty("maintenance_text_color", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Maintenance_text_color { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Pages {
    private IDictionary<string, object> _additionalProperties;

    [JsonProperty("page_id", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Page_id Page_id { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Data3 {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// User identifier
    /// </summary>
    [JsonProperty("user_id", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string User_id { get; set; }

    /// <summary>
    /// Pages accessible by the user.
    /// </summary>
    [JsonProperty("pages", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public Pages2 Pages { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class User2 {
    private IDictionary<string, object> _additionalProperties;

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
    public string First_name { get; set; }

    [JsonProperty("last_name", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Last_name { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
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

  public enum TemplateUpdate_status {

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
    In_progress = 5,

    [System.Runtime.Serialization.EnumMember(Value = @"verifying")]
    Verifying = 6,

    [System.Runtime.Serialization.EnumMember(Value = @"completed")]
    Completed = 7,

  }

  public enum Incident2Status {

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
    In_progress = 5,

    [System.Runtime.Serialization.EnumMember(Value = @"verifying")]
    Verifying = 6,

    [System.Runtime.Serialization.EnumMember(Value = @"completed")]
    Completed = 7,

  }

  public enum Incident2Impact_override {

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

  public class Components {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// Map of status changes to apply to affected components
    /// </summary>
    [JsonProperty("2y6527s0bj94", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public Components_2y6527s0bj94 _2y6527s0bj94 { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public enum Incident3Status {

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
    In_progress = 5,

    [System.Runtime.Serialization.EnumMember(Value = @"verifying")]
    Verifying = 6,

    [System.Runtime.Serialization.EnumMember(Value = @"completed")]
    Completed = 7,

  }

  public enum Incident3Impact_override {

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

  public class Components2 {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// Map of status changes to apply to affected components
    /// </summary>
    [JsonProperty("xb63q9zglmyk", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public Components2Xb63q9zglmyk Xb63q9zglmyk { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public enum Incident4Status {

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
    In_progress = 5,

    [System.Runtime.Serialization.EnumMember(Value = @"verifying")]
    Verifying = 6,

    [System.Runtime.Serialization.EnumMember(Value = @"completed")]
    Completed = 7,

  }

  public enum Incident4Impact_override {

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

  public class Components3 {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// Map of status changes to apply to affected components
    /// </summary>
    [JsonProperty("xb63q9zglmyk", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public Components3Xb63q9zglmyk Xb63q9zglmyk { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public enum Component2Status {

    [System.Runtime.Serialization.EnumMember(Value = @"operational")]
    Operational = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"under_maintenance")]
    Under_maintenance = 1,

    [System.Runtime.Serialization.EnumMember(Value = @"degraded_performance")]
    Degraded_performance = 2,

    [System.Runtime.Serialization.EnumMember(Value = @"partial_outage")]
    Partial_outage = 3,

    [System.Runtime.Serialization.EnumMember(Value = @"major_outage")]
    Major_outage = 4,

    [System.Runtime.Serialization.EnumMember(Value = @"")]
    Empty = 5,

  }

  public enum Component3Status {

    [System.Runtime.Serialization.EnumMember(Value = @"operational")]
    Operational = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"under_maintenance")]
    Under_maintenance = 1,

    [System.Runtime.Serialization.EnumMember(Value = @"degraded_performance")]
    Degraded_performance = 2,

    [System.Runtime.Serialization.EnumMember(Value = @"partial_outage")]
    Partial_outage = 3,

    [System.Runtime.Serialization.EnumMember(Value = @"major_outage")]
    Major_outage = 4,

    [System.Runtime.Serialization.EnumMember(Value = @"")]
    Empty = 5,

  }

  public enum Component4Status {

    [System.Runtime.Serialization.EnumMember(Value = @"operational")]
    Operational = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"under_maintenance")]
    Under_maintenance = 1,

    [System.Runtime.Serialization.EnumMember(Value = @"degraded_performance")]
    Degraded_performance = 2,

    [System.Runtime.Serialization.EnumMember(Value = @"partial_outage")]
    Partial_outage = 3,

    [System.Runtime.Serialization.EnumMember(Value = @"major_outage")]
    Major_outage = 4,

    [System.Runtime.Serialization.EnumMember(Value = @"")]
    Empty = 5,

  }

  public class Incidents {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// Incident identifier
    /// </summary>
    [JsonProperty("id", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Page_id {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// Whether or not user should have page configuration role. This field will only be present for pages with Role Based Access Control.
    /// </summary>
    [JsonProperty("page_configuration", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Page_configuration { get; set; }

    /// <summary>
    /// Whether or not user should have incident manager role. This field will only be present for pages with Role Based Access Control.
    /// </summary>
    [JsonProperty("incident_manager", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Incident_manager { get; set; }

    /// <summary>
    /// Whether or not user should have maintenance manager role. This field will only be present for pages with Role Based Access Control.
    /// </summary>
    [JsonProperty("maintenance_manager", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Maintenance_manager { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public class Pages2 {
    private IDictionary<string, object> _additionalProperties;

    /// <summary>
    /// Page identifier
    /// </summary>
    [JsonProperty("page_id", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public string Page_id { get; set; }

    /// <summary>
    /// User has page configuration role. This field will only be present if the organization has Role Based Access Control.
    /// </summary>
    [JsonProperty("page_configuration", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Page_configuration { get; set; }

    /// <summary>
    /// User has incident manager role. This field will only be present if the organization has Role Based Access Control.
    /// </summary>
    [JsonProperty("incident_manager", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Incident_manager { get; set; }

    /// <summary>
    /// User has maintenance manager role. This field will only be present if the organization has Role Based Access Control.
    /// </summary>
    [JsonProperty("maintenance_manager", Required = Required.DisallowNull,
        NullValueHandling = NullValueHandling.Ignore)]
    public bool Maintenance_manager { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties {
      get {
        return _additionalProperties ??
               (_additionalProperties = new Dictionary<string, object>());
      }
      set { _additionalProperties = value; }
    }
  }

  public enum Components_2y6527s0bj94 {

    [System.Runtime.Serialization.EnumMember(Value = @"operational")]
    Operational = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"under_maintenance")]
    Under_maintenance = 1,

    [System.Runtime.Serialization.EnumMember(Value = @"degraded_performance")]
    Degraded_performance = 2,

    [System.Runtime.Serialization.EnumMember(Value = @"partial_outage")]
    Partial_outage = 3,

    [System.Runtime.Serialization.EnumMember(Value = @"major_outage")]
    Major_outage = 4,

    [System.Runtime.Serialization.EnumMember(Value = @"")]
    Empty = 5,

  }

  public enum Components2Xb63q9zglmyk {

    [System.Runtime.Serialization.EnumMember(Value = @"operational")]
    Operational = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"under_maintenance")]
    Under_maintenance = 1,

    [System.Runtime.Serialization.EnumMember(Value = @"degraded_performance")]
    Degraded_performance = 2,

    [System.Runtime.Serialization.EnumMember(Value = @"partial_outage")]
    Partial_outage = 3,

    [System.Runtime.Serialization.EnumMember(Value = @"major_outage")]
    Major_outage = 4,

    [System.Runtime.Serialization.EnumMember(Value = @"")]
    Empty = 5,

  }

  public enum Components3Xb63q9zglmyk {

    [System.Runtime.Serialization.EnumMember(Value = @"operational")]
    Operational = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"under_maintenance")]
    Under_maintenance = 1,

    [System.Runtime.Serialization.EnumMember(Value = @"degraded_performance")]
    Degraded_performance = 2,

    [System.Runtime.Serialization.EnumMember(Value = @"partial_outage")]
    Partial_outage = 3,

    [System.Runtime.Serialization.EnumMember(Value = @"major_outage")]
    Major_outage = 4,

    [System.Runtime.Serialization.EnumMember(Value = @"")]
    Empty = 5,

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