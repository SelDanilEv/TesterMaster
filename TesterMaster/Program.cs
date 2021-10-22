namespace TesterMaster
{
    using Microsoft.CSharp.RuntimeBinder;
    using Newtonsoft.Json;
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using TesterMaster.FileSystem;
    using TesterMaster.Polly;

    class Program
    {
        public static void Main()
        {
            try
            {
                MainAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error {ex.Message}");
            }
            Console.ReadKey();
        }

        private static async Task MainAsync()
        {
            var test = new TestFileSystem();
            test.Start();
        }
    }
}
