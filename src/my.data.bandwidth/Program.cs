using Microsoft.Extensions.DependencyInjection;
using My.Data.Bandwidth.Handlers;
using My.Data.Bandwidth.Models;
using System;
using System.Linq;
using System.Net.Http;

namespace My.Data.Bandwidth
{
    static partial class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("My.Data.Bandwidth Initialising...");

            var arguments = args.Select(x => x.ToLower()).ToList();

            var test = new SpeedTestHandler();
            var persist = new PersistenceHandler<SpeedTestDataModel>();

            var httpClientFactory = new ServiceCollection()
                                               .AddHttpClient()
                                               .BuildServiceProvider()
                                               .GetService<IHttpClientFactory>();

            var transmit = new TransmitHandler<SpeedTestDataModel>(httpClientFactory);
            var announce = new ConsoleOutputHandler<SpeedTestDataModel>();
            var process = new ResultHandler<SpeedTestDataModel>();

            var success = true;

            Console.WriteLine("My.Data.Bandwidth Starting...");
            while (success)
            {
                var result = test.Execute(DateTimeOffset.UtcNow).GetAwaiter().GetResult();

                // abort on error if instructed, else continue forever
                if (
                    arguments.Any() &&
                    arguments.Contains("abortonerror") &&
                    result.IsError)
                {
                    // error and exit
                    success = !result.IsError;
                    persist.Execute(result).Wait();
                    bool transmitViaAPI = false; // TODO via settings
                    if (transmitViaAPI)
                    {
                        transmit.Execute(result).Wait();
                    }
                    announce.Execute(result).Wait();
                }
                else if (result.IsError)
                {
                    // error and repeat
                    persist.Execute(result).Wait();
                    bool transmitViaAPI = false; // TODO via settings
                    if (transmitViaAPI)
                    {
                        transmit.Execute(result).Wait();
                    }
                    announce.Execute(result).Wait();
                }
                else
                {
                    // all is well
                    persist.Execute(result).Wait();
                    bool transmitViaAPI = false; // TODO via settings
                    if (transmitViaAPI)
                    {
                        transmit.Execute(result).Wait();
                    }
                    announce.Execute(result).Wait();
                    process.Execute(result).Wait();
                }
            }
            return 1; // always returns an error - because it should run forever.
        }
    }
}
