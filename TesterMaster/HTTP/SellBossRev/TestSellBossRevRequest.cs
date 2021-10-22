namespace TesterMaster.HTTP.SellBossRev
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class TestSellBossRevRequest
    {
        public async Task DoTestRequest()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://sandbox.zohoapis.com");
                var jsonString = JsonSerializer.Serialize(new
                {
                    first_name = "Name",
                    last_name = "LastName",
                    company_name = "Company",
                    street_address1 = "520 Broad St",
                    city = "New York",
                    state = "NY",
                    country = "USA",
                    street_address2 = "12345",
                    location_type = "Individual Seller",
                    potential_vend_telephone = "8884121577",
                    potential_vend_email = "test@test.test",
                    source = "Newspaper",
                    message = "Some additional info"
                });
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var result = await client.PostAsync("/crm/...", content);
                var resultContent = await result.Content.ReadAsStringAsync();
            }
        }

    }
}
