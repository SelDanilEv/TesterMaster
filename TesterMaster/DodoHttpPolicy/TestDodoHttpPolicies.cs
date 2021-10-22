namespace TesterMaster.DodoHttpPolicy
{
    using Polly;
    using System;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Threading;
    using Dodo.HttpClientResiliencePolicies;
    using Dodo.HttpClientResiliencePolicies.RetryPolicy;
    using Dodo.HttpClientResiliencePolicies.CircuitBreakerPolicy;
    using TesterMaster.DodoHttpPolicy.DSL;
    using System.Net;
    using global::Polly.CircuitBreaker;

    class TestDodoHttpPolicies
    {
        private int _successfulRequestCounter;
        private readonly Random _random = new Random();
        private const string _enternalServerErrorUrl = "http://httpstat.us/500";

        private ResiliencePoliciesSettings settings = new ResiliencePoliciesSettings
        {
            OverallTimeout = TimeSpan.FromSeconds(50),
            TimeoutPerTry = TimeSpan.FromSeconds(2),
            RetryPolicySettings = RetryPolicySettings.Jitter(5, TimeSpan.FromMilliseconds(50)),
            CircuitBreakerPolicySettings = new CircuitBreakerPolicySettings(
                failureThreshold: 0.5,
                minimumThroughput: 10,
                durationOfBreak: TimeSpan.FromSeconds(5),
                samplingDuration: TimeSpan.FromSeconds(30)
            ),
            OnRetry = (response, time) => { Console.WriteLine("Retry policy"); },      // Handle retry event. For example you may add logging here
            OnBreak = (response, time) => { Console.WriteLine("Break"); },      // Handle CircuitBreaker break event. For example you may add logging here
            OnReset = () => { Console.WriteLine("Reset"); },                      // Handle CircuitBreaker reset event. For example you may add logging here
            OnHalfOpen = () => { Console.WriteLine("Half open"); }                   // Handle CircuitBreaker reset event. For example you may add logging here
        };

        public async Task<string> StartTest(string uri = "http://google.com/", int successfulTimes = 10)
        {
            var response = await Make(uri, successfulTimes);
            Console.WriteLine(response);
            return response;
        }

        private async Task<string> Make(string uri, int successfulTimes)
        {
            var wrapper = Create.HttpClientWrapperWrapperBuilder
                .WithStatusCode(HttpStatusCode.OK)
                .WithResiliencePolicySettings(settings)
                .Please();


            while (_successfulRequestCounter < successfulTimes)
            {
                var watch = new Stopwatch();
                watch.Start();

                try
                {
                    // This code is executed within the circuitBreakerPolicy 
                    HttpResponseMessage result;
                    if (_random.Next(0, 5) % 2 == 1)
                    {
                        result = await wrapper.Client.GetAsync(uri);
                    }
                    else
                    {
                        result = await wrapper.Client.GetAsync(_enternalServerErrorUrl);
                    }

                    if(result.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception();
                    }

                    watch.Stop();
                    Console.WriteLine("Response : " + "OK" + " (after " + watch.ElapsedMilliseconds + "ms)");

                    _successfulRequestCounter++;
                }
                catch (BrokenCircuitException b)
                {
                    watch.Stop();

                    Console.WriteLine("Request failed with BrokenCircuitException");
                }
                catch (Exception e)
                {
                    watch.Stop();

                    Console.WriteLine("Request failed with another exception");
                }

                // Wait half second
                await Task.Delay(TimeSpan.FromSeconds(0.5));
            }

            await wrapper.Client.GetAsync("");

            return "";
        }
    }
}
