namespace TesterMaster
{
    using Microsoft.CSharp.RuntimeBinder;
    using Newtonsoft.Json;
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using TesterMaster.Dictionary;
    using TesterMaster.FileSystem;
    using TesterMaster.Polly;
    using TestMasterInfrastructure.Interfaces;

    class Program
    {
        static ITest Test = new TestDictionary();

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
            Test.StartTest();
        }
    }
}
