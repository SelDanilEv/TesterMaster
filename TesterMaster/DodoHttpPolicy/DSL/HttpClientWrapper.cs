namespace TesterMaster.DodoHttpPolicy.DSL
{
	using System.Net.Http;
    using TesterMaster.DodoHttpPolicy.Fakes;

    public class HttpClientWrapper
	{
		private readonly HttpClient _client;
		private readonly MockHttpMessageHandler _handler;

		public HttpClient Client => _client;
		public long NumberOfCalls => _handler.NumberOfCalls;

		public HttpClientWrapper(HttpClient client, MockHttpMessageHandler handler)
		{
			_client = client;
			_handler = handler;
		}
	}
}
