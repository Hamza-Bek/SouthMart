using System.Net.Http.Headers;


namespace Application.Extensions
{
    public class HttpClientService(IHttpClientFactory httpClientFactory, LocalStorageService localStorageService)
    {
        private HttpClient CreateClient() => httpClientFactory!.CreateClient(Constants.HttpClientName);
        public HttpClient GetPublicClient() => CreateClient();

        public async Task<HttpClient> GetPrivateClient()
        {
            try
            {
                var client = CreateClient();
                var localStorageDTO = await localStorageService.GetModelFromToken();
                if (string.IsNullOrEmpty(localStorageDTO.Token))
                    return client;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.HttpClientHeaderScheme, localStorageDTO.Token);
                return client;
            }
            catch
            {
                return new HttpClient();
            }
        }
    }
}
