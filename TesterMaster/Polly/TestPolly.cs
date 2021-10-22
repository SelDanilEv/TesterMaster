namespace TesterMaster.Polly
{
    using global::Polly;
    using global::Polly.CircuitBreaker;
    using System;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Threading.Tasks;
    using TesterMaster.Exceptions.Generator;

    class TestPolly
    {
        private int _successfulRequestCounter;
        private readonly Random _random = new Random();
        private const string _enternalServerErrorUrl = "http://httpstat.us/429";

        public async Task<string> StartPollyTest(string uri = "http://google.com/", int successfulTimes = 10)
        {
            var response = await Make(uri, successfulTimes);
            Console.WriteLine(response);
            return response;
        }

        public async Task<HttpResponseMessage> StartPollyTestRetry()
        {
            var httpClient = new HttpClient();
            var maxRetryAttempts = 2;
            var pauseBetweenFailures = TimeSpan.FromSeconds(1);

            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(maxRetryAttempts, i => pauseBetweenFailures);

            var circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(
                4,
                TimeSpan.FromSeconds(10));


            string url = "http://httpstat.us/200";

            HttpResponseMessage response = null;

            ExceptionGenerator.SetChanceOfError(99);

            var polcomb = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);

            try
            {
                await polcomb.ExecuteAsync(async () =>
                {
                    Console.WriteLine("Make circuit breaker");
                    ExceptionGenerator.ThrowRandomException();
                    response = await httpClient.GetAsync(url);
                });
            }
            catch (BrokenCircuitException b)
            {
                Console.WriteLine("Request failed with BrokenCircuitException");
            }
            catch (Exception e)
            {
                return response;
            }

            return response;
        }

        private async Task<string> Make(string uri, int successfulTimes)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            // Define our waitAndRetry policy: keep retrying with 200ms gaps.
            var waitAndRetryPolicy = Policy
                .Handle<Exception>(e => !(e is BrokenCircuitException)) // Exception filtering!  We don't retry if the inner circuit-breaker judges the underlying system is out of commission!
                .WaitAndRetryForeverAsync(
                    attempt => TimeSpan.FromMilliseconds(200),
                    (exception, calculatedWaitDuration) =>
                    {
                        // This is your new exception handler! 
                        // Tell the user what they've won!
                        Console.WriteLine("Retry policy");
                    });

            // Define our CircuitBreaker policy: Break if the action fails 4 times in a row.
            var circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(
                    4,
                    TimeSpan.FromSeconds(3),
                    (ex, breakDelay) =>
                    {
                        Console.WriteLine("CircuitBreakerPolicy");
                    },
                    () => Console.WriteLine("Break"),
                    () => Console.WriteLine("Half-open: Next call is a trial!")
                );

            using (var client = new HttpClient())
            {
                // Do the following until a key is pressed
                while (_successfulRequestCounter < successfulTimes)
                {
                    var watch = new Stopwatch();
                    watch.Start();

                    try
                    {
                        // Retry the following call according to the policy - 3 times.
                        await waitAndRetryPolicy.ExecuteAsync(async () =>
                        {
                            // This code is executed within the waitAndRetryPolicy 

                            response = await circuitBreakerPolicy.ExecuteAsync<HttpResponseMessage>(
                                async () => // Note how we can also Execute() a Func<TResult> and pass back the value.
                                {
                                    // This code is executed within the circuitBreakerPolicy 
                                    HttpResponseMessage result;
                                    if (_random.Next(0, 5) % 5 == 1)
                                    {
                                        result = await client.GetAsync(uri);
                                    }
                                    else
                                    {
                                        result = await client.GetAsync(_enternalServerErrorUrl);
                                    }
                                    // Make a request and get a response
                                    return result;
                                });

                            response.EnsureSuccessStatusCode();

                            watch.Stop();

                            // Display the response message on the console
                            Console.WriteLine("Response : " + "OK" + " (after " + watch.ElapsedMilliseconds + "ms)");

                            _successfulRequestCounter++;
                        });
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
            }
            return response.StatusCode.ToString();
        }
    }
}
