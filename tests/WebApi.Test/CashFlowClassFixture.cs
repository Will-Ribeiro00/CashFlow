using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebApi.Test
{
    public class CashFlowClassFixture : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;

        public CashFlowClassFixture(CustomWebApplicationFactory webApplicationFactory)
        {
            _httpClient = webApplicationFactory.CreateClient();
        }

        protected async Task<HttpResponseMessage> DoPost(string uri, object request, string token = "", string culture = "en")
        {
            AuthorizeRequest(token);
            ChangeRequestCulture(culture);

            return await _httpClient.PostAsJsonAsync(uri, request);
        }
        protected async Task<HttpResponseMessage> DoPut(string uri, object request, string token, string culture = "en")
        {
            AuthorizeRequest(token);
            ChangeRequestCulture(culture);

            return await  _httpClient.PutAsJsonAsync(uri, request);
        }
        protected async Task<HttpResponseMessage> DoGet(string uri, string token, string culture = "en")
        {
            AuthorizeRequest(token);
            ChangeRequestCulture(culture);

            return await _httpClient.GetAsync(uri);
        }
        protected async Task<HttpResponseMessage> DoDelete(string uri, string token, string culture = "en")
        {
            AuthorizeRequest(token);
            ChangeRequestCulture(culture);

            return await _httpClient.DeleteAsync(uri);
        }

        private void AuthorizeRequest(string token)
        {
            if (!string.IsNullOrWhiteSpace(token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        private void ChangeRequestCulture(string culture)
        {
            _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
            _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));
        }
    }
}
