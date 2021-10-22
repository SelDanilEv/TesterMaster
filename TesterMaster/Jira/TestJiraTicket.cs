namespace TesterMaster.Jira
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Http.Formatting;
    using System.IO;
    using Atlassian.Jira;

    class TestJiraTicket
    {
        public void TestAtlassianPackage()
        {
            var jira = Jira.CreateRestClient("https://idtjira.atlassian.net/", "login", "password/token");

            var issue = jira.CreateIssue("project");
            issue.Type = "Bug";
            issue.Priority = "Major";
            issue.Summary = "Issue Summary";
            issue.SaveChangesAsync();

            var issues = from i in jira.Issues.Queryable
                         select i;

            Console.WriteLine(issues.Count());

            foreach (var elem in issues)
            {
                Console.WriteLine(elem.Summary);
                Console.WriteLine("-----------------");
            }
        }

        public void TestCreateTicket()
        {
            string data = @"{""fields"":{""project"":{""key"": ""fitpoit4""},""summary"": ""REST EXAMPLE"",""description"":""Creating an issue via REST API"",""issuetype"": {""name"": ""Bug""},""mrNumber"": {""name"": ""AnyMrNumber what i will send later""}}}";
            string postUrl = "http://fitpoit4.atlassian.net/rest/api/latest/issue/";

            HttpClient client = new HttpClient();

            client.BaseAddress = new System.Uri(postUrl);

            byte[] cred = UTF8Encoding.UTF8.GetBytes("fit");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(cred));

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            MediaTypeFormatter jsonFormatter = new JsonMediaTypeFormatter();
            //System.Net.Http.HttpContent content = new System.Net.Http.ObjectContent<Issue>(data, jsonFormatter);
            HttpContent content = new ObjectContent<string>(data, jsonFormatter);
            HttpResponseMessage response = client.PostAsync("/secure/CreateIssueDetails", content).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                //descrptiontextBox.Text = result;
                //MrNumber = result;
                File.WriteAllText("D://Trash//indx.html", result);
                Console.WriteLine("OK");
            }
            else
            {
                //descrptiontextBox.Text = response.StatusCode.ToString();
                //MrNumber = response.StatusCode.ToString();
                Console.WriteLine(response.StatusCode.ToString());
            }





            //Chilkat.Rest rest = new Chilkat.Rest();
            //bool success;

            ////  URL: https://your-domain.atlassian.net/rest/api/2/issue
            //bool bTls = true;
            //int port = 443;
            //bool bAutoReconnect = true;
            //success = rest.Connect("your-domain.atlassian.net", port, bTls, bAutoReconnect);
            //if (success != true)
            //{
            //    Debug.WriteLine("ConnectFailReason: " + Convert.ToString(rest.ConnectFailReason));
            //    Debug.WriteLine(rest.LastErrorText);
            //    return;
            //}

            //rest.SetAuthBasic("jira@example.com", "JIRA_API_TOKEN");

            //Chilkat.JsonObject json = new Chilkat.JsonObject();
            //json.UpdateString("fields.project.id", "10000");
            //json.UpdateString("fields.summary", "something is wrong");
            //json.UpdateString("fields.issuetype.id", "10000");
            //json.UpdateString("fields.assignee.name", "matt");
            //json.UpdateString("fields.priority.id", "3");
            //json.UpdateString("fields.labels[0]", "bugfix");
            //json.UpdateString("fields.labels[1]", "blitz_test");
            //json.UpdateString("fields.description", "description");
            //json.UpdateString("fields.fixVersions[0].id", "10001");
            //json.UpdateString("fields.customfield_10005", "blah blah");

            //rest.AddHeader("Content-Type", "application/json");
            //rest.AddHeader("Accept", "application/json");

            //Chilkat.StringBuilder sbRequestBody = new Chilkat.StringBuilder();
            //json.EmitSb(sbRequestBody);
            //Chilkat.StringBuilder sbResponseBody = new Chilkat.StringBuilder();
            //success = rest.FullRequestSb("POST", "/rest/api/2/issue", sbRequestBody, sbResponseBody);
            //if (success != true)
            //{
            //    Debug.WriteLine(rest.LastErrorText);
            //    return;
            //}

            //int respStatusCode = rest.ResponseStatusCode;
            //if (respStatusCode >= 400)
            //{
            //    Debug.WriteLine("Response Status Code = " + Convert.ToString(respStatusCode));
            //    Debug.WriteLine("Response Header:");
            //    Debug.WriteLine(rest.ResponseHeader);
            //    Debug.WriteLine("Response Body:");
            //    Debug.WriteLine(sbResponseBody.GetAsString());
            //    return;
            //}

            //Chilkat.JsonObject jsonResponse = new Chilkat.JsonObject();
            //jsonResponse.LoadSb(sbResponseBody);

            //string id;
            //string key;
            //string self;

            //id = jsonResponse.StringOf("id");
            //key = jsonResponse.StringOf("key");
            //self = jsonResponse.StringOf("self");
        }
    }
}