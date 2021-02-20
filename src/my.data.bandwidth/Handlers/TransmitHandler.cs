using My.Data.Bandwidth.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace My.Data.Bandwidth.Handlers
{
    public class TransmitHandler<T> where T : ISpeedTestData
    {
        private IHttpClientFactory _clientFactory;
        public TransmitHandler(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// Transmits the result model to a remote API. 
        /// Sends the success and error data to different endpoints. 
        /// Will also clean up any failed transmissions from previous calls, in filename (which is in ascending temporal) order.
        /// Deletes the data files of successfully transmited files
        /// </summary>
        /// <param name="result">The result model instance</param>
        /// <returns><see cref="Task"/></returns>
        public async Task Execute(T result)
        {
            var files = new DirectoryInfo(@".\").EnumerateFiles("????-??-??-T-??-??-??-??????.json");
            Console.WriteLine($"Transmitting {result.TimeStamp} {(result.IsError ? "error" : "data")} ...");
            var filesToDelete = new List<FileInfo>();
            foreach (var file in files.Where(x => x.Length > 0))
            {
                Console.WriteLine($"Transmitting {file.FullName} ...");
                var data = JsonConvert.DeserializeObject<T>(File.ReadAllText(file.FullName));
                if (data != null)
                {
                    var client = _clientFactory.CreateClient();
                    var baseUri = new Uri("https://localhost/");
                    client.BaseAddress = baseUri;
                    
                    Uri requestUri ;
                    HttpContent requestContent ;
                    HttpResponseMessage response;
                    if (data.IsError)
                    {
                        requestContent = new StringContent($"{data.ErrorOutput}", Encoding.UTF8,
                                    "application/json");
                        requestUri = new Uri(baseUri, "api/Network/Bandwidth/Error");
                    }
                    else
                    {
                        requestContent = new StringContent($"{data.DataOutput}", Encoding.UTF8,
                                    "application/json");
                        requestUri = new Uri(baseUri, "api/Network/bandwidth/data");
                    }
                    try
                    {
                        response = await client.PostAsync(requestUri, requestContent);
                        if (response.IsSuccessStatusCode)
                        {
                            // delete file
                            filesToDelete.Add(file);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine($"Call to {requestUri.AbsoluteUri} failed: {ex}");
                        //throw ex;
                    }
                    finally { }
                }
            }
            foreach(var file in filesToDelete)
            {
                //file.Delete();
                System.Diagnostics.Trace.WriteLine($"Deleting {file.FullName}");
            }            
        }
    }
}
