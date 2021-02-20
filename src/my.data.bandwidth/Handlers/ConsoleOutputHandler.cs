using My.Data.Bandwidth.Models;
using System;
using System.Threading.Tasks;

namespace My.Data.Bandwidth.Handlers
{
    public class ConsoleOutputHandler<T> where T: ISpeedTestData
    {
        /// <summary>
        /// Announces the results to the console.
        /// </summary>
        /// <param name="result">the result model instance</param>
        /// <returns><see cref="Task"/></returns>
        public Task Execute(T result)
        {
            var timeString = $"{result.TimeStamp:yyyy-MM-dd-T-HH-mm-ss-ffffff}";
            if (result.IsError)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{timeString} *** Error Data: [{result.ErrorOutput}]");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{timeString} Speed Data: [{result.DataOutput}]");
                Console.ForegroundColor = ConsoleColor.White;

            }
            return Task.FromResult(0);
        }
    }
}