namespace TesterMaster.DodoHttpPolicy
{
	using System.Net.Http;
	using System.Threading.Tasks;
	public static class Helper
	{
		public static async Task InvokeMultipleHttpRequests(HttpClient client, int taskCount, string uri = "http://localhost")
		{
			var tasks = new Task[taskCount];
			for (var i = 0; i < taskCount; i++)
			{
				using var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
				requestMessage.Headers.Add("TaskId", i.ToString());
				tasks[i] = client.SendAsync(requestMessage);
			}

			await Task.WhenAll(tasks);
		}
	}
}
