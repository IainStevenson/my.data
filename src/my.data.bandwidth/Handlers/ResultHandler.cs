using My.Data.Bandwidth.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace My.Data.Bandwidth.Handlers
{
    public class ResultHandler<T> where T: ISpeedTestData
    {
        private static int ONE_SECOND_MILLSECONDS = 1000;
        private static int DATAPOINT_INTERVAL_SECONDS = 300;
        /// <summary>
        /// Processes the result and determines the next start time then waits until that has expired.
        /// </summary>
        /// <param name="result">the result model instance</param>
        /// <returns><see cref="Task"/></returns>
        public Task Execute(T result)
        {
            if (!result.IsError)
            {
                result.TimeStamp =  result.TimeStamp.AddSeconds(DATAPOINT_INTERVAL_SECONDS); // await the next interval

                while (DateTime.UtcNow.Ticks < result.TimeStamp.Ticks)  // until time exceeded
                {
                    Thread.Sleep(ONE_SECOND_MILLSECONDS); // have a wee kip
                }
                
            }
            return Task.FromResult(0);
        }    
    }
}