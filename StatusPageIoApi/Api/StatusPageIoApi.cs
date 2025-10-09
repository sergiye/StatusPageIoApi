using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace SergiyE.StatusPageIoApi {

  public partial class StatusPageIoApi {
    
    private readonly HttpClient httpClient;
    private readonly JsonSerializerSettings jsonSettings;

    public StatusPageIoApi(string apiKey) {
      httpClient = new HttpClient();
      httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", apiKey);
      httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      //httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
      jsonSettings = new JsonSerializerSettings {
        NullValueHandling = NullValueHandling.Ignore,
        ContractResolver = new CamelCasePropertyNamesContractResolver {
          NamingStrategy = new SnakeCaseNamingStrategy()
        }
      };
      jsonSettings.Converters.Add(new StringEnumConverter());
    }

    private string BaseUrl { get; set; } = "https://api.statuspage.io/v1";

    /// <summary>
    /// Get status embed config settings
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Get status embed config settings</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<StatusEmbedConfig> GetStatusEmbedConfig(string pageId,
      CancellationToken cancellationToken = default(CancellationToken)) {
      if (pageId == null)
        throw new ArgumentNullException(nameof(pageId));

      var urlBuilder = new StringBuilder();
      urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "")
        .Append("/pages/{page_id}/status_embed_config");
      urlBuilder.Replace("{page_id}",
        Uri.EscapeDataString(ConvertToString(pageId,
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
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Update status embed config settings
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="body"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Update status embed config settings</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<StatusEmbedConfig> PatchStatusEmbedConfig(
      string pageId, PatchStatusEmbedConfig body,
      CancellationToken cancellationToken = default(CancellationToken)) {
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

      using (var request = new HttpRequestMessage()) {
        var json = JsonConvert.SerializeObject(body, jsonSettings);
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
          response.Dispose();
        }
      }
    }

    /// <summary>
    /// Update status embed config settings
    /// </summary>
    /// <param name="pageId">Page identifier</param>
    /// <param name="body"></param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Update status embed config settings</returns>
    /// <exception cref="ApiException">A server side error occurred.</exception>
    public virtual async Task<StatusEmbedConfig> PutStatusEmbedConfig(
      string pageId, PutStatusEmbedConfig body,
      CancellationToken cancellationToken = default(CancellationToken)) {
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
          response.Dispose();
        }
      }
    }

    protected virtual async Task<ObjectResponseResult<T>> ReadObjectResponseAsync<T>(HttpResponseMessage response,
      IReadOnlyDictionary<string, IEnumerable<string>>
        headers, CancellationToken cancellationToken = default(CancellationToken)) {
      if (response == null)
        return new ObjectResponseResult<T>(default(T), string.Empty);

      var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
      try {
        var typedBody = JsonConvert.DeserializeObject<T>(responseText, jsonSettings);
        return new ObjectResponseResult<T>(typedBody, responseText);
      }
      catch (JsonException exception) {
        throw new ApiException("Could not deserialize the response body string as " + typeof(T).FullName + ".", (int)response.StatusCode, responseText, headers, exception);
      }
    }

    private static string ConvertToString(object value, CultureInfo cultureInfo) {
      switch (value) {
        case null:
          return "";
        case Enum: {
            var name = Enum.GetName(value.GetType(), value);
            if (name != null) {
              var field = System.Reflection.IntrospectionExtensions.GetTypeInfo(value.GetType())
                .GetDeclaredField(name);
              if (field != null && System.Reflection.CustomAttributeExtensions.GetCustomAttribute(field, typeof(EnumMemberAttribute)) is EnumMemberAttribute attribute) {
                return attribute.Value ?? name;
              }

              var converted = Convert.ToString(Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType()), cultureInfo));
              return converted;
            }
            break;
          }
        case bool b:
          return Convert.ToString(b, cultureInfo).ToLowerInvariant();
        case byte[] bytes:
          return Convert.ToBase64String(bytes);
        default: {
            if (value.GetType().IsArray) {
              var array = ((Array)value).OfType<object>();
              return string.Join(",", array.Select(o => ConvertToString(o, cultureInfo)));
            }

            break;
          }
      }

      var result = Convert.ToString(value, cultureInfo);
      return result ?? "";
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
}